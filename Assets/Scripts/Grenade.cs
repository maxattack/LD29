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
	
	// Update is called once per frame
	void Update () {
		timeout += Time.deltaTime;
		if (timeout > 2) {

			WorldGen.inst.DigGrenade((int)xform.position.x,(int)xform.position.y);
			CameraFX.inst.Shake();
			CameraFX.inst.Flash(RGBA(Color.white, 0.5f));
			explosionPrefab.Alloc(xform.position);
			Release();

				}
	}

	public override void Init()
	{
		timeout = 0f;
	}
	public float speed = 100;

	float timeout;
	internal void SetInitDir(Vector2 initDir) {


		body.velocity = initDir * (speed + Hero.inst.body.velocity.magnitude);
		body.rotation = Quaternion.FromToRotation (new Vector3 (1, 0, 0), initDir);
	}
}
