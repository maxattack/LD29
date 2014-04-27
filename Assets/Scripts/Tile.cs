using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class Tile : PooledObject {
	
	internal SpriteRenderer spr;
	
	void Awake() {
		// Make sure we're tagged correctly
		Assert(this.IsTile());
		spr = GetComponent<SpriteRenderer>();
	}
	
	public override void Deinit() {
		int x,y;
		WorldToCoord(transform.position.xy(), out x, out y);
		WorldGen.inst.tiles[x,y] = null;
	}	
	
	
	public Tile AllocAtCoord(int x, int y) {
		return Alloc(CoordToWorld(x,y)) as Tile;
	}
	
	public static Vector2 CoordToWorld(int x, int y) {
		return Vec(x,y-WorldGen.inst.height);
	}
	
	public static bool WorldToCoord(Vector2 position, out int x, out int y) {
		// TRUE IF THE COORDINATE IS IN-BOUNDS
		x = Mathf.RoundToInt(position.x);
		y = Mathf.RoundToInt(position.y) + WorldGen.inst.height;
		return x >= 0 && x < WorldGen.inst.width && y >= 0 && y < WorldGen.inst.height;
		
	}
	
}
