using UnityEngine;

using System.Collections;

public  class WorldGen : CustomBehaviour {

	public Transform tile;
	public Debris debrisPrefab;
	public Transform rocket;
	
	public Item[] items;	

	public Sprite [] sprites;
	public Sprite [] debrisSprites;

	internal static WorldGen inst;

	Transform [,] tiles;
	public int height = 100;
	public int width = 40;
    
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

		tiles = new Transform[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var inst = Dup(tile);

                inst.position = new Vector2(x, y - height);
				tiles[x,y] = inst;

                if (y == height-1)
                {
                    
                    inst.GetComponent<SpriteRenderer>().sprite = sprites[0];

                }
                else
                {
                    inst.GetComponent<SpriteRenderer>().sprite = sprites[1];
                }
            }
        }

		//put caverns in (small medium and large
	
		for(int y = 5 ; y < height ; y++)
		{
			int r = rand.Next () % 2;
			if(r == 0)
			{
				//punch a hole
				int size = rand.Next () % 3; //0 - 3 small -> big
				int x = rand.Next () % width;

				if(size >= 0)
				{
					Destroy(tiles[x,y].gameObject);

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
    }
	System.Random rand = new System.Random ();

	bool SafeDestroy(int x,int y)
	{
		if (y < height && y >= 0 && x < width && x >= 0) 
		{
			if(tiles[x,y])
			{
				Destroy (tiles [x, y].gameObject);
				return true;
			}
		}
		return false;
	}

	float timer = 0;
	void Update()
	{

		timer += Time.deltaTime;



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

	bool Dig(int x,int y)
	{


		if(SafeDestroy(x,height + y))
		{
			SpawnDebrisAt(x,y);
			return true;
		} else {
			return false;
		}
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
