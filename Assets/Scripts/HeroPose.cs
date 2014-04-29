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
	public Transform leftHand;
	public Transform rightHand;
	public Transform leftFoot;
	public Transform rightFoot;
	
	internal bool showing = true;
	internal SpriteRenderer[] sprites;
	
	void Awake() {
		sprites = GetComponentsInChildren<SpriteRenderer>();
	}

	public Sprite girlTorso;
	public Sprite boyTorso;

	public void SetGirl(bool g)
	{
		if(g)
		{
			sprites[4].sprite = girlTorso;
		}
		else
		{
			sprites[4].sprite = boyTorso;
		}
	}
	
	public void Show(bool flag) {
		if (flag != showing) {
			var len = sprites.Length;
			for(int i=0; i<len; ++i) {
				sprites[i].enabled = flag;
			}
			showing = flag;
			if (!showing) {
				Reset();
			}
		}
	}
	
	public void Reset() {
		root.localPosition = Vector3.zero;
		leftArm.localRotation = Quaternion.identity;
		rightArm.localRotation = Quaternion.identity;
		leftLeg.localRotation = Quaternion.identity;
		rightLeg.localRotation = Quaternion.identity;
	}
	
	//--------------------------------------------------------------------------------
	// ROTATE INDIVIDUAL LIMBS
	// ARMS ON RANGE -1:0:1
	// LEGS ON RANGE 0:1
	//--------------------------------------------------------------------------------
	
	public void SetLeftArm(float u) { leftArm.localRotation = QDegrees(-80f * u); }
	public void SetRightArm(float u) { rightArm.localRotation = QDegrees(50f * u); }
	public void SetLeftLeg(float u) { leftLeg.localRotation = QDegrees(60f * u); }
	public void SetRightLeg(float u) { rightLeg.localRotation = QDegrees(-60f * u); }
	
	//--------------------------------------------------------------------------------
	// ANIMATION CYCLES
	//--------------------------------------------------------------------------------
	
	const float kJumpHeight = 0.14f;
	
	public void ApplyRunCycle(float time) {
		var u = Mathf.Abs(Mathf.Sin (2f * TAU * time));
		
		root.localPosition = Vec(0, kJumpHeight * u, 0);
		SetLeftArm(1f-u);
		SetRightArm(1f-u);
		SetLeftLeg(1f-u);
		SetRightLeg(1f-u);
	}
	
	public void ApplyJumpCycle(float time) {
		var u = 0.5f + 0.5f * Mathf.Sin (4f * TAU * time);
		u = 0.8f + 0.2f * u;
		root.localPosition = Vec(0, kJumpHeight, 0);
		SetLeftArm(1f-u);
		SetRightArm(1f-u);
		SetLeftLeg(1f-u);
		SetRightLeg(1f-u);		
	}
	
	public void TickRagdoll() {
		leftArm.localRotation = leftArm.localRotation * QDegrees(40f * Time.deltaTime);
		rightArm.localRotation = leftArm.localRotation * QDegrees(50f * Time.deltaTime);
		leftLeg.localRotation = leftArm.localRotation * QDegrees(55f * Time.deltaTime);
		rightLeg.localRotation = leftArm.localRotation * QDegrees(45f * Time.deltaTime);	
	}
	
}
