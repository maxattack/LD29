using UnityEngine;
using System.Collections;


public class Debris : CustomBehaviour {

	Debris prefab;
	Debris next;

	public bool IsPrefab { 
		get { return prefab == null; } 
	}


	public Debris Alloc() {
		Assert(IsPrefab);
		
		Debris result;
		
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

		rigidbody.velocity = new Vector3 (0, 0, 0);
		rigidbody.angularVelocity = new Vector3 (0, 0, 0);

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



	// Use this for initialization
	void Start () {
	
	}
	float totalTime = 0;
	// Update is called once per frame
	void Update () {

		totalTime += Time.deltaTime;

		if (totalTime > 5) {
			Release();
				}
	
	}
}
