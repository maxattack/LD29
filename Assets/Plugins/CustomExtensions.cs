// CONFIDENTIAL Copyright 2013 (C) Little Polygon LLC, All Rights Reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Diagnostics = System.Diagnostics;
using CB = CustomBehaviour;

// Convenience extension methods for common unity objects
public static class CustomExtensions {

	// Shader-style swizzling
	public static Vector2 yx(this Vector2 v) { return new Vector2(v.y, v.x); }
	public static Vector2 xy(this Vector3 v) { return new Vector3(v.x, v.y); }
	public static Vector2 yx(this Vector3 v) { return new Vector3(v.y, v.x); }
	public static Vector2 xz(this Vector3 v) { return new Vector3(v.x, v.z); }
	public static Vector2 yz(this Vector3 v) { return new Vector3(v.y, v.z); }
	public static Vector2 zx(this Vector3 v) { return new Vector3(v.z, v.x); }
	public static Vector2 zy(this Vector3 v) { return new Vector3(v.z, v.y); }
	public static Vector3 x0z(this Vector3 v) { return new Vector3(v.x, 0, v.z); }

	public static Vector2 Clockwise(this Vector2 v) { return new Vector2(-v.y, v.x); }
	public static Vector2 Anticlockwise(this Vector2 v) { return new Vector2(v.y, -v.x); }

	public static Vector3 RotXZ(this Vector3 v) { return new Vector3(-v.z, v.y, v.x); }
	
	public static Vector3 Above(this Vector3 v, float dh) { return new Vector3(v.x, v.y+dh, v.z); }
	public static Vector3 Below(this Vector3 v, float dh) { return new Vector3(v.x, v.y-dh, v.z); }
	public static Vector3 Ahead(this Vector3 v, float dz) { return new Vector3(v.x, v.y, v.z+dz); }
	public static Vector3 Behind(this Vector3 v, float dz) { return new Vector3(v.x, v.y, v.z-dz); }

	public static Vector2 Conjugate(this Vector2 v) {
		return new Vector2(v.x, -v.y);
	}
	
	public static float Radians(this Vector2 v) {
		return Mathf.Atan2(v.y, v.x);
	}
	
	public static float Degrees(this Vector2 v) {
		return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
	}
	
	// Random shuffle
	public static void Shuffle<T>(this IList<T> list) {  
		var rng = new System.Random();  
		var n = list.Count;  
		while (n > 1) {  
			n--;  
			var k = rng.Next(n + 1);  
			var value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}
	
	// Treating lists as stacks
	public static T PopLast<T>(this IList<T> list) {
		var result = list[list.Count-1];
		list.RemoveAt(list.Count-1);
		return result;
	}

	// Time-independent Easing
	
	
	static float TimeIndependentRate(float easing) {
		var dt = Time.deltaTime;
		return dt > Mathf.Epsilon ? Mathf.Pow(easing, (1f/(60f * dt))) : 0f;
	}
	
	public static float EaseTowards(this float curr, float target, float easing) {
		return Mathf.Lerp(curr, target, TimeIndependentRate(easing));
	}
	
	public static Quaternion EaseTowards(this Quaternion curr, Quaternion target, float easing) {
		return Quaternion.Slerp(curr, target, TimeIndependentRate(easing));
	}
	
	public static Vector2 EaseTowards(this Vector2 curr, Vector2 target, float easing) {
		return Vector2.Lerp(curr, target, TimeIndependentRate(easing));
	}
	
	public static Vector3 EaseTowards(this Vector3 curr, Vector3 target, float easing) {
		return Vector3.Lerp(curr, target, TimeIndependentRate(easing));
	}

	public static Vector3 EaseTowards(this Vector3 curr, Vector3 target, float easing, float dt) {
		return Vector3.Lerp(curr, target, TimeIndependentRate(easing));
	}

	public static Vector4 EaseTowards(this Vector4 curr, Vector4 target, float easing) {
		return Vector4.Lerp(curr, target, TimeIndependentRate(easing));
	}
	
	public static Color EaseTowards(this Color curr, Color target, float easing) {
		return Color.Lerp(curr, target, TimeIndependentRate(easing));
	}
	
	// Like the UI command
	public static void Reset(this Transform t) {
		t.localPosition = Vector3.zero;
		t.localScale = Vector3.one;
		t.localRotation = Quaternion.identity;
	}
	
	public static T GetComponentUpwards<T>(this MonoBehaviour b) where T : UnityEngine.Component {
		var t = b.transform.parent;
		while(t != null) {
			var result = t.GetComponent<T>();
			if (result == null) {
				t = t.parent;
			} else {
				return result;
			}
		}
		return null;
	}
	
	
}




