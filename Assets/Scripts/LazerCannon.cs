using UnityEngine;
using System.Collections;

public class LazerCannon : Item {
	
	public LazerBlast blastPrefab;
	public Transform muzzle;
	SpriteRenderer spr;
	
	void Awake() {
		spr = GetComponentInChildren<SpriteRenderer>();
	}
	
	public override void Operate (Vector2 dir) {
		blastPrefab.AllocBlast(muzzle.position, dir);
		
		Hero.inst.Kickback(-8f * dir);
	}
	
	public override void Init () {
		spr.color = RGB(0.6f, 0.6f, 0.6f);
	}
	
	public override void OnPickUp () {
		spr.color = Color.white;
	}
	
	public override void OnDrop () {
		spr.color = RGB(0.6f, 0.6f, 0.6f);
	}
	
	void LateUpdate() {
	}
}
