using UnityEngine;

using System.Collections;

public  class WorldGen : CustomBehaviour {

	public Transform tile;

	public Sprite [] sprites;

    void Start()
    {
        


        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                var inst = Dup(tile);

                inst.position = new Vector2(x, 0-y);

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

    }

}
