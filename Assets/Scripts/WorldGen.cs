using UnityEngine;

using System.Collections;

public  class WorldGen : CustomBehaviour {

	public Transform tile;
	public Transform debris;

	public Sprite [] sprites;
	public Sprite [] debrisSprites;

	Transform [,] tiles;
	int height = 100;
	int width = 40;
    void Start()
    {
        
		//generate grid

	

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

	int depth = 0;
	float timer = 0;
	void Update()
	{

		timer += Time.deltaTime;
		if (timer > 1) 
		{
			DigShovel (15,depth);
			depth--;

			timer = 0;

		}

	}


	void SpawnDebrisAt(int x,int y)
	{
		for(int sy = 0 ; sy < 2 ; sy++)
		{
			for(int sx = 0 ; sx < 2 ; sx++)
			{

				var inst = Dup (debris);
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

	void Dig(int x,int y)
	{
		if(SafeDestroy(x,height + y))
		{
			SpawnDebrisAt(x,y);
		}
	}

	void DigShovel(int x,int y)
	{
		Dig (x, y);
		Dig (x - 1, y);
		Dig (x + 1, y);
		Dig (x, y - 1);

	}

}
