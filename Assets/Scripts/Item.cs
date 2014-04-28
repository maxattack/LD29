using UnityEngine;
using System.Collections;

public abstract class Item : CustomBehaviour {
	
	internal Transform fx;	
	internal Transform xform;
	internal Vector2 dir;
	internal bool goodToCapture;



	//--------------------------------------------------------------------------------
	// GENERIC ITEM INTERFACE
	//--------------------------------------------------------------------------------

	public int ammo = 5;

	public virtual void Init() {  }

	public void Use(Vector2 dir)
	{
		if (ammo > 0)
				Operate (dir);
		ammo--;
	
	}
	public abstract void Operate (Vector2 dir);

	public virtual void OnPickUp() {}
	public virtual void OnDrop() {}
	
	internal void SetDir(Vector2 d)
	{
		dir = d;
		fx.localRotation = Quaternion.FromToRotation (new Vector3 (1, 0, 0), new Vector3 (dir.x, dir.y, 0));
	}
	
	//--------------------------------------------------------------------------------
	// POOLING
	//--------------------------------------------------------------------------------
	
	Item prefab;
	Item next;


	public bool IsPrefab { 
		get { return prefab == null; } 
	}
	
	
	public Item Alloc(Vector2 pos) {
		Assert(IsPrefab);
		
		Item result;
		
		if (next != null) {
			
			// RECYCLE INSTANCE
			result = next;
			next = result.next;
			result.next = null;
			result.xform.position = pos;
			result.gameObject.SetActive(true);
			
		} else {
			
			// CREATE NEW INSTANCE
			result = Dup(this, pos);
			Assert(result.IsItem());
			result.xform = result.transform;
			result.fx = result.xform.GetChild(0);
			result.prefab = this;
			
		}
		
		
		// RE-INIT INSTANCE
		result.goodToCapture = true;
		result.Init();
		
		
		return result;
	}
	
	public void Release() {
		
		if (Hero.inst && this == Hero.inst.currItem) {
			Hero.inst.currItem = null;
			fx.parent = xform;
			fx.Reset();
			this.rigidbody.detectCollisions = true;
			this.rigidbody.isKinematic = false;
		}
		
		if (prefab != null) {
			gameObject.SetActive(false);
			next = prefab.next;
			prefab.next = this;
		} else if (gameObject) {
			Destroy(gameObject);
		}
		
	}
	
	public void StopPhysics() {
		StopAllCoroutines();
		
		var body = this.rigidbody;
		if (!body.isKinematic) {
			body.velocity = Vector3.zero;
			body.isKinematic = true;
		}
		body.detectCollisions = false;
	}
	
	public void StartPhysics(Vector2 initialVelocity, bool deferCollider = false) {
		if (fx.parent != xform) {
			xform.position = fx.position;
			xform.rotation = fx.rotation;
			fx.parent = xform;
			// Not sure why we need this - sprite randomly shrinks sometimes :P
			fx.localScale = Vector3.one;
		}
		if (deferCollider) {
			goodToCapture = false;
			StartCoroutine(DoWaitToResetCapture());
		}
		var body = this.rigidbody;
		body.detectCollisions = true;
		body.isKinematic = false;
		body.AddForce(initialVelocity, ForceMode.VelocityChange);
	}
	
	IEnumerator DoWaitToResetCapture() {
		yield return new WaitForSeconds(0.5f);
		if (ammo <= 0)
						Release ();
		else
			goodToCapture = true;
	}
	
}
