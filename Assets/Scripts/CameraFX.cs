using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraFX : CustomBehaviour {
	
	public float lookAheadAmount = 1f;
	public float killSpeed = 1f;
	
	internal static CameraFX inst;
	internal Camera cam;
	internal Transform xform;
	internal Color baseColor;
	
	float smoothedSpeed = 0f;
	
	//--------------------------------------------------------------------------------
	// GETTERS
	//--------------------------------------------------------------------------------

	public float Width { get { return 2f * cam.orthographicSize * cam.aspect; } }
	public float Height { get { return 2f * cam.orthographicSize; } }
	
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
		
		// TRACK HERO WITH A LITTLE LOOK-AHEAD
		if (Hero.inst) {
			var p0 = xform.position;
			smoothedSpeed = smoothedSpeed.EaseTowards(Hero.inst.body.velocity.x, 0.1f);
			var targetPosition = Hero.inst.xform.position.x + lookAheadAmount * smoothedSpeed;
			p0.x = p0.x.EaseTowards(targetPosition, 0.2f);
			p0.y -= Time.deltaTime * killSpeed;
			xform.position = p0;
		}
	
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
