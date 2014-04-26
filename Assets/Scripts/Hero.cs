﻿using System.Collections;
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

	Vector2 currDir;

	void Update() {

		//funky..make rawkit launcher look like one
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
		if (Grounded && input.PressedJump) {
			Jukebox.Play("Jump");
			body.AddForce(Vec(0, jumpImpulse, 0), ForceMode.VelocityChange);
		}

		if (currItem) {
						currItem.xform.position = xform.position + new Vector3(0,0.5f,0);
			currItem.SetDir(currDir);

				}
		if (input.PressedItem) {

			if(currItem)
				currItem.Operate(currDir);
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
