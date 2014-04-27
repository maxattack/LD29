using UnityEngine;
using System.Collections;

public class KillScreen : CustomBehaviour {
	
	IEnumerator Start() {
		var xform = transform;
		xform.parent = CameraFX.inst.xform;
		var p0 = Vec(0, CameraFX.inst.Height, 1f);
		var p1 = Vec(0, 1f, 1f);
		xform.localPosition = p0;
		var duration = 0.5f;
		for(var t=0f; t<0.5f; t+=RealTime.DeltaTime) {
			yield return null;
		}
		for(var t=0f; t<duration; t+=RealTime.DeltaTime) {
			xform.localPosition = Vector3.Lerp(p0, p1, EaseOut2(t/duration));
			yield return null;
		}
		
		while(!Input.anyKey) { yield return null; }
		Application.LoadLevel(Application.loadedLevel);
		Time.timeScale = 1f;
	}
	
}
