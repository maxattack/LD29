using UnityEngine;
using System.Collections;

public class HeroPose : CustomBehaviour {

	//--------------------------------------------------------------------------------
	// COLLECTS A HERO'S ARTICULATED LIMBS FOR SAMPLING AND SETTING
	// INDIVIDUAL JOINT ROTATIONS AS WELL AS PARAMETRIC FULL-BODY POSES
	//--------------------------------------------------------------------------------
	
	public Transform root;
	public Transform leftArm;
	public Transform rightArm;
	public Transform leftLeg;
	public Transform rightLeg;
	public Transform leftHandLocator;
	public Transform rightHandLocator;
	public Transform leftFootLocator;
	public Transform rightFootLocator;
	
	public void Reset() {
		root.Reset();
		leftArm.Reset();
		rightArm.Reset();
		leftLeg.Reset();
		rightLeg.Reset();
	}
	
	//--------------------------------------------------------------------------------
	// ROTATE INDIVIDUAL LIMBS
	// ARMS ON RANGE -1:0:1
	// LEGS ON RANGE 0:1
	//--------------------------------------------------------------------------------
	
	public void SetLeftArm(float u) { leftArm.localRotation = QDegrees(-90f * u); }
	public void SetRightArm(float u) { rightArm.localRotation = QDegrees(90f * u); }
	public void SetLeftLeg(float u) { leftLeg.localRotation = QDegrees(50f * u); }
	public void SetRightLeg(float u) { rightLeg.localRotation = QDegrees(-50f * u); }
	
	
	
}
