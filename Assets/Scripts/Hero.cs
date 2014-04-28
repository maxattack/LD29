using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HeroInput))]
[RequireComponent(typeof(Rigidbody))]
public class Hero : CustomBehaviour {
	
	// DESIGNER PARAMETERS
	public float runSpeed = 1f;
	public float jumpImpulse = 1f;
	public float doubleJumpImpulse = 1f;
	public float kickback = 10f;
	float maxSpeed = 32f;
	
	public GameObject killScreenPrefab;
	
	// INTERNAL PARAMETERS
	internal static Hero inst;
	internal HeroInput input;
	internal HeroFX fx;
	internal Rigidbody body;
	internal Transform xform;
	
	internal Item currItem;
	
	internal enum Status { Idle, Dead };
	internal Status status = Status.Idle;
	internal bool grounded = false;
	
	// PRIVATE MEMBERS	
	float targetRunningSpeed = 0f;
	internal Vector2 currDir;
	bool canDoubleJump;
	
	//--------------------------------------------------------------------------------
	// EVENT CALLBACKS
	//--------------------------------------------------------------------------------

	void Awake() {
		
		// EAGERLY ACQUIRE SINGLETON REFERENCE
		inst = this;
		
		// CACHE COMMON SIBLINGS
		input = GetComponent<HeroInput>();
		fx = GetComponentInChildren<HeroFX>();
		body = this.rigidbody;
		xform = this.transform;
		
	}
	
	void OnDestroy() {
	
		// RELEASE SINGLETON REFERENCE
		if (inst == this) { inst = null; }
		Time.timeScale = 1f;		
	}

	void Start() {
		PollGrounded();
	}
	
	void Update() {
		TickJump();
		TickCurrentItem();
	}
	
	void TickJump() {
		if (input.PressedJump) {
			if (grounded) {
				Jukebox.Play("Jump");
				grounded = false;
				body.AddForce(
					Vec(0, jumpImpulse - body.velocity.y, 0), 
					ForceMode.VelocityChange
				);
			} else if (canDoubleJump) {
				Jukebox.Play("Jump");
				canDoubleJump = false;
				body.AddForce(
					Vec(0, doubleJumpImpulse - body.velocity.y, 0), 
					ForceMode.VelocityChange
				);
			}
		
		}
	}
	
	void TickCurrentItem() {
		// COMPUTE ITEM DIRECITON
		Vector2 newDir = Vector2.zero;
		if (input.PressingUp) { newDir.y += 1; } 
		if (input.PressingDown) { newDir.y -= 1; }
		if (input.PressingRight) { newDir.x += 1; }
		if (input.PressingLeft) { newDir.x -= 1; }
		if (newDir.sqrMagnitude > 0) { currDir = newDir.normalized; }
		if (currDir.sqrMagnitude < 0.0001f) {
			currDir.x = fx.direction == HeroFX.Direction.Right ? 1 : -1;
		}
		
		// TICK ITEM DIRECTION
		if (currItem) {
			currItem.SetDir(currDir);
		}
		
		if (input.PressedItem) {
			if(currItem) { currItem.Operate(currDir); }
		}	
	}
	
	void PollGrounded() {
		var groundMask = Layers.TileMask | Layers.DefaultMask;
		grounded = status != Status.Dead && Physics.CheckSphere(body.position, 0.1f, groundMask);
		canDoubleJump |= grounded;
	}
	
	void FixedUpdate() {
		if (status == Status.Dead) { return; }
		
		PollGrounded();
		
		// RUNNING
		var vel = body.velocity;
		var easing = 0.2f;
		if (input.PressingLeft) {
			fx.SetDirection(HeroFX.Direction.Left);
			targetRunningSpeed = targetRunningSpeed.EaseTowards(-runSpeed, easing);
		} else if (input.PressingRight) {
			fx.SetDirection(HeroFX.Direction.Right);
			targetRunningSpeed = targetRunningSpeed.EaseTowards(runSpeed, easing);
		} else {
			targetRunningSpeed = targetRunningSpeed.EaseTowards(0, easing);
		}
		
		body.AddForce(Vec(targetRunningSpeed - vel.x, 0, 0), ForceMode.VelocityChange);
		
		vel = body.velocity;
		if (vel.sqrMagnitude > maxSpeed * maxSpeed) {
			body.velocity = body.velocity.normalized * maxSpeed;
		}
	}

