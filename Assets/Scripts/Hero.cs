using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HeroInput))]
public class Hero : CustomBehaviour {
	
	// DESIGNER PARAMETERS
	public float runSpeed = 1f;
	public float jumpImpulse = 1f;
	
	// INTERNAL PARAMETERS
	internal static Hero inst;
	internal HeroInput input;
	
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
		
	}
	
	void OnDestroy() {
	
		// RELEASE SINGLETON REFERENCE
		if (inst == this) { inst = null; }
		
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
