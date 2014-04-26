using UnityEngine;
using System.Collections;

public class PoolingSample : CustomBehaviour {
	
	// AN EXAMPLE OF HOW TO DO ONE-SHOT POOLING.  NOTE THAT BECAUSE THE OBJECT
	// IS DEACTIVATED RATHER THAN DESTROYED, THE EVENT-HANDLING SEMANTICS ARE 
	// A LITTLE DIFFERENT (Awake-[OnEnable-OnDisable]*-OnDestroy
	
	PoolingSample prefab;
	PoolingSample next;
	
	public bool IsPrefab { 
		get { return prefab == null; } 
	}
	
	public PoolingSample Alloc(Vector3 position) {
		Assert(IsPrefab);
		
		PoolingSample result;
		
		if (next != null) {
			
			// RECYCLE INSTANCE
			result = next;
			next = result.next;
			result.next = null;
			result.transform.position = position;
			result.SetActive(true);
			
		} else {
		
			// CREATE NEW INSTANCE
			result = Dup(this, position);
			result.prefab = this;
			
		}
		
		// RE-INIT INSTANCE
		result.Init();
		return result;
	}
	
	void Init() {
	
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
	
}
