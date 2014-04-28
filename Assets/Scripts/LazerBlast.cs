using UnityEngine;
using System.Collections;

public class LazerBlast : PooledObject {

	public Transform rayRoot;
	public Transform rayStartRoot;
	public Transform[] zaps;
	public SpriteRenderer[] sprites;
	internal Transform xform;
	internal Vector3[] zapStarts;
	
	void Awake() {
		xform = transform;
		zapStarts = new Vector3[zaps.Length];
		for(int i=0; i<zaps.Length; ++i) {
			zapStarts[i] = zaps[i].localPosition;
		}
	}

	public LazerBlast AllocBlast(Vector3 position, Vector2 dir) {
		var result = Alloc(position) as LazerBlast;
		result.xform.rotation = QDir(dir);
		return result; 
	}
	
	public override void Init () {
		rayStartRoot.localScale = Vec(1,1,1);
		rayRoot.localScale = Vec(0,1,1);
		SetAlpha(1);
		StartCoroutine(ShowEffects());
		CameraFX.inst.Flash(Color.white);
		
		// RESTORE ZAP POSITIONS
		for(int i=0; i<zaps.Length; ++i) {
			zaps[i].localPosition = zapStarts[i];
		}
		
	}
	
	void SetAlpha(float u) {
		for(int i=0; i<sprites.Length; ++i) {
			sprites[i].color = RGBA(1,1,1,u);
		}
	}
	
	IEnumerator ShowEffects() {
		
		// shoot
		var diag = Vec(CameraFX.inst.Width, CameraFX.inst.Height).magnitude;
		foreach(var u in Interpolate(0.2f)) {
			rayRoot.localScale = Vec(Mathf.Lerp(0f, diag, u), 1f, 1f);
			yield return null;
		}
		
		// fade out
		foreach(var u in Interpolate (0.25f)) {
			rayStartRoot.localScale = Vec(1f, 1f-u, 1f);
			rayRoot.localScale = Vec(diag, 1f-u, 1f);
			SetAlpha(1f-EaseOut2(u));
			yield return null;
		}
		
		Release();
	}
	
	public override void Deinit () {
		StopAllCoroutines();
	}
	
	void Update() {
		for(int i=0; i<zaps.Length; ++i) {
			var speed = 24f * (1f + 0.8f * i);
			zaps[i].localPosition = zaps[i].localPosition + Vec(speed * Time.deltaTime, 0, 0);
		}
	}
	
}
