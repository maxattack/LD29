using UnityEngine;
using System.Collections;

public abstract class Item : CustomBehaviour {
	
	internal Transform fx;	
	internal Transform xform;
	internal Vector2 dir;
	
	//--------------------------------------------------------------------------------
	// GENERIC ITEM INTERFACE
	//--------------------------------------------------------------------------------
	
	public abstract void Init();
	public abstract void Operate(Vector2 dir);
	
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
			result.prefab = this;
			
		}
		
		// RE-PARENT FX
		result.fx.parent = result.xform;
		result.fx.Reset();
		
		// RE-INIT INSTANCE
		result.Init();
		
		return result;
	}
	
	public void Release() {
		
		if (prefab != null) {
			
			// DEACTIVATE AND PREPEND TO PREFAB'S FREELIST
			gameObject.SetActive(false);
			next = prefab.next;
			prefab.next = this;

			
		} else if (gameObject) {
			
			// THIS OBJECT WAS NOT DYNAMICALLY CREATED
			Destroy(gameObject);
			
		}
		
	}
	
	
	
	//--------------------------------------------------------------------------------
	// EVENTS	
	//--------------------------------------------------------------------------------
	
	
	void Awake() {
		xform = transform;
		fx = xform.GetChild(0);
	}
	
	void Update () 
	{
		if (this == Hero.inst.currItem) {
			xform.localPosition = xform.localPosition.EaseTowards (Vector3.zero, 0.1f); 
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		
		print("DIR: " + dir);
		print("fx.pos = " + fx.position);
		
		Vector3 digPos = new Vector3 ((int)(fx.position.x + dir.x + 0.5f), (int)(fx.position.y + dir.y + 0.5f), 0);
		Gizmos.DrawWireCube(digPos, new Vector3(1.0f,1.0f,1.0f));

		Vector3 digPosRaw = new Vector3 ((fx.position.x + dir.x), (fx.position.y + dir.y), 0);
		Gizmos.DrawWireSphere (digPosRaw, 0.1f);
	}



}
