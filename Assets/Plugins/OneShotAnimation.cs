using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class OneShotAnimation : PooledObject {
	public Sprite[] frames;
	public float framesPerSecond = 30f;
	public string sfx;
	
	SpriteRenderer spriteRenderer;
	float time;
	int frame;
	
	void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	public override void Init() {
		spriteRenderer.sprite = frames[0];
		frame = 0;
		time = 0f;
		if (sfx.Length > 0) {
			Jukebox.Play(sfx);
		}
	}
	
	void Update() {
		time += Time.deltaTime;
		var nextFrame = Mathf.FloorToInt(time * framesPerSecond);
		if (nextFrame >= frames.Length) {
			SetFrame(frames.Length-1);
			Release();
		} else {
			SetFrame(nextFrame);
		}
	}
	
	void SetFrame(int aFrame) {
		if (aFrame != frame) {
			frame = aFrame;
			spriteRenderer.sprite = frames[frame];
		}
	}
	
	
}
