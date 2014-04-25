using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : CustomBehaviour {

	internal static Jukebox inst;
	Dictionary<string, AudioSource> sources = new Dictionary<string, AudioSource>();
	
	void Awake() {
		inst = this;
		foreach(var src in transform.GetComponentsInChildren<AudioSource>()) {
			sources[src.name.ToLower()] = src;
		}		
	}
	
	void OnDestroy() {
		if (inst == this) { inst = null; }
	}
	
	public static void Play(string name) {
		// Play if name exists
		AudioSource src;
		if (inst && inst.sources.TryGetValue(name.ToLower(), out src)) {
			src.Play();
		}
	}
	
	public static void Play(string name, float delay) {
		// Play with delay if name exists
		AudioSource src;
		if (inst && inst.sources.TryGetValue(name.ToLower(), out src)) {
			if (delay < Mathf.Epsilon) {
				src.Play();
			} else {
				src.PlayDelayed(delay);
			}
		}
	}
	
	
	
}
