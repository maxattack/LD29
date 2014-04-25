using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class OneShotSpriteAnimation : CustomBehaviour {
	public Sprite[] frames;
	public float framesPerSecond = 30f;
	
	SpriteRenderer spriteRenderer;
	float time;
	int frame;
	
	void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = frames[0];
		frame = 0;
	}
	
	void OnEnable() {
		SetFrame(0);
		time = 0f;
	}
	
	void Update() {
		time += Time.deltaTime;
		var nextFrame = Mathf.FloorToInt(time * framesPerSecond);
		if (nextFrame >= frames.Length) {
			SetFrame(frames.Length-1);
			SendMessage("AnimationComplete", SendMessageOptions.DontRequireReceiver);
		} else {
			SetFrame(nextFrame);
		}
	}
	
	void SetFrame(int aFrame) {
		//if (aFrame != frame) {
			frame = aFrame;
			spriteRenderer.sprite = frames[frame];
		//}
	}
	
	
}
