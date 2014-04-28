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
		
			if(timeout + Time.deltaTime > explodeTime - 0.1f && timeout < explodeTime - 0.1f)
					{
				Jukebox.Play ("beep1");
					}

					flickerTime -= Time.deltaTime;
					if (flickerTime < 0) {
							Color c = GetComponent<SpriteRenderer> ().color;
							if (c.a == 1.0f)
									c.a = 0.0f;
							else
							{
								c.a = 1.0f;
								if(timeout < explodeTime - 0.1f)
									Jukebox.Play ("beep0");
								
							}
			
							GetComponent<SpriteRenderer> ().color = c;
			
							if (timeout < explodeTime - 1.0f) 
							{
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

						float dist = 1.0f;
							Vector3 [] offsets = new Vector3[6];
						
						offsets[0] = new Vector3(dist * 1,0,0);
						offsets[1] = new Vector3(-dist * 1,0,0);
						offsets[2] = new Vector3(0,dist * 1,0);
						offsets[3] = new Vector3(dist * 2,0,0);
						offsets[4] = new Vector3(-dist * 2,0,0);
						offsets[5] = new Vector3(0,dist * 2,0);

							for(int i = 0 ; i < offsets.Length ; i++)
							{
								Vector3 blastPos = xform.position + offsets[i];

								PooledObject inst = explosionPrefab.Alloc (xform.position + offsets[i]) as PooledObject;
								float size = 0.45f;
								inst.transform.localScale = new Vector3 (size,size,size );


								WorldGen.inst.Dig((int)(blastPos.x + 0.5f),(int)(blastPos.y ),3);
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
		GetComponent<SpriteRenderer> ().color = new Color(1,1,1,0);

	}

	internal void Trip()
	{
		tripped = true;
	}
	void OnCollisionEnter(Collision collision) 
	{
				if (collision.collider.IsHero ()) {
						tripped = true;
						
				}
				if (collision.collider.IsEnemy ()) {
					tripped = true;
					
				}
				if (collision.collider.IsProjectile ()) {
				
						tripped = true;
						timeout = 10000;
				}
		}
}
