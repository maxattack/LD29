using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class Tile : PooledObject {
	
	internal SpriteRenderer spr;
	internal int tileX, tileY;
	public int health = 1;
	
	void Awake() {
		// Make sure we're tagged correctly
		Assert(this.IsTile());
		spr = GetComponent<SpriteRenderer>();
	}

	float whiteTimer = 0;
	internal void TakeDamage(int dmg)
	{
		health -= dmg;
		spr.color = new Color (1, 1, 1, 1);
		whiteTimer = 0.1f;

		baseColor.a = 0.5f;

	}

	void Update()
	{
		whiteTimer -= Time.deltaTime;
		if (whiteTimer < 0) {
			spr.color = spr.color.EaseTowards(baseColor,0.5f);
				}


	}
	Color baseColor;
	public override void Init() {
		WorldToCoord(transform.position.xy(), out tileX, out tileY);
		
		baseColor = (tileX + tileY) % 2 == 0 ? RGBA(0.0f, 0.0f, 0.0f,0.1f) : new Color(0,0,0,0);
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
