using UnityEngine;
using System.Collections;

public class Grenade : PooledObject {

	Transform xform;
	Rigidbody body;
	public PooledObject explosionPrefab;

	void Awake() {
		xform = this.transform;
		body = this.rigidbody;

	
	}
	// Use this for initialization
	void Start () {
	

	}


	void FixedUpdate()
	{
		float len = body.velocity.magnitude;
		if (len > 50) {
						Vector3 dir = body.velocity / len;
						body.velocity = dir * 50;
				}

	}

	public float explodeTime = 3.0f;
	float flickerTime = 0;
	// Update is called once per frame
	void Update () {
		timeout += Time.deltaTime;

		flickerTime -= Time.deltaTime;
		if (flickerTime < 0)
		{
			Color c = GetComponent<SpriteRenderer> ().color;
			if (c.a == 1.0f)
					c.a = 0.0f;
			else
					c.a = 1.0f;

			GetComponent<SpriteRenderer> ().color = c;

			if(timeout < explodeTime - 1.0f)
			{
				if(c.a == 1.0f)
					flickerTime = 0.1f;
				else
					flickerTime = 0.5f;
			}
			else
				flickerTime = 0.05f;
		}


		if (timeout > explodeTime) {

			WorldGen.inst.DigGrenade((int)xform.position.x,(int)xform.position.y);
			CameraFX.inst.Shake();
			CameraFX.inst.Flash(RGBA(Color.white, 0.5f));
			PooledObject inst = explosionPrefab.Alloc(xform.position) as PooledObject;
			inst.transform.localScale = new Vector3(3,3,1);
			Release();

				}
	}

	public override void Init()
	{
		playOnce = true;
		timeout = 0f;
	}
	public float speed = 100;

	float timeout;
	internal void SetInitDir(Vector2 initDir) {


		body.velocity = initDir * (speed + Hero.inst.body.velocity.magnitude);
		body.rotation = Quaternion.FromToRotation (new Vector3 (1, 0, 0), initDir);
	}

	bool playOnce = true;
	void OnCollisionEnter(Collision collision) {
		if (collision.collider.IsTile()) {

			if( playOnce)
			{
				Jukebox.Play("GrenadeBounce");
				playOnce = false;
			}
			
		}


		
	}
}
