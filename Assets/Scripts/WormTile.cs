using UnityEngine;
using System.Collections;

public class WormTile : Tile {
	
	public Sprite normalFace;
	public Sprite sadFace;
	public Sprite bend;
	public Sprite midsection;
	public Sprite tail;
	public PooledObject explosionPrefab;
	
	internal WormTile wPrev, wNext;
	
	public bool IsHead { get { return wPrev == null; } }
	public bool IsTail { get { return wNext == null; } }
	
	public override void Init ()
	{
		base.Init ();
		baseColor = RGBA(0,0,0,0);
	}
	
	public void InitFX() {
		
		// pick sprite and rotation based on prev/next
		if (IsHead) {
			spr.sprite = normalFace;
			int nDX = wNext.tileX - tileX;
			int nDY = wNext.tileY - tileY;
			transform.rotation = QDir(Vec(nDX, nDY));
			
		} else if (IsTail) {
			spr.sprite = tail;
			int pDX = tileX - wPrev.tileX;
			int pDY = tileY - wPrev.tileY;
			transform.rotation = QDir(Vec(pDX, pDY));
			
		} else {
			int pDX = tileX - wPrev.tileX;
			int pDY = tileY - wPrev.tileY;
			int nDX = wNext.tileX - tileX;
			int nDY = wNext.tileY - tileY;
			
			var isStraight = (pDX == 0 && nDX == 0) || (pDY == 0 && nDY == 0);
			
			if (isStraight) {
				spr.sprite = midsection;
				if (pDY == 0) {
					transform.rotation = Quaternion.identity;
				} else {
					transform.rotation = QDegrees(90f);
				}
				
			} else {
				spr.sprite = bend;
				if (pDX == 1) {
					transform.rotation = Quaternion.identity;
				} else if (pDX == -1) {
					transform.rotation = QDegrees(180f);
				} else if (pDY == 1) {
					transform.rotation = QDegrees(90f);
				} else {
					transform.rotation = QDegrees(-90f);
				}
				var pAngle = Vec(pDX, pDY).Degrees();
				var nAngle = Vec(nDX, nDY).Degrees();
				if (Mathf.DeltaAngle(pAngle, nAngle) > 0) {
					transform.localScale = Vec(1, -1, 1);
				}
			}
		
		}
	
	}
	
	bool waiting = false;
	void WaitAndDestroy() {
		if (gameObject.activeSelf && !waiting) {
			waiting = true;
			StartCoroutine(DoWaitAndDestroy());
		}
	}
	
	IEnumerator DoWaitAndDestroy() {
		yield return new WaitForSeconds(0.1f);
		TakeDamage(999);
	}
	
	public override void Deinit () {
		StopAllCoroutines();
		waiting = false;
	}
	
	protected override void WillDestroy () {
		if (explosionPrefab != null) { 
			var e = explosionPrefab.Alloc(transform.position); 
			e.transform.localScale = Vec(1.25f, 1.25f, 1.25f);
		}
		if (wNext) { wNext.WaitAndDestroy(); }
		if (wPrev) { wPrev.WaitAndDestroy(); }
	}
}
