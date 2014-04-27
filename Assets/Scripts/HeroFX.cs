using UnityEngine;
using System.Collections;

public class HeroFX : CustomBehaviour {

	public enum Direction { Left, Right }
	public enum Status { Idle, Running, Jumping, Ragdoll, Corpse }

	public bool TerminalState { get { return status == Status.Ragdoll || status == Status.Corpse; } }
	
	public float runAnimScale = 1f;
	
	public Sprite corpse;
	
	internal HeroPose pose;
	internal SpriteRenderer idleSprite;
	internal Transform xform;
	internal float animationTime;
	
	internal Direction direction = Direction.Right;
	internal Status status = Status.Idle;
	
	//--------------------------------------------------------------------------------
	// INIT
	//--------------------------------------------------------------------------------
	
	void Awake() {
		// CACHE REFERENCES
		xform = transform;
		idleSprite = xform.Find("idlePose").GetComponent<SpriteRenderer>();
		pose = GetComponentInChildren<HeroPose>();
	}
		
	void Start() {
		// START IDLE
		pose.Show (false);
	}
	
	//--------------------------------------------------------------------------------
	// MAJOR STATES
	//--------------------------------------------------------------------------------
	
	public void SetDirection(Direction dir) {
	
		// SET THE CURRENT DIRECTION BY FLIPPING THE ROOT AND UNFLIPPING
		// LOCATORS WHERE ITEMS ARE ATTACHED
		if (direction != dir) {
			direction = dir;

			xform.localScale = Vec(dir == Direction.Left ? -1 : 1, 1, 1);
			pose.rightHand.localScale = Vec(dir == Direction.Left ? -1 : 1, 1, 1);
			pose.leftHand.localScale = Vec(dir == Direction.Left ? -1 : 1, 1, 1);
		}
	}
	
	public bool SetStatus(Status aStatus) {
		
		if (TerminalState) { return false; }
		
		// FLIP BETWEEN THE IDLE SPRITE AND THE ARTICULATED POSE
		if (status != aStatus) {
			if (aStatus == Status.Corpse) {
				status = aStatus;
				pose.Show (false);
				idleSprite.enabled = true;
				idleSprite.sprite = corpse;
			} else {
			
				if (status == Status.Idle) {
					idleSprite.enabled = false;
					pose.Show(true);
				}
				status = aStatus;
				if (status == Status.Idle) {
					idleSprite.enabled = true;
					pose.Show(false);
				}
			}
			return true;
		} else {
			return false;
		}
		
	}
	
	//--------------------------------------------------------------------------------
	// ANIMATION EFFECTS
	//--------------------------------------------------------------------------------
	
	void Update() {
	
		if (!TerminalState) {
			
			// DO NORMAL ANIMATIONS
			var oldStatus = status;
			if (Hero.inst.grounded) {
				var speed = Mathf.Abs (Hero.inst.body.velocity.x);
				var prevTime = animationTime;
				if (speed > 0.05f) {
					SetStatus(Status.Running);
					animationTime += runAnimScale * (speed+0.25f) * Time.deltaTime;
					pose.ApplyRunCycle(animationTime);
				} else {
					SetStatus(Status.Idle);
					animationTime = 0f;
				}
				var a = Mathf.FloorToInt(8f * prevTime) % 2;
				var b = Mathf.FloorToInt(8f * animationTime) % 2;
				if (a != b && b == 0) { Jukebox.Play("Footfall"); }
				
			} else {
				animationTime = 0f;
				SetStatus(Status.Jumping);
				pose.ApplyJumpCycle(Time.time);
			}
			if (status != oldStatus && oldStatus == Status.Jumping) {
				Jukebox.Play("Footfall");
			}
			
		} else if (status == Status.Ragdoll) {
			// DO RAGDOLL ANIMATIONS
			pose.TickRagdoll();
		}
	}
	
	//--------------------------------------------------------------------------------
	// COLOR TINTING EFFECTS
	//--------------------------------------------------------------------------------
	
	public void Flash(Color c, float duration = 1f) {
		StartCoroutine(DoFlash(c, duration));
	}
	
	IEnumerator DoFlash(Color c, float duration) {
		foreach(var u in Interpolate(duration)) {
			SetColor(RGBA(c, 1f-EaseOut2(u)));
			yield return null;
		}
	}
	
	public void SetColor(Color c) {
		idleSprite.color = c;
		for(int i=0; i<pose.sprites.Length; ++i) {
			pose.sprites[i].color = c;
		}
	}

	

}
