using UnityEngine;
using System.Collections;

public class Item : CustomBehaviour {
	
	Item prefab;
	Item next;




	public bool IsPrefab { 
		get { return prefab == null; } 
	}
	
	
	public Item Alloc() {
		Assert(IsPrefab);
		
		Item result;
		
		if (next != null) {
			
			// RECYCLE INSTANCE
			result = next;
			next = result.next;
			result.next = null;
			result.gameObject.SetActive(true);
			
		} else {
			
			// CREATE NEW INSTANCE
			result = Dup(this);
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


	internal Transform xform;
	// Use this for initialization
	void Start () {
	
		xform = transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	
	}


	enum ItemType
	{
		Shovel,
		RocketLauncher,
	}
	ItemType itemType = ItemType.Shovel;

	internal void Operate(Vector2 dir)
	{
		if (itemType == ItemType.Shovel) {

		

			WorldGen.inst.DigShovel((int)(xform.position.x + dir.x),(int)(xform.position.y + dir.y));

				}
		if (itemType == ItemType.RocketLauncher) {
			
			WorldGen.inst.DigRocket((int)xform.position.x,(int)xform.position.y);
			
		}

	}


}
