using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public  class WorldGen : CustomBehaviour {

	public EarthCore earthCorePrefab;

	internal EarthCore earthCore;
	public Dino dinoPrefab;

	public Tile grassTile;
	public Tile dirtTile;
	public Tile stoneTile;
	public Tile wormTile;

	public Debris debrisPrefab;
	public LandMine landMine;
	
	public Item[] items;	
	

	public Sprite [] sprites;
	public Sprite [] debrisSprites;

	internal static WorldGen inst;

	public int height = 40;
	public int width = 40;
	internal Tile [,] tiles;
	
	void Awake()
	{
		
		inst = this;
	}
	
	
	void OnDrawGizmos() {
		if (!Application.isPlaying) {
			Gizmos.color = Color.yellow;
			var left = Vec(-0.5f,-0.5f,0);
			var right = Vec(width-0.5f,-0.5f,0);
			var nibNeight = 0.1f;
			Gizmos.DrawLine(left, right);
			Gizmos.DrawLine (left.Above(nibNeight), left.Below(nibNeight));
			Gizmos.DrawLine(right.Above(nibNeight), right.Below(nibNeight));
		}
		
	}
	
	

	void Start()
    {

		//items [1].Alloc (new Vector2 (5, 10));

		items [1].Alloc (new Vector2 (5, 10));

		items [3].Alloc (new Vector2 (Hero.inst.xform.position.x - 3, 10));



		//generate grid


		tiles = new Tile[width, height];

        for (int y = 0; y < height; y++)
		for (int x = 0; x < width; x++) {
			tiles[x,y] = (y == height - 1 ? grassTile : dirtTile).AllocAtCoord(x,y);
        }

		//put caverns in (small medium and large
	
		for(int y = 0 ; y < height - 5; y++)
		{
			for(int x = 0 ; x < width ; x++)
			{
				int r = rand.Next () % 30;
				if(r == 0)
				{
					if (IsTileAt(x,y)) {
						tiles[x,y].Release();
					}

					SafeDestroy(x,y+1);
					SafeDestroy(x,y-1);
					SafeDestroy(x+1,y);
					SafeDestroy(x-1,y);

					int holeWidth = 1 + rand.Next () % 5;
					for(int i = 0 ; i < holeWidth ; i++)
					{
						SafeDestroy(x+1 + i,y+1);
						SafeDestroy(x+1 + i,y-1);
						SafeDestroy(x+2 + i,y);

					}


				}

			}

		}

		//set up stone tiles
		for (int y = 0; y < height - 5; y++) 
		{
			for(int x = 1 ; x < width - 1 ; x++)
			{
				//put in stone tiles, there are a few patterns to use
				int r = rand.Next () % 30;
				if(r == 0)
				{

					int p = rand.Next() % 5;
					if(p > 0)
					{
						int stoneWidth = 3 + rand.Next () % 6;
						for(int i = 0 ; i < stoneWidth ; i++)
						{
							if( rand.Next() % 100 < 80)
								TryPlaceStone(x + i - 1,y +1);
							if( rand.Next() % 100 < 80)
								TryPlaceStone(x + i,y);
							if( rand.Next() % 100 < 80)
								TryPlaceStone(x + i - 1,y -1);
							
						}
					}
					else if(p == 0)
					{
						int stoneHeight = 2 + rand.Next () % 4;
						for(int i = 0 ; i < stoneHeight ; i++)
						{
							if( rand.Next() % 100 < 80)
								TryPlaceStone(x,y + i);
							if( rand.Next() % 100 < 80)
								TryPlaceStone(x - 1,y + i - 1);

						}
					}




				}


			}

		}

		// set up some worms
		for(int x=0; x<width; ++x) 
		for(int y=0; y<height-10; ++y) {
			TryPlaceWorm(x,y);
		}
		

		//place stuff in the caverns
		for (int y = 0; y < height - 5; y++) 
		{
				for (int x = 1; x < width - 1; x++) 
				{
					if(tiles[x,y] == false)
					{
						//dinosaurs
					    if( y < height - 30)
						{
							int r = rand.Next () % 30;
							if (r == 0) 
							{
								
								{
									dinoPrefab.Alloc( Tile.CoordToWorld(x,y) );
								}

							}
						}
						{
							//weapons
							int r = rand.Next()% 50;
							if(r == 0)
							{
								int w = 1 + rand.Next () % (items.Length-1);
								items[w].Alloc (Tile.CoordToWorld(x,y));
							
							}
						}
					}
				}
		}


		//Add the earth's core

		earthCore = Dup (earthCorePrefab);
		earthCore.transform.position = new Vector3 (width / 2, -height - 25, 0);
    }

	void TryPlaceStone(int x,int y)
	{
		if (y < height && y >= 0 && x < width && x >= 0) 
		{
				if (IsTileAt (x, y)) {
						tiles [x, y].Release ();
						tiles [x, y] = stoneTile.AllocAtCoord (x, y); 
				}
		}
	}
	
	bool IsDirt(int x, int y) {
		return IsTileAt(x,y) && tiles[x,y].SourcePrefab == dirtTile;
	}
	
	bool CanPlaceWormAt(int x, int y) {
		// is a 4x4 grid of dirt tiles open?
		for(int u=0; u<4; ++u)
		for(int v=0; v<4; ++v) {
			if (!IsDirt(x+u, y+v)) { return false; }
		}
		return true;
	}
	
	void TryPlaceWorm(int x, int y) {
		if (!CanPlaceWormAt(x,y)) {
			return;
		}
		
		// pick a random start corner
		switch(Random.Range(0,4)) {
			case 1: x+=3; break;
			case 2: x+=3; y+=3; break;
			case 3: y+=3; break;
		}
		
		// alloc the head
		var wormTiles = new List<WormTile>(5);
		var curr = wormTile.AllocAtCoord(x,y) as WormTile;
		curr.wPrev = null;
		wormTiles.Add(curr);
		tiles[x,y].Release();
		tiles[x,y] = curr;
		
		// alloc the others			
		var nhood = new List<Tile>();
		while(wormTiles.Count < wormTiles.Capacity) {
			// pick diry neighbor
			nhood.Clear();
			if (IsDirt(x+1,y)) { nhood.Add(tiles[x+1,y]); }
			if (IsDirt(x,y+1)) { nhood.Add(tiles[x,y+1]); }
			if (IsDirt(x-1,y)) { nhood.Add(tiles[x-1,y]); }
			if (IsDirt(x,y-1)) { nhood.Add(tiles[x,y-1]); }
			if (nhood.Count == 0) {	break; }
			var tile = nhood[Random.Range(0,nhood.Count)];
			
			// append worm tile				
			x = tile.tileX;
			y = tile.tileY;
			var next = wormTile.AllocAtCoord(x,y) as WormTile;
			curr.wNext = next;
			next.wPrev = curr;
			wormTiles.Add(next);
			curr = next;
			tiles[x,y].Release();
			tiles[x,y] = curr;
		}
		curr.wNext = null;
		
		// don't place small worms
		if (wormTiles.Count < 3) {
			foreach(var tile in wormTiles) {
				tile.Release();
			}
			
			return;
		}
		
		foreach(var tile in wormTiles) {
			tile.InitFX();
		}
			
	}
	
	System.Random rand = new System.Random ();



	bool SafeDestroy(int x,int y)
	{
		if (y < height && y >= 0 && x < width && x >= 0) 
		{
			if(tiles[x,y])
			{
				tiles[x,y].Release();
				return true;
			}
		}
		return false;
	}

	internal void SpawnDebrisAt(int x,int y)
	{
		for(int sy = 0 ; sy < 2 ; sy++)
		{
			for(int sx = 0 ; sx < 2 ; sx++)
			{

				var inst = debrisPrefab.Alloc();
				inst.transform.position = new Vector2 (x + sx * 0.5f,y + sy * 0.5f);



				inst.rigidbody.AddForce (new Vector3 ((float)rand.NextDouble() - 0.5f * 50, 300, 0));
				if(y > height -1) //hacky whatever
					inst.GetComponent<SpriteRenderer> ().sprite = debrisSprites [0];
				else
					inst.GetComponent<SpriteRenderer> ().sprite = debrisSprites [1];



				float c = (float)rand.NextDouble() * 0.25f + 0.65f;
				inst.GetComponent<SpriteRenderer> ().color = new Color(c,c,c);

			}
		}

	}

	internal bool IsTileAt(int x, int y) {
		return x >= 0 && y >= 0 && x < width && y < height && tiles[x,y] != null;
	}

	internal bool Dig(int x,int y,int dmg) {
		y += height;
		if (y < height && y >= 0 && x < width && x >= 0) {
			if(IsTileAt(x,y)) {
				tiles[x,y].TakeDamage(dmg);
				return true;
			}
		}
		return false;
	}

	internal bool DigShovel(int x,int y)
	{
		int dmg = 1;
		var result = Dig (x, y,dmg);
		result |= Dig (x - 1, y,dmg);
		result |= Dig (x + 1, y,dmg);
		result |= Dig (x, y - 1,dmg);
		return result;

	}

	internal bool DigRocket(int x,int y)
	{
		int dmg = 6;
		var result = Dig (x, y,dmg);

		result |= Dig (x - 1, y - 1,dmg);
		result |= Dig (x + 1, y + 1,dmg);
		result |= Dig (x + 1, y - 1,dmg);
		result |= Dig (x - 1, y + 1,dmg);

		result |= Dig (x - 1, y,dmg);
		result |= Dig (x + 1, y,dmg);
		result |= Dig (x, y - 1,dmg);
		result |= Dig (x, y + 1,dmg);


		return result;
	}

	internal bool DigGrenade(int x,int y)
	{
		int dmg = 6;
		var result = Dig (x, y,dmg);
		result |= Dig (x - 1, y,dmg);
		result |= Dig (x + 1, y,dmg);
		result |= Dig (x, y - 1,dmg);

		result |= Dig (x - 2, y,dmg);
		result |= Dig (x + 2, y,dmg);
		result |= Dig (x, y - 2,dmg);
		result |= Dig (x, y + 2,dmg);


		result |= Dig (x - 1, y - 1,dmg);
		result |= Dig (x + 1, y + 1,dmg);
		result |= Dig (x + 1, y - 1,dmg);
		result |= Dig (x - 1, y + 1,dmg);



		result |= Dig (x - 1, y - 2,dmg);
		result |= Dig (x - 1, y + 2,dmg);

		result |= Dig (x + 1, y + 2,dmg);
		result |= Dig (x + 1, y - 2,dmg);


		result |= Dig (x - 2, y - 1,dmg);
		result |= Dig (x + 2, y - 1,dmg);
		
		result |= Dig (x - 2, y + 1,dmg);
		result |= Dig (x + 2, y + 1,dmg);



		return result;
		
	}
	
	



	void OnDestroy() {
		
		// RELEASE SINGLETON REFERENCE
		if (inst == this) { inst = null; }
		
	}

}
