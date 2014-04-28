using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class Tile : PooledObject {
	
	internal SpriteRenderer spr;
	internal int tileX=-1, tileY=-1;
	public int health = 1;
	internal Color baseColor;
	float whiteTimer = 0;
	
	//--------------------------------------------------------------------------------
	// EVENTS
	//--------------------------------------------------------------------------------
	
	protected virtual void Awake() {
		// Make sure we're tagged correctly
		Assert(this.IsTile());
		spr = GetComponent<SpriteRenderer>();
	}

	protected virtual void Update()
	{
		whiteTimer -= Time.deltaTime;
		if (whiteTimer < 0) {
			spr.color = spr.color.EaseTowards(baseColor,0.5f);
		}
	}
	
	public override void Init() {
		WorldToCoord(transform.position.xy(), out tileX, out tileY);
		
		baseColor = (tileX + tileY) % 2 == 0 ? RGBA(0.0f, 0.0f, 0.0f,0.1f) : new Color(0,0,0,0);
	}
	
	public override void Deinit() {
		if (tileX >= 0 && tileY >= 0 && WorldGen.inst.tiles[tileX, tileY] == this) {
			WorldGen.inst.tiles[tileX, tileY] = null;
		}
	}	
	
	//--------------------------------------------------------------------------------
	// DAMAGE
	//--------------------------------------------------------------------------------	
	
	internal void TakeDamage(int dmg)
	{
		health -= dmg;
		spr.color = new Color (1, 1, 1, 1);
		whiteTimer = 0.1f;
		baseColor.a = 0.5f;
		if (health <= 0) {
			var height = WorldGen.inst.height;
			WorldGen.inst.SpawnDebrisAt(tileX, tileY - height);
			Vector3 pos = transform.position;
			int range = 30;
			if(tileY < (float)height * 0.2f) { range = 5; }
			if(tileY < (float)height * 0.4f) { range = 7; }
			if(tileY < (float)height * 0.6f) { range = 10; }
			if(tileY < (float)height * 0.8f) { range = 20; }
			int r = Random.Range(0, range);
			if(r == 0) {
				WorldGen.inst.landMine.Alloc(pos);
			}
			WillDestroy();
			Release();
		}
	}
	
	protected virtual void WillDestroy() {}	
	
	//--------------------------------------------------------------------------------
	// POOLING
	//--------------------------------------------------------------------------------
	
	public Tile AllocAtCoord(int x, int y) {
		return Alloc(CoordToWorld(x,y)) as Tile;
	}
	
	//--------------------------------------------------------------------------------
	// COORDINATE TRANSFORMS
	//--------------------------------------------------------------------------------
	
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
