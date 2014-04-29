using InControl;
using UnityEngine;
using System.Collections;

public class IntroBumper : CustomBehaviour {
	
	public SpriteRenderer dialogSpr;
	public SpriteRenderer[] textSprites;
	public SpriteRenderer genderOp;
	public TextMesh pressAny;
	static bool firstTime = true;
	
	IEnumerator Start() {
		
		
		
		// CACHE REFERENCES
		var xform = transform;
		var appearFx = xform.Find("AppearFX");
		var logo = xform.Find("Logo");
		var logoFx = logo.GetComponent<SpriteRenderer>();
		
		var credits = xform.Find ("Credits");
		
		// HIDE THE PLAYER AND APPEAR FX
		Hero.inst.input.Halt();
		KillBar.inst.Halt();
		Hero.inst.gameObject.SetActive(false);
		var baseScale = appearFx.localScale;
		appearFx.localScale = Vec(baseScale.x, 0, baseScale.z);
		dialogSpr.enabled = false;
		foreach(var spr in textSprites) { spr.enabled = false; }
		genderOp.enabled = false;
		
		if (firstTime) {
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
			do { yield return null; } while(!GameInput.AnyPress);
			
			if (genderOp) {
				pressAny.text = "GIRL OR BOY?";
				genderOp.enabled = true;
				genderOp.color = RGBA(1,1,1,0);
				foreach(var u in Interpolate(0.25f)) {
					genderOp.color = RGBA(1,1,1,EaseOut2(u));
					yield return null;
				}
				bool? girlChoice = null;
				do {
					var pressedLeft = Input.GetKeyDown(KeyCode.LeftArrow) || InputManager.ActiveDevice.DPadLeft.WasPressed;
					var pressedRight = Input.GetKeyDown(KeyCode.RightArrow) || InputManager.ActiveDevice.DPadRight.WasPressed;
					if (pressedLeft || pressedRight) {
						girlChoice = pressedLeft;
					}
					yield return null;
				} while(girlChoice == null);
				Jukebox.Play("Pickup");
				if (girlChoice.Value) {
					Hero.inst.SetGirl(true);
				}
				genderOp.enabled = false;
			
			}
			
			
			// TRANSITION IN "FRONT" / OUT LOGO
			var cp0 = credits.position;
			var cp1 = credits.position.Below(0.5f * CameraFX.inst.HalfHeight);
			
			foreach(var u in Interpolate(0.1f)) {
				appearFx.localScale = Vec(baseScale.x, u * baseScale.y, baseScale.z);
				logo.position = Vector3.Lerp(p0, p1, EaseOut2(u));			
				credits.position = Vector3.Lerp (cp0, cp1, EaseOut2(u));
				yield return null;
			}
			logo.gameObject.SetActive(false);
			credits.gameObject.SetActive(false);
			
		} else {
			logo.gameObject.SetActive(false);
			credits.gameObject.SetActive(false);
		
			// TRANSITION IN "FRONT"
			foreach(var u in Interpolate(0.1f)) {
				appearFx.localScale = Vec(baseScale.x, u * baseScale.y, baseScale.z);
				yield return null;
			}
			
		}
	
		// SHOW THE PLAYER
		Hero.inst.gameObject.SetActive(true);
		Hero.inst.fx.SetColor(Color.white);
		
		CameraFX.inst.Flash(Color.white, 0.1f);
		CameraFX.inst.Shake();
		Jukebox.Play("AppearThump");
		yield return new WaitForSeconds(0.1f);
		
		// TRANSITION IN "TAIL"
		appearFx.localPosition = appearFx.TransformPoint(Vec(0, 1, 0));
		appearFx.rotation = appearFx.rotation * QRadians(Mathf.PI);
		
		
		foreach(var u in Interpolate(0.1f)) {
			var v = u - 1f;
			appearFx.localScale = Vec(baseScale.x, v * baseScale.y, baseScale.z);
			yield return null;
		}
		Hero.inst.fx.Flash(Color.white, 0.25f);
		
		if (firstTime) {
			
			// DIALOG SCENE
			Jukebox.Play("Warning");
			var dRoot = dialogSpr.transform;
			dRoot.parent = CameraFX.inst.xform;
			var p = dRoot.localPosition;
			p.x = 0;
			dialogSpr.enabled = true;
			foreach(var u in Interpolate(0.5f)) {
				dialogSpr.color = RGBA(Color.white, 1f - EaseOut2(u));
				dRoot.localPosition = Vector3.Lerp(p.Below(1f), p, EaseOutBack(u));
				yield return null;
			}
			foreach(var spr in textSprites) {
				spr.enabled = true;
				Jukebox.Play("Beep1");
				foreach(var u in Interpolate(0.25f)) {
					spr.color = RGBA(1,1,1,u);
					yield return null;
				}
				for(var t=0f; t<4f; t+=Time.deltaTime) {
					if (GameInput.AnyPress) { break; }
					yield return null;
				}
				foreach(var u in Interpolate(0.25f)) {
					spr.color = RGBA(1,1,1,1f-u);
					yield return null;					
				}
			}
			Jukebox.Play("Derp");
			foreach(var u in Interpolate(0.1f)) {
				dRoot.localScale = Vec(1, 1-EaseOut2(u), 1);
				//dRoot.localPosition = Vector3.Lerp(p.Below(1f), p, 1f-EaseOutBack(u));
				yield return null;
			}
			
			Destroy(dialogSpr.gameObject);
		}
				
		// BEGIN INTERACTION
		Hero.inst.input.Unhalt();
		KillBar.inst.Unhalt();
		
		firstTime = false;
		
		
		Destroy(gameObject);
	
	}
}
