using UnityEngine;
using System.Collections;

public class Worm : Tile {
	public Sprite sadFace;
	internal Sprite normalFace;
	
	protected override void Awake() {
		base.Awake ();
		normalFace = spr.sprite;
	}
	
	
	
	
}
