using UnityEngine;
using System.Collections;

public class PoseTest : MonoBehaviour {

	public HeroPose pose;
	
	void Update () {
		//pose.ApplyRunCycle(Time.time);		
		pose.ApplyJumpCycle(Time.time);
	}
}
