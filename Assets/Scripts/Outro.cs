using UnityEngine;
using System.Collections;

public class Outro : CustomBehaviour {
	
	public Transform heroes;
	public Transform youWin;
	public SpriteRenderer earth1;
	public SpriteRenderer earth2;
	
	public AudioSource riff;
	public AudioSource explo;
	
	IEnumerator Start() {
		Music.KillMusic();

		var hh = Camera.main.orthographicSize;
		//var h = 2f * Camera.main.orthographicSize;
		
		var hp = heroes.position;
		var wp = youWin.position;
		heroes.position = Vec(hp.x, -1.5f * hh, hp.z);
		youWin.position = Vec(wp.x, 1.5f * hh, wp.z);
		
		earth2.enabled = false;
		
		foreach(var u in Interpolate(1f)) {
			earth1.color = RGBA(Color.white, u*u);
			yield return null;
		}
		
		
		
		earth1.enabled = false;
		earth2.enabled = true;
		
		explo.Play();
		StartCoroutine(DoWaitAndPlay());
		StartCoroutine(DoWibble(earth2.transform));
		
		var hp0 = heroes.position;
		var hp1 = youWin.position;
		
		foreach(var u in Interpolate(0.5f)) {
			earth2.color = RGBA(Color.white, 1f-u);
			heroes.position = Vector3.Lerp(hp0, hp, EaseOut2(u));
			youWin.position = Vector3.Lerp(hp1, wp, EaseOut2(Mathf.Clamp01(2f*(u-0.5f))));
			yield return null;
		}
		
		
		
	}
	
	IEnumerator DoWaitAndPlay() {
		yield return new WaitForSeconds(0.375f);
		riff.Play();
	}
	
	IEnumerator DoWibble(Transform xform) {
		var t = 0f;
		var magnitude = 1.5f;
		var p0 = xform.position;
		for(;;) {
			t += 100f * Time.deltaTime;
			magnitude = magnitude.EaseTowards(0f, 0.05f);
			xform.position = p0 + magnitude * Vec(
				3f * Mathf.Sin(t * TAU * 1.1f),
				2.1f * Mathf.Sin(t * TAU * 1.333f),
				0f
			);
			
			yield return null;
		
		}
	}
	
	
}
