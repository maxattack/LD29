using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraFX : CustomBehaviour {
	
	public float lookAheadAmount = 1f;
	public float killSpeed = 1f;
	public float bottomOffsetLimit = 0f;
	
	internal static CameraFX inst;
	internal Camera cam;
	internal Transform xform;
	internal Color baseColor;
	
	float smoothedSpeed = 0f;
	int haltingSemaphore = 0;
	
	//--------------------------------------------------------------------------------
	// GETTERS
	//--------------------------------------------------------------------------------

	public float HalfWidth { get { return cam.orthographicSize * cam.aspect; } }
	public float HalfHeight { get { return cam.orthographicSize; } }
	public float Width { get { return 2f * cam.orthographicSize * cam.aspect; } }
	public float Height { get { return 2f * cam.orthographicSize; } }
	
	public float LimitLeft {
		get {
			var worldLeft = -0.5f;
			return worldLeft + HalfWidth;
		}
	}
	
	public float LimitRight {
		get {
			var worldRight = WorldGen.inst.width - 0.5f;
			return worldRight - HalfWidth;
		}
	}
	
	//--------------------------------------------------------------------------------
	// EVENTS
	//--------------------------------------------------------------------------------

	void Awake() {
		// EAGERLY ACQUIRE SINGLETON REF
		inst = this;
		
		// CACHE BASE CAMERA PARAMS
		xform = this.transform;
		cam = GetComponent<Camera>();
		baseColor = cam.backgroundColor;
		
	}
	
	void OnDestroy() {
		// RELEASE SINGLETON REF
		if (inst == this) { inst = null; }
	}
	
	void LateUpdate() {
		
		if (Hero.inst) {
			var p0 = xform.position;

			// TRACK HERO WITH A LITTLE LOOK-AHEAD
			smoothedSpeed = smoothedSpeed.EaseTowards(Hero.inst.body.velocity.x, 0.1f);
			var hp = Hero.inst.xform.position;
			var targetPosition = hp.x + lookAheadAmount * smoothedSpeed;
			p0.x = Mathf.Clamp(
				p0.x.EaseTowards(targetPosition, 0.2f), 
				LimitLeft, 
				LimitRight
			);
			if (!Halted) {
				p0.y -= Time.deltaTime * killSpeed;
			}
			
			// CAMERA TRACKS PLAYER BELOW SCREEN
			var bottomThreshold = p0.y - HalfHeight + bottomOffsetLimit;
			var diff = bottomThreshold - hp.y;
			if (diff > 0f) {
				p0.y -= diff;
			}
			
			xform.position = p0;
		}
	
	}

	//--------------------------------------------------------------------------------
	// Y-MOVE HALTING
	//--------------------------------------------------------------------------------
	
	public bool Halted { get { return haltingSemaphore > 0; } }
	
	public void Halt() {
		++haltingSemaphore;
	}
	
	public void Unhalt() {
		if (haltingSemaphore > 0) { --haltingSemaphore; }
	}
	
	//--------------------------------------------------------------------------------
	// COLOR FLASHES
	//--------------------------------------------------------------------------------
	
	public void Flash(Color c, float duration=1f) {
		StartCoroutine(DoFlash(c, duration));
	}
	
	IEnumerator DoFlash(Color c, float duration) {
		foreach(var u in Interpolate(duration)) {
			cam.backgroundColor = Color.Lerp(c, baseColor, EaseOut4(u));
			yield return null;
		}
	}
	
}
