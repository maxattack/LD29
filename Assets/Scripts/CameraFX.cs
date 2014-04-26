using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraFX : CustomBehaviour {
	
	internal static CameraFX inst;
	internal Camera cam;
	internal Color baseColor;
	
	void Awake() {
		// EAGERLY ACQUIRE SINGLETON REF
		inst = this;
		cam = GetComponent<Camera>();
		baseColor = cam.backgroundColor;
	}
	
	void OnDestroy() {
		// RELEASE SINGLETON REF
		if (inst == this) { inst = null; }
	}
	
	public void Flash(Color c, float duration=1f) {
		StartCoroutine(DoFlash(c, duration));
	}
	
	IEnumerator DoFlash(Color c, float duration) {
		foreach(var u in Interpolate(duration)) {
			cam.backgroundColor = Color.Lerp(c, baseColor, EaseOut4(u));
			yield return null;
		}
	}
	
	public float Height { get { return 2f * cam.orthographicSize; } }
	
}
