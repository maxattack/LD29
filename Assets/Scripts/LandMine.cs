using UnityEngine;
using System.Collections;

public class LandMine : PooledObject {

	public PooledObject explosionPrefab;
	Transform xform;

	void Awake() {
		xform = this.transform;

		
		
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (tripped) {
					timeout += Time.deltaTime;
		
					flickerTime -= Time.deltaTime;
					if (flickerTime < 0) {
							Color c = GetComponent<SpriteRenderer> ().color;
							if (c.a == 1.0f)
									c.a = 0.0f;
							else
									c.a = 1.0f;
			
							GetComponent<SpriteRenderer> ().color = c;
			
							if (timeout < explodeTime - 1.0f) {
									if (c.a == 1.0f)
											flickerTime = 0.1f;
									else
											flickerTime = 0.5f;
							} else
									flickerTime = 0.05f;
					}
		
		
					if (timeout > explodeTime) {
			
			
							CameraFX.inst.Shake ();
							CameraFX.inst.Flash (RGBA (Color.white, 0.5f));

						float size = 0.75f;
							Vector3 [] offsets = new Vector3[7];
						offsets[0] = Vector3.zero;
						offsets[1] = new Vector3(size * 2,0,0);
						offsets[2] = new Vector3(-size * 2,0,0);
						offsets[3] = new Vector3(0,size * 2,0);
						offsets[4] = new Vector3(size * 4,0,0);
						offsets[5] = new Vector3(-size * 4,0,0);
						offsets[6] = new Vector3(0,size * 4,0);

							for(int i = 0 ; i < offsets.Length ; i++)
							{
								Vector3 blastPos = xform.position + offsets[i];

								PooledObject inst = explosionPrefab.Alloc (xform.position + offsets[i]) as PooledObject;
								inst.transform.localScale = new Vector3 (size,size,size);


								WorldGen.inst.Dig((int)(blastPos.x),(int)(blastPos.y));
							}
						

						Release ();
			
					}
			}

	}
	float flickerTime = 0;
	float timeout = 0;
	public static float explodeTime = 1.5f;
	bool tripped = false;
	public override void Init()
	{
			timeout = 0;
			tripped = false;

	}

	void OnCollisionEnter(Collision collision) 
	{
				if (collision.collider.IsHero ()) {
						tripped = true;
						
				}
				if (collision.collider.IsProjectile ()) {
				
						tripped = true;
						timeout = 10000;
				}
		}
}
