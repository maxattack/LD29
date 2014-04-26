using UnityEngine;

using System.Collections;

public  class WorldGen : CustomBehaviour {

	public Transform tile;

	public Sprite [] sprites;

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

                inst.position = new Vector2(x, 0-y);
				tiles[x,y] = inst;

                if (y == 0)
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
		System.Random rand = new System.Random ();

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

	void SafeDestroy(int x,int y)
	{
		if(y < height && y >= 0 && x < width && x >= 0)
			Destroy(tiles[x,y].gameObject);
	}

}
