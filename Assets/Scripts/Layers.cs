using UnityEngine;
using System.Collections;

public static class Layers {
	public const int Default = 0;
	public const int Debris = 8;
	public const int Projectile = 9;
	public const int Hero = 10;
	public const int Item = 11;
	public const int Camera = 12;
	public const int Hazard = 13;
	public const int Enemy = 14;
	
	public const int DefaultMask = 1;
	public const int DebrisMask = 1<<8;
	public const int ProjectileMask = 1<<9;
	public const int HeroMask = 1<<10;
	public const int ItemMask= 1<<11;
	public const int CameraMask = 1<<12;
	public const int HazardMask = 1<<12;
	public const int EnemyMask = 1<<12;
	
	public static bool IsDefault(this GameObject go) { return go.layer == Default; }
	public static bool IsDebris(this GameObject go) { return go.layer == Debris; }
	public static bool IsProjectile(this GameObject go) { return go.layer == Projectile; }
	public static bool IsHero(this GameObject go) { return go.layer == Hero; }
	public static bool IsItem(this GameObject go) { return go.layer == Item; }
	public static bool IsCamera(this GameObject go) { return go.layer == Camera; }
	public static bool IsHazard(this GameObject go) { return go.layer == Hazard; }
	public static bool IsEnemy(this GameObject go) { return go.layer == Enemy; }
	
	public static bool IsTile(this GameObject go) { return go.CompareTag("Tile"); }
	
	public static bool IsDefault(this Component c) { return c.gameObject.layer == Default; }	
	public static bool IsDebris(this Component c) { return c.gameObject.layer == Debris; }
	public static bool IsProjectile(this Component c) { return c.gameObject.layer == Projectile; }
	public static bool IsHero(this Component c) { return c.gameObject.layer == Hero; }
	public static bool IsItem(this Component c) { return c.gameObject.layer == Item; }
	public static bool IsCamera(this Component c) { return c.gameObject.layer == Camera; }
	public static bool IsHazard(this Component c) { return c.gameObject.layer == Hazard; }
	public static bool IsEnemy(this Component c) { return c.gameObject.layer == Enemy; }
	
	public static bool IsTile(this Component c) { return c.CompareTag("Tile"); }
}

