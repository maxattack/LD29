using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraFX : CustomBehaviour {
	
	public float lookAheadAmount = 1f;
	public float killSpeed = 1f;
	public float bottomOffsetLimit = 0f;
	public float shakeIntensity = 0.1f;
	
	internal static CameraFX inst;
	internal Camera cam;
	internal Transform xform;
	internal Rigidbody body;
	internal Color baseColor;
	internal float cameraColorEasing = 0.2f;
	
	float smoothedSpeed = 0f;
	Vector3 shake = Vec(0,0,0);
	int haltingSemaphore = 0;
	
	Vector3 p0; // restposition
	
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
		body = this.rigidbody;
		cam = GetComponent<Camera>();
		baseColor = cam.backgroundColor;
		
		p0 = xform.position;
	}
	
	void OnDestroy() {
		// RELEASE SINGLETON REF
		if (inst == this) { inst = null; }
	}
	
	void LateUpdate() {
		
		if (Hero.inst) {

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
			
		}
		
		// CAMERA SHAKE
		if (shake.sqrMagnitude > 0.0001f) {
			shake = shake.EaseTowards(Vector3.zero, 0.5f);
		}
		
		// body position instead?
		xform.position = p0 + shakeIntensity * shake;
		
		// TONE DOWN FLASH
		cam.backgroundColor = cam.backgroundColor.EaseTowards(baseColor, cameraColorEasing);
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
	
	public void Flash(Color c, float easing = 0.4f) {
		var c0 = cam.backgroundColor;
		c0.r = Mathf.Max(c0.r, c.r);
		c0.g = Mathf.Max(c0.g, c.g);
		c0.b = Mathf.Max(c0.b, c.b);
		c0.a = Mathf.Max(c0.a, c.a);
		cameraColorEasing = easing;
		cam.backgroundColor = c0;
	}
	
	//--------------------------------------------------------------------------------
	// SHAKE
	//--------------------------------------------------------------------------------
	
	public void Shake(float amount=1f) {
//		switch(Mathf.FloorToInt(4 * Random.value)) {
//		case 0: shake = Vec(amount,0,0); break;
//		case 1: shake = Vec(-amount,0,0); break;
//		case 2: shake = Vec(0,amount,0);break;
//		default: shake = Vec(0,-amount,0);break;
//		}
		shake = Vec(0,-amount,0);
	}
	
	
}
