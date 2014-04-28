using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraFX : CustomBehaviour {
	
	public float lookAheadAmount = 1f;

	public float bottomOffsetLimit = 0f;
	public float shakeIntensity = 0.1f;
	
	internal static CameraFX inst;
	internal Camera cam;
	internal Transform xform;
	internal Rigidbody body;
	internal Color baseColor;
	internal float cameraColorEasing = 0.2f;
	
	Vector3 smoothedSpeed = Vector3.zero;
	Vector3 shake = Vec(0,0,0);

	
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
		
		// ADJUST KILL COLLIDER
		var box = GetComponent<BoxCollider>();
		if (box) {
			var c = box.center;
			box.center = Vec(c.x, HalfHeight + 0.5f * box.size.y, c.z);
		}
	}
	
	void OnDestroy() {
		// RELEASE SINGLETON REF
		if (inst == this) { inst = null; }
	}
	
	void LateUpdate() {
		
		if (Hero.inst) {

			// TRACK HERO WITH A LITTLE LOOK-AHEAD
			Vector3 dir = new Vector3( Hero.inst.currDir.x,Hero.inst.currDir.y,0);
			smoothedSpeed = smoothedSpeed.EaseTowards(dir, 0.1f);

			var pos = Hero.inst.xform.position;


			var targetPosition = pos + new Vector3(lookAheadAmount * 0.5f * smoothedSpeed.x,
			                                       lookAheadAmount  * smoothedSpeed.y,
			                                       0);

			if(Hero.inst.xform.position.y < -WorldGen.inst.height)
			{
				GetComponent<Camera>().orthographicSize = 12;
				targetPosition = (WorldGen.inst.earthCore.xform.position + Hero.inst.xform.position) * 0.5f;
			}


			p0.x = Mathf.Clamp(
				p0.x.EaseTowards(targetPosition.x, 0.2f), 
				LimitLeft, 
				LimitRight
			);
			p0.y = Mathf.Clamp( p0.y.EaseTowards(targetPosition.y,0.2f),-10000,KillBar.inst.xform.position.y - HalfHeight);

			
			// CAMERA TRACKS PLAYER BELOW SCREEN
			//var bottomThreshold = p0.y - HalfHeight + bottomOffsetLimit;
			//var diff = bottomThreshold - pos.y;
			//if (diff > 0f) {
		//		p0.y -= diff;
		//	}			
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
