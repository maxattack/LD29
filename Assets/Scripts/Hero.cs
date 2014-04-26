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
	
	// PRIVATE MEMBERS	
	int haltSemaphore = 0;

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
		
	}
	
	void OnDestroy() {
	
		// RELEASE SINGLETON REFERENCE
		if (inst == this) { inst = null; }
		
	}
	
	void FixedUpdate() {
		var currVel = body.velocity;
		if (input.PressingLeft) {
			fx.SetDirection(HeroFX.Direction.Left);
			var targetVel = Vec(-runSpeed, 0, 0);
			body.AddForce(targetVel - currVel, ForceMode.VelocityChange);
		} else if (input.PressingRight) {
			fx.SetDirection(HeroFX.Direction.Right);
			var targetVel = Vec(runSpeed, 0, 0);
			body.AddForce(targetVel - currVel, ForceMode.VelocityChange);
		} else {
			var targetVel = Vector3.zero;
			body.AddForce(targetVel - currVel, ForceMode.Acceleration);
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
