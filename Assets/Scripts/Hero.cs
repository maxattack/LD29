using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HeroInput))]
[RequireComponent(typeof(HeroFX))]
[RequireComponent(typeof(Rigidbody))]
public class Hero : CustomBehaviour {
	
	// DESIGNER PARAMETERS
	public float runSpeed = 1f;
	public float jumpImpulse = 1f;
	
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
	int haltSemaphore = 0;
	float targetRunningSpeed = 0f;	
	
	//--------------------------------------------------------------------------------
	// EVENT CALLBACKS
	//--------------------------------------------------------------------------------

	void Awake() {
		
		// EAGERLY ACQUIRE SINGLETON REFERENCE
		inst = this;
		
		// CACHE COMMON SIBLINGS
		input = GetComponent<HeroInput>();
		fx = GetComponent<HeroFX>();
		body = this.rigidbody;
		xform = this.transform;
		
	}
	
	void OnDestroy() {
	
		// RELEASE SINGLETON REFERENCE
		if (inst == this) { inst = null; }
		
	}

	Vector2 currDir;

	void Start() {
		PollGrounded();
	}


		
		

	void Update() {

		Vector2 newDir = Vector2.zero;
		if (input.PressingUp)
			newDir.y += 1;
		if (input.PressingDown)
			newDir.y -= 1;
		if (input.PressingRight)
			newDir.x += 1;
		if (input.PressingLeft)
			newDir.x -= 1;

		newDir.Normalize ();

		if (newDir.sqrMagnitude > 0)
					currDir = newDir;



		// JUMPING
		if (grounded && input.PressedJump) {
			Jukebox.Play("Jump");
			body.AddForce(Vec(0, jumpImpulse, 0), ForceMode.VelocityChange);
		}

		if (currItem) {
						
			currItem.SetDir(currDir);
			currItem.SetOperatePos(xform.position);

				}
		if (input.PressedItem) {

			if(currItem)
				currItem.Operate(currDir);
				}
	}
	
	void PollGrounded() {
		grounded = Physics.CheckSphere(body.position, 0.1f, Layers.DefaultMask);
	}
	
	void FixedUpdate() {
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
	}

	//-------------------------------------------------------------------------------
	// COLLISION HANDLING
	//--------------------------------------------------------------------------------
	
	void OnCollisionEnter(Collision collision) {
		
		switch(collision.collider.gameObject.layer) {
			case Layers.Item:
				currItem = collision.collider.GetComponent<Item>();
				currItem.rigidbody.isKinematic = true;
				currItem.rigidbody.detectCollisions = false;	
				currItem.transform.parent = fx.highHand;
				currItem.transform.localPosition = Vector3.zero;
				break;
			case Layers.Camera:
				if (grounded) {
					Kill();
				}
				break;
		}
		
		
		
	}
	
	//--------------------------------------------------------------------------------
	// INTERACTION HALTING
	//--------------------------------------------------------------------------------
	
	public bool Halting { get { return haltSemaphore > 0; } }
	
	public void Halt() {
		++haltSemaphore;
	}
	
	public void Unhalt() {
		if (haltSemaphore > 0) { --haltSemaphore; }
	}
	
	//--------------------------------------------------------------------------------
	// KILL! KILL!
	//--------------------------------------------------------------------------------
	
	public void Kill() {
		if (status != Status.Dead) {
			status = Status.Dead;
			Time.timeScale = 0;
			Jukebox.Play("Kill");
			Music.KillMusic();
			print("KILL!");
		}
		
	}
				
}
