using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {



	Transform xform;

	// Use this for initialization
	void Start () {
		xform = transform;

		rigidbody.centerOfMass = new Vector3 (-0.4f, 0, 0);

		initDir.Normalize ();

		rigidbody.rotation = Quaternion.FromToRotation (new Vector3 (1, 0, 0), initDir);
	}

	public Vector3 initDir = new Vector3(1,0,0);
	void FixedUpdate()
	{

		rigidbody.AddForce (new Vector3 (0, -1, 0));
		rigidbody.AddForce (initDir * 10); 

	}

	// Update is called once per frame
	float totalTime = 0;
	void Update () {
	
		totalTime += Time.deltaTime;



	}


	void OnCollisionEnter(Collision collision) {

		GameObject obj = collision.collider.gameObject;
		if (obj) {

			GameObjUserData ud = obj.GetComponent<GameObjUserData>();
			if(ud )
			{
				if(ud.goType == GameObjUserData.GOType.Tile)
				{
					int x = (int)obj.transform.position.x;
					int y = (int)obj.transform.position.y;
					WorldGen.inst.DigRocket(x,y);

					Destroy (gameObject);
				}
			}

				}
		
	}
}
