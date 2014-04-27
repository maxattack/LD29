using UnityEngine;
using System.Collections;

public class Projectile : CustomBehaviour {

	public PooledObject explosionPrefab;
	
	Projectile prefab;
	Projectile next;
	internal Transform xform;
	internal Rigidbody body;
	internal ParticleSystem particles;
	float timeout;
	
	public bool IsPrefab { get { return prefab == null; } }
	
	//--------------------------------------------------------------------------------
	// OBJECT POOL
	//--------------------------------------------------------------------------------
	
	public Projectile Alloc(Vector2 position, Vector2 heading) {
		Assert(IsPrefab);
		Projectile result;
		if (next != null) {
			// RECYCLE INSTANCE
			result = next;
			next = result.next;
			result.next = null;
			result.xform.position = position;
			result.gameObject.SetActive(true);
		} else {
			// CREATE NEW INSTANCE
			result = Dup(this, position);
			result.prefab = this;
		}
		// RE-INIT INSTANCE
		result.Init(heading.normalized);
		return result;
	}	

	void Init(Vector2 initDir) {
		timeout = 0f;
		particles.Clear();		
		body.velocity = Hero.inst.body.velocity.magnitude * initDir;
		body.rotation = Quaternion.FromToRotation (new Vector3 (1, 0, 0), initDir);
	}
	
	
	public void Release() {
		if (prefab != null) {
			gameObject.SetActive(false);
			next = prefab.next;
			prefab.next = this;
		} else if (gameObject) {
			Destroy(gameObject);
		}
	}	
	
	//--------------------------------------------------------------------------------
	// EVENTS
	//--------------------------------------------------------------------------------
	
	void Awake() {
		xform = this.transform;
		body = this.rigidbody;
		body.centerOfMass = new Vector3 (-0.4f, 0, 0);
		particles = GetComponentInChildren<ParticleSystem>();
	}
	
	void FixedUpdate() {
		timeout += Time.fixedDeltaTime;
		if (timeout > 10f) {
			Release();
		} else {	
			body.AddForce (Vec(0, -1, 0));
			body.AddForce (xform.right * 10f); 
		}

	}

	void OnCollisionEnter(Collision collision) {
		if (collision.collider.IsTile()) {
			var p = collision.transform.position;
			WorldGen.inst.DigRocket(Mathf.FloorToInt(p.x), Mathf.FloorToInt(p.y));
			
		}
		CameraFX.inst.Shake();
		CameraFX.inst.Flash(RGBA(Color.red, 0.5f));
		explosionPrefab.Alloc(xform.position);
		Release();
		
	}
}
