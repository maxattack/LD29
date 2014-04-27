using UnityEngine;

using System.Collections;

public  class WorldGen : CustomBehaviour {

	public EarthCore earthCore;


	public Tile grassTile;
	public Tile dirtTile;

	public Debris debrisPrefab;
	public LandMine landMine;
	
	public Item[] items;	
	

	public Sprite [] sprites;
	public Sprite [] debrisSprites;

	internal static WorldGen inst;

	public int height = 100;
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
		items[1].Alloc (new Vector2 (5, 10));
		items[2].Alloc (new Vector2 (1, 10));

		tiles = new Tile[width, height];

        for (int y = 0; y < height; y++)
		for (int x = 0; x < width; x++) {
			tiles[x,y] = (y == height - 1 ? grassTile : dirtTile).AllocAtCoord(x,y);
        }

		//put caverns in (small medium and large
	
		for(int y = 0 ; y < height - 5; y++)
		{
			int r = rand.Next () % 2;
			if(r == 0)
			{
				//punch a hole
				int size = rand.Next () % 3; //0 - 3 small -> big
				int x = rand.Next () % width;

				if(size >= 0)
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

		for (int y = 0; y < height - 1; y++) 
		{
			for(int x = 1 ; x < width - 1 ; x++)
			{


				{
					//bool validPlacement = false;
					//if(!tiles[x,y])
					//	validPlacement = true;

					//if(validPlacement)
					//{
					//	int r = rand.Next() % 10; //roll for land mine
					//	if(r == 0)
					//		landMine.Alloc(new Vector3 (x,y - height,0));


					//}

				}

			}

		}


		//Add the earth's core

		var inst = Dup (earthCore);
		inst.transform.position = new Vector3 (width / 2, -height - 25, 0);
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

	internal bool Dig(int x,int y)
	{
		
		y += height;
		if (y < height && y >= 0 && x < width && x >= 0) 
		{
			if(IsTileAt(x,y))
			{
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

				SpawnDebrisAt(x,y - height);
				tiles[x,y].Release();
				return true;
			}

		}


		return false;
	}

	internal bool DigShovel(int x,int y)
	{
		var result = Dig (x, y);
		result |= Dig (x - 1, y);
		result |= Dig (x + 1, y);
		result |= Dig (x, y - 1);
		return result;

	}

	internal bool DigRocket(int x,int y)
	{
		var result = Dig (x, y);

		result |= Dig (x - 1, y - 1);
		result |= Dig (x + 1, y + 1);
		result |= Dig (x + 1, y - 1);
		result |= Dig (x - 1, y + 1);

		result |= Dig (x - 1, y);
		result |= Dig (x + 1, y);
		result |= Dig (x, y - 1);
		result |= Dig (x, y + 1);


		return result;
	}

	internal bool DigGrenade(int x,int y)
	{
		var result = DigShovel (x, y);

		result |= Dig (x - 2, y);
		result |= Dig (x + 2, y);
		result |= Dig (x, y - 2);
		result |= Dig (x, y + 2);


		result |= Dig (x - 1, y - 1);
		result |= Dig (x + 1, y + 1);
		result |= Dig (x + 1, y - 1);
		result |= Dig (x - 1, y + 1);
		
		return result;
		
	}
	
	



	void OnDestroy() {
		
		// RELEASE SINGLETON REFERENCE
		if (inst == this) { inst = null; }
		
	}

}
