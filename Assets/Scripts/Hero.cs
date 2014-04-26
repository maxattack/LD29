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
	
	// PRIVATE MEMBERS	
	int haltSemaphore = 0;
	float targetRunningSpeed = 0f;	
	int groundCount = 0;
	
	//--------------------------------------------------------------------------------
	// GETTERS
	//--------------------------------------------------------------------------------

	public bool Grounded { get { return groundCount > 0; } }
	
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
	
	void Update() {
		
		// JUMPING
		if (Grounded && input.PressedJump) {
			Jukebox.Play("Jump");
			body.AddForce(Vec(0, jumpImpulse, 0), ForceMode.VelocityChange);
		}

		if (currItem) {
						currItem.xform = xform;

				}
		if (input.PressedItem) {

			if(currItem)
				currItem.Operate();
				}
	}
	
	void FixedUpdate() {
	
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
	
	void OnTriggerEnter(Collider c) {
		if (!c.isTrigger) {
			++groundCount;
		}
	}
	
	void OnTriggerExit(Collider c) {
		if (!c.isTrigger) {
			--groundCount;
		}
	}

	Item currItem;

	//-------------------------------------------------------------------------------
	// COLLISION HANDLING

	void OnCollisionEnter(Collision collision) {
		
		GameObject obj = collision.collider.gameObject;
		if (obj) {
			
			GameObjUserData ud = obj.GetComponent<GameObjUserData>();
			if(ud )
			{
				if(ud.goType == GameObjUserData.GOType.Item)
				{
					currItem = obj.GetComponent<Item>();
					currItem.rigidbody.isKinematic = true;
					currItem.rigidbody.detectCollisions = false;
				}
			}
			
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
}
