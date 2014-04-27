using UnityEngine;
using System.Collections;

public class IntroBumper : CustomBehaviour {
	
	IEnumerator Start() {
		
		// CACHE REFERENCES
		var xform = transform;
		var appearFx = xform.Find("AppearFX");
		var logo = xform.Find("Logo");
		var logoFx = logo.GetComponent<SpriteRenderer>();
		
		// HIDE THE PLAYER AND APPEAR FX
		Hero.inst.input.Halt();
		CameraFX.inst.Halt();
		Hero.inst.gameObject.SetActive(false);
		var baseScale = appearFx.localScale;
		appearFx.localScale = Vec(baseScale.x, 0, baseScale.z);
		
		// SHOW LOGO
		var p0 = logo.position;
		var p1 = p0.Above( CameraFX.inst.Height );
		logo.position = p1;
		yield return null;
		foreach(var u in Interpolate(0.5f)) {
			logo.position = Vector3.Lerp(p1, p0, EaseOut2(u));
			yield return null;
		}
		foreach(var u in Interpolate(0.1f)) {
			logoFx.color = RGBA(Color.white, u);
			yield return null;
		}
		Jukebox.Play("Bumper");
		foreach(var u in Interpolate(0.1f)) {
			logoFx.color = RGBA(Color.white, 1f-u);
			yield return null;
		}
		
		
		// WAIT FOR ANY KEY TO START
		do { yield return null; } while(!Input.anyKeyDown);
	
		// TRANSITION IN "FRONT"
		foreach(var u in Interpolate(0.1f)) {
			appearFx.localScale = Vec(baseScale.x, u * baseScale.y, baseScale.z);
			logo.position = Vector3.Lerp(p0, p1, EaseOut2(u));			
			yield return null;
		}
		
		// SHOW THE PLAYER
		Hero.inst.gameObject.SetActive(true);
		Hero.inst.fx.SetColor(Color.white);
		
		CameraFX.inst.Flash(Color.white, 0.1f);
		CameraFX.inst.Shake();
		Jukebox.Play("AppearThump");
		yield return new WaitForSeconds(0.1f);
		
		// TRANSITION IN "TAIL"
		appearFx.position = appearFx.TransformPoint(Vec(0, 1, 0));
		appearFx.rotation = appearFx.rotation * QRadians(Mathf.PI);
		
		
		foreach(var u in Interpolate(0.1f)) {
			var v = u - 1f;
			appearFx.localScale = Vec(baseScale.x, v * baseScale.y, baseScale.z);
			yield return null;
		}
		Hero.inst.fx.Flash(Color.white, 0.25f);
		
				
		// BEGIN INTERACTION
		Hero.inst.input.Unhalt();
		CameraFX.inst.Unhalt();
		
		Destroy(gameObject);
	
	}
}
