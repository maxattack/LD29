using UnityEngine;
using System.Collections;

public class RealTime : CustomBehaviour {
	
	// GETTING THE REAL DELTA TIME, FOR WHEN TIMESCALE IS SET TO ZERO
	// E.G. DURING DEATH OR PAUSE
	public static float DeltaTime { get { return GetInstance().deltaTime; } }
	
	//--------------------------------------------------------------------------------
	// BORING PRIVATE DETAILS
	//--------------------------------------------------------------------------------	
	
	static RealTime inst;
	float prevTime;
	float deltaTime;
	
	void Awake() {
		inst = this;
		prevTime = Time.realtimeSinceStartup;
		deltaTime = 0f;
	}
	
	void OnDestroy() {
		if (inst == this) { inst = null; }
	}
	
	void Update() {
		var nextTime = Time.realtimeSinceStartup;
		deltaTime = nextTime - prevTime;
		prevTime = nextTime;
	}
	
	static RealTime GetInstance() {
		if (inst == null) { new GameObject("RealTime", typeof(RealTime)); }
		return inst;
	}
	
}

