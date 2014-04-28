// CONFIDENTIAL Copyright 2013 (C) Little Polygon LLC, All Rights Reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Diagnostics = System.Diagnostics;

// Base class to use instead of MonoBehaviour.  No additional cost (all methods are static), but
// hella convenient, bro.
public class CustomBehaviour : MonoBehaviour {
	
	// TAU MANIFESTO!
	public static float TAU = Mathf.PI + Mathf.PI;
	
	// Shader-Style Vector Shorthand
	public static Vector2 Vec(float x, float y) { return new Vector2(x, y); }
	public static Vector3 Vec(float x, float y, float z) { return new Vector3(x, y, z); }
	public static Vector3 Vec(float x, Vector2 yz) { return new Vector3(x, yz.x, yz.y); }
	public static Vector3 Vec(Vector2 v, float z) { return new Vector3(v.x, v.y, z); }
	public static Vector4 Vec(float x, float y, float z, float w) { return new Vector4(x, y, z, w); }
	public static Vector4 Vec(Vector3 v, float w) { return new Vector4(v.x, v.y, v.z, w); }
		
	public static Vector2 Cmul(Vector2 u, Vector2 v) {
		return new Vector2(u.x*v.x-u.y*v.y, u.x*v.y + u.y*v.x);
	}

	public static Vector2 PolarVec(float r, float theta) {
		return r * (new Vector2(Mathf.Cos(theta), Mathf.Sin(theta)));
	}

	public static Vector2 UnitVec(float theta) {
		return new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
	}

	// Easy way to type rectangles
	public static Rect Rectangle(float x, float y, float w, float h) { return new Rect(x, y, w, h); }
	public static Rect Rectangle(Vector2 p, Vector2 s) { return new Rect(p.x, p.y, s.x, s.y); }
	
	public static Quaternion QDegrees(float degrees) {
		return Quaternion.AngleAxis(degrees, Vector3.forward);
	}
	
	public static Quaternion QRadians(float radians) {
		return Quaternion.AngleAxis(radians * Mathf.Rad2Deg, Vector3.forward);
	}
	
	public static Quaternion QDir(Vector2 dir) {
		return Quaternion.AngleAxis(dir.Degrees(), Vector3.forward);
	}
	
	// Runtime assertions which only run in the editor
	[Diagnostics.Conditional ("UNITY_ENGINE")]
	public static void Assert(bool cond) { 
		if (!cond) {
			Debug.LogError("Assertion Failed, Dawg");
			Application.Quit();
		}
	}
	
	// Some easing functions
	public static float Parabola(float x) { return 1f - (x=1f-x-x)*(x); }
	public static float ParabolaDeriv(float x) { return 4f*(1f-x-x); }
	public static float EaseOut2(float u) { return 1f-(u=1f-u)*u; }
	public static float EaseOut4(float u) { return 1f-(u=1f-u)*u*u*u; }
	
	
	public static float EaseInOutBack(float t) {
		var v = t + t;
		var s = 1.70158f * 1.525f;
		if (v < 1.0f) {
			return 0.5f * (v * v * ((s + 1.0f) * v - s));
		} else {
			v -= 2.0f;
			return 0.5f * (v * v * ((s + 1.0f) * v + s) + 2.0f);
		}
	}
	
	public static float EaseOutBack(float t) { t-=1.0f; return t*t*((1.70158f+1.0f)*t + 1.70158f) + 1.0f; }
	
	public static float Expovariate(float avgDuration, float uMin, float uMax) { 
		return -avgDuration * Mathf.Log(1.0f - UnityEngine.Random.Range(uMin, uMax)); 
	}
	
	// Generic versions of Unity calls that take a type argument
	public static T Dup<T> (T obj) where T : UnityEngine.Object { return Instantiate(obj) as T; }
	public static T Dup<T> (T obj, Vector3 pos) where T : UnityEngine.Object { return Instantiate(obj, pos, Quaternion.identity) as T; }
	public static T Dup<T> (T obj, Vector3 pos, Quaternion q) where T : UnityEngine.Object { return Instantiate(obj, pos, q) as T; }
	
	public static T LoadResource<T> (string name) where T : UnityEngine.Object { return Resources.Load(name, typeof(T)) as T; }
	public static T CreateInstance<T> (string name) where T : UnityEngine.Object { return Dup<T>(LoadResource<T>(name)); }
	
	public static T FindObject<T> () where T : UnityEngine.Object { return GameObject.FindObjectOfType(typeof(T)) as T; }
	public static T[] FindObjects<T>() where T : UnityEngine.Object { return (T[]) GameObject.FindObjectsOfType(typeof(T)); }
		
	// Prolly in the std lib, but whatever
	public static void Swap<T>(ref T u, ref T v) {
		var tmp = u;
		u = v;
		v = tmp;
	}
	
	// Easy color literals
	public static Color32 RGB(uint hex) {
		return new Color32(
			(byte)((0xff0000 & hex) >> 16),
			(byte)((0x00ff00 & hex) >>  8),
			(byte)((0x0000ff & hex)      ),
			(byte)255
		);		
	}
	
	public static Color32 RGBA(uint hex) {
		return new Color32(
			(byte)((0xff000000 & hex) >> 24),
			(byte)((0x00ff0000 & hex) >> 16),
			(byte)((0x0000ff00 & hex) >>  8),
			(byte)((0x000000ff & hex)      )
		);
	}
	
	public static Color RGB(float r, float g, float b) { return new Color(r, g, b); }
	public static Color RGBA(float r, float g, float b, float a) { return new Color(r, g, b, a); }
	public static Color RGBA(Color c, float a) { return new Color(c.r, c.g, c.b, a); }
	
	public static IEnumerable<float> Interpolate(float duration) {
		for(var t=0f; t<duration; t+=Time.deltaTime) {
			yield return t / duration;
		}
		yield return 1f;
	}
	
	public static Vector2 Abs(Vector2 v)  { return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y)); }
	public static Vector3 Abs(Vector3 v)  { return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z)); }
	public static Vector2 Min(Vector2 u, Vector2 v)  { return new Vector2(Mathf.Min(u.x, v.x), Mathf.Min(u.y, v.y)); }
	public static Vector3 Min(Vector3 u, Vector3 v)  { return new Vector3(Mathf.Min(u.x, v.x), Mathf.Min(u.y, v.y), Mathf.Min(u.z, v.z)); }
	public static Vector2 Max(Vector2 u, Vector2 v)  { return new Vector2(Mathf.Max(u.x, v.x), Mathf.Max(u.y, v.y)); }
	public static Vector3 Max(Vector3 u, Vector3 v)  { return new Vector3(Mathf.Max(u.x, v.x), Mathf.Max(u.y, v.y), Mathf.Max(u.z, v.z)); }
	
	
	public static void DrawArcGizmo(float radius, float a0, float a1) {
		var da = a1 - a0;
		var curr = PolarVec(radius, a0);
		var rotor = PolarVec(1f, da/49f);
		for(int i=0; i<50; ++i) {
			var next = Cmul(curr, rotor);
			if (i % 2 == 0) {
				Gizmos.DrawLine(curr, next);
			}
			curr = next;
		}	
	}
}


