using UnityEngine;

using System.Collections;

public  class WorldGen : CustomBehaviour {

	public EarthCore earthCorePrefab;

	internal EarthCore earthCore;
	public Dino dinoPrefab;

	public Tile grassTile;
	public Tile dirtTile;
	public Tile stoneTile;

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
        
		//generate grid
		items[0].Alloc (new Vector2 (10, 10));
		items[3].Alloc (new Vector2 (5, 10));
		items[2].Alloc (new Vector2 (1, 10));

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



		//place dinosaurs in the caverns
		for (int y = 0; y < height - 5; y++) 
		{
				for (int x = 1; x < width - 1; x++) 
				{
						//put in stone tiles, there are a few patterns to use
						int r = rand.Next () % 20;
						if (r == 0) 
						{
							if(tiles[x,y] == false)
							{
								dinoPrefab.Alloc( Tile.CoordToWorld(x,y) );
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

	void SpawnDebrisAt(int x,int y)
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
		return tiles[x,y] != null;
	}

	internal bool Dig(int x,int y,int dmg)
	{
		
		y += height;
		if (y < height && y >= 0 && x < width && x >= 0) 
		{
			if(IsTileAt(x,y))
			{
				tiles[x,y].TakeDamage(dmg);
				if(tiles[x,y].health <= 0)
				{
					SpawnDebrisAt(x,y - height);

					Vector3 pos = tiles [x, y].transform.position;
					int range = 30;
					if(y < (float)height * 0.2f)
						range = 5;
					if(y < (float)height * 0.4f)
						range = 7;
					if(y < (float)height * 0.6f)
						range = 10;
					if(y < (float)height * 0.8f)
						range = 20;
					
					int r = rand.Next() % range;
					if(r == 0)
						landMine.Alloc(pos + new Vector3 (0,0,0));

					tiles[x,y].Release();
				}
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
		int dmg = 3;
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
		
		return result;
		
	}
	
	



	void OnDestroy() {
		
		// RELEASE SINGLETON REFERENCE
		if (inst == this) { inst = null; }
		
	}

}
