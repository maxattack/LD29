using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Hero))]
public class HeroFX : CustomBehaviour {

	public enum Direction { Left, Right }
	
	public SpriteRenderer fx;
	public Sprite jumpySprite;
	internal Sprite idleSprite;
	internal Transform xform;
	internal Hero hero;
	
	internal float animationTime;
	internal Direction direction = Direction.Right;
	
	void Awake() {
		xform = fx.transform;
		idleSprite = fx.sprite;
		hero = GetComponent<Hero>();
	}
	
	public void SetDirection(Direction dir) {
		if (direction != dir) {
			direction = dir;
			xform.localScale = Vec(dir == Direction.Left ? -1 : 1, 1, 1);
		}
	}
	
	void Update() {
	
		if (hero.grounded) {
			var speed = Mathf.Abs (hero.body.velocity.x);
			if (speed > 0.02f) {
				animationTime += speed * Time.deltaTime;
				var spr =  Mathf.FloorToInt(2 * animationTime) % 2 == 0 ? idleSprite : jumpySprite;
				if (fx.sprite != spr) {
					fx.sprite = spr;
					if (spr == idleSprite) {
						Jukebox.Play("Footfall");
					}
				}
				
			} else {
				animationTime = 0f;
				if (fx.sprite != idleSprite) {
					fx.sprite = idleSprite;
					Jukebox.Play("Footfall");
				}
			}
		} else {
			animationTime = 0f;
			fx.sprite = jumpySprite;			
		}
	}
	
	public void Flash(Color c, float duration = 1f) {
		StartCoroutine(DoFlash(c, duration));
	}
	
	IEnumerator DoFlash(Color c, float duration) {
		foreach(var u in Interpolate(duration)) {
			fx.color = RGBA(c, 1f-EaseOut2(u));
			yield return null;
		}
	}
	

	

}
