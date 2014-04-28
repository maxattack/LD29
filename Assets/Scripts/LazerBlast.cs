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
		result.Shoot(dir);
		return result; 
	}
	
	public void Shoot(Vector2 dir) {
		
		// RESET FROM LAST TIME
		rayStartRoot.localScale = Vec(1,1,1);
		rayRoot.localScale = Vec(0,1,1);
		SetAlpha(1);
		for(int i=0; i<zaps.Length; ++i) {
			zaps[i].localPosition = zapStarts[i];
		}
		
		// ROTATE BEAM
		xform.rotation = QDir(dir);
		
		// START ANIMATIONS
		Jukebox.Play("LazerBlast");
		StartCoroutine(ShowEffects());
		CameraFX.inst.Flash(Color.white, 0.1f);
		
		// ACTUALLY SHOOT
		var diag = Vec(CameraFX.inst.Width, CameraFX.inst.Height).magnitude;
		var shootMask = Layers.TileMask | Layers.EnemyMask;
		var hits = Physics.SphereCastAll(xform.position.xy ()-2f*dir, 0.5f, Vec(dir,0), diag, shootMask);
		foreach(var hit in hits) {
			switch(hit.transform.gameObject.layer) {
				case Layers.Enemy:
					// TODO: GENERALIZE TO ANY ENEMY?
					var dino = hit.transform.GetComponent<Dino>();
					if (dino) { dino.Kill(); }
					break;
				case Layers.Tile:
					var tile = hit.transform.GetComponent<Tile>();
					WorldGen.inst.Dig(tile.tileX, tile.tileY - WorldGen.inst.height, 10);
					break;
			}
			
		}
	}
	
	void SetAlpha(float u) {
		for(int i=0; i<sprites.Length; ++i) {
			sprites[i].color = RGBA(1,1,1,u);
		}
	}
	
	IEnumerator ShowEffects() {
		
		// SHOOT
		var diag = Vec(CameraFX.inst.Width, CameraFX.inst.Height).magnitude;
		foreach(var u in Interpolate(0.2f)) {
			rayRoot.localScale = Vec(Mathf.Lerp(0f, diag, u), 1f, 1f);
			yield return null;
		}
		
		// FADE OUT
		foreach(var u in Interpolate (0.1f)) {
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
		// UPDATE ZAPS
		for(int i=0; i<zaps.Length; ++i) {
			var speed = 30f * (1f + 0.8f * i);
			zaps[i].localPosition = zaps[i].localPosition + Vec(speed * Time.deltaTime, 0, 0);
			zaps[i].localScale = Vec(-zaps[i].localScale.x, 1, 1);
		}
	}
	
}
