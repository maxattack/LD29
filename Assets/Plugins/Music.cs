// CONFIDENTIAL Copyright 2013 (C) Little Polygon LLC, All Rights Reserved.

#if UNITY_EDITOR
#define MUTE_MUSIC
#endif

using UnityEngine;
using System.Collections;

// Plays music and ensure only one is going at the same time.  
// Allows same song to span multiple scenes.
[RequireComponent(typeof(AudioSource))]
public class Music : MonoBehaviour {
	public static Music inst = null;
	
	public bool interrupt = true;
	
	public static void KillMusic() {
		if (inst != null) {
			Destroy(inst.gameObject);
			inst = null;
		}
	}
	
	void Awake() {
#if MUTE_MUSIC
		Destroy(gameObject);
#else
		if (inst == null || (interrupt && !inst.SameSong(this))) {
			KillMusic();
			DontDestroyOnLoad(gameObject);
			inst = this;
			GetComponent<AudioSource>().Play();
		} else {
			Destroy(gameObject);
		}
#endif
	}
	
	void OnDestroy() {
		if (inst == this) { inst = null; }
	}
	
	bool SameSong(Music other) {
		return other.GetComponent<AudioSource>().clip == GetComponent<AudioSource>().clip;
	}
	
}
