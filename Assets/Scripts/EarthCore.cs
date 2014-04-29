using UnityEngine;
using System.Collections;

public class EarthCore : CustomBehaviour {


	public PooledObject explosionPrefab;
	internal Transform xform;
	// Use this for initialization
	void Start () {
		xform = transform;
	}


	internal bool dying = false;

	// Update is called once per frame
	void Update () {


		GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color.EaseTowards( new Color(1,1,1,0),Time.deltaTime * 8);




	}

	internal void StartDestroy()
	{
		CameraFX.inst.Flash(RGBA(Color.white, 1.0f));
		CameraFX.inst.Shake(5);
		
		
		GetComponent<SpriteRenderer>().sprite = sadFace;
		GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
		
		if (!dying) {
			dying = true;		
			StartCoroutine(DeathSequence());
		}
	}

	public Sprite sadFace;

	void OnCollisionEnter(Collision collision) 
	{
		if (collision.collider.IsProjectile()) {

			StartDestroy ();
		}

	}

	IEnumerator DeathSequence()
	{
		System.Random rand = new System.Random ();
		float radius = GetComponent<SphereCollider> ().radius * 0.75f;
		for(int i = 0 ; i < 30 ; i++)
		{
			float size = 2;
			if(i > 15)
			{
				yield return new WaitForSeconds(0.1f);
				size = 5;
			}
			else if(i > 5)
			{
				yield return new WaitForSeconds(0.2f);
				size = 3;
			}
			else
			{
				yield return new WaitForSeconds(0.3f);
				size = 2;
			}
			
			Vector3 p = xform.position;

			float xMod = (float)rand.NextDouble() * (radius * 2) - radius;
			float yMod = (float)rand.NextDouble() * (radius * 2) - radius;

			CameraFX.inst.Shake(size);

			PooledObject inst = explosionPrefab.Alloc (p + new Vector3(xMod,yMod,0)) as PooledObject;
			inst.transform.localScale = new Vector3 (size,size, 1);

			GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);

			xform.position += new Vector3( ((float)rand.NextDouble() - 0.5f) * 0.5f,((float)rand.NextDouble() - 0.5f) * 0.5f,0);


		}
		yield return new WaitForSeconds(0.5f);
		Application.LoadLevel("outro");
	}
}
