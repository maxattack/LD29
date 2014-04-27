using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class Tile : PooledObject {
	
	internal SpriteRenderer spr;
	internal int tileX, tileY;
	
	void Awake() {
		// Make sure we're tagged correctly
		Assert(this.IsTile());
		spr = GetComponent<SpriteRenderer>();
	}
	
	public override void Init() {
		WorldToCoord(transform.position.xy(), out tileX, out tileY);
		
		spr.color = (tileX + tileY) % 2 == 0 ? RGB(0.9f, 0.9f, 0.9f) : Color.white;
	}
	
	public override void Deinit() {
		WorldGen.inst.tiles[tileX, tileY] = null;
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
