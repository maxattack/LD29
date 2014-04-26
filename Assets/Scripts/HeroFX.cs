using UnityEngine;
using System.Collections;

public class HeroFX : CustomBehaviour {

	public enum Direction { Left, Right }
	
	public SpriteRenderer fx;
	public Sprite jumpySprite;
	internal Sprite idleSprite;
	internal Transform xform;
	
	void Awake() {
		xform = fx.transform;
		idleSprite = fx.sprite;
		//jumpySprite = 
	}
	
	public void SetDirection(Direction dir) {
		xform.localScale = Vec(dir == Direction.Left ? -1 : 1, 1, 1);
	}

}