	//-------------------------------------------------------------------------------
	// COLLISION HANDLING
	//--------------------------------------------------------------------------------
	
	void OnCollisionEnter(Collision collision) {
		
		switch(collision.collider.gameObject.layer) {
			case Layers.Item:
				var item = collision.collider.GetComponent<Item>();
				if (item.goodToCapture) { PickUp(item);	}
				break;
			case Layers.Camera:
				if (grounded) {
					Kill();
			} break;
		case Layers.Enemy:
			if(collision.collider.gameObject.GetComponent<Dino>() != null)
			{
				if(collision.collider.gameObject.GetComponent<Dino>().IsDead == false)
					RagdollKill(collision.transform.position);
			}
				break;
		}
		
		
		
	}

	//--------------------------------------------------------------------------------
	// INTERACTING WITH ITEMS
	//--------------------------------------------------------------------------------
	
	public void PickUp(Item item) {
		if (item == currItem) { return; }
	
		DropItem();
		
		
		Jukebox.Play("Pickup");
		
		currItem = item;
		
		// REPARENT FX TO THIS
		currItem.fx.parent = fx.pose.rightHand;
		currItem.fx.localPosition = Vector3.zero;
		currItem.fx.localScale = Vector3.one;
		
		currItem.StopPhysics();
		
		fx.Flash(Color.white);
		
		currItem.OnPickUp();
	}
	
	public void DropItem() {
		if (currItem != null) {
			
			currItem.StartPhysics(fx.direction == HeroFX.Direction.Right ?Vec(-8,16) : Vec(8,16), true);
			currItem.rigidbody.angularVelocity = Vec(0, 0, 360f);
			currItem.OnDrop();
			currItem = null;
			Jukebox.Play("DropWeapon");
			
		}
	}
	
	public void Kickback(Vector2 force) {

		body.AddForce(Vec(force.x,force.y,0), ForceMode.VelocityChange);

	}
	
	//--------------------------------------------------------------------------------
	// KILL! KILL!
	//--------------------------------------------------------------------------------
	
	public void RagdollKill(Vector2 sourcePosition) {
		if (status == Status.Dead) { return; }
		status = Status.Dead;
		StartCoroutine(DoFlyingDeath());
		DoKillscreen();
	}
	IEnumerator DoFlyingDeath() {
		fx.SetStatus(HeroFX.Status.Ragdoll);
		
		var height = 0.8f * (CameraFX.inst.xform.position.y + CameraFX.inst.HalfHeight - xform.position.y);
		var heightSqrt = Mathf.Sqrt(height);
		var metersPerSecond = 4f;
	
		body.isKinematic = true;
		body.detectCollisions = false;
		
		var sign = xform.position.x < CameraFX.inst.xform.position.x ? 1f : -1f;
		
		var p0 = body.position;
		var u = 0f;
		while(Time.deltaTime > 0.0001f) {
			yield return null;
			u += metersPerSecond * Time.deltaTime;
			xform.position = p0 + Vec(
				sign * u,
				height - (heightSqrt-u)*(heightSqrt-u),
				0
			);
			xform.rotation = QDegrees(-sign * 20f * u);
		}
	
	
	}
	
	
	public void Kill() {
		if (status == Status.Dead) { return; }
		status = Status.Dead;
		StartCoroutine(DoFallingDeath());
		DoKillscreen();
	}
	
	IEnumerator DoFallingDeath() {
		fx.SetStatus(HeroFX.Status.Corpse);
		body.isKinematic = true;
		body.detectCollisions = false;
		var metersPerSecond = 4f;
		var p0 = body.position;
		var u = 0f;
		while(Time.deltaTime > 0.0001f) {
			yield return null;
			u += metersPerSecond * Time.deltaTime;
			xform.position = p0 + Vec(u, -u * u, 0f);
			xform.rotation = QDegrees(20f * u);
		}
	}
	
	void DoKillscreen() {
		Jukebox.Play("MetalRiff" + Random.Range(0,3));
		DropItem();
		StartCoroutine(DoSlowDownTime());
		input.Halt();
		Jukebox.Play("Kill");
		Music.KillMusic();
		Dup(killScreenPrefab);
	}
	
	IEnumerator DoSlowDownTime() {
		var duration = 2.75f;
		for(var t=0f; t<duration; t+=RealTime.DeltaTime) {
			yield return null;
			Time.timeScale = 1f-EaseOut4(t / duration);
		}
		Time.timeScale = 0f;
	}
				
}
