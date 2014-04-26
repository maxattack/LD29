using UnityEngine;
using System.Collections;

public class Item : CustomBehaviour {
	
	Item prefab;
	Item next;

	public Transform rocket;


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
	Vector2 dir;
	internal void SetDir(Vector2 d)
	{
		dir = d;
	
		xform.localRotation = Quaternion.FromToRotation (new Vector3 (1, 0, 0), new Vector3 (dir.x, dir.y, 0));



	}

	Vector3 basePos;

	internal void SetOperatePos(Vector3 p)
	{
		basePos = p;

	
	}


	internal Transform xform;
	// Use this for initialization
	void Start () {
	

		GetComponent<SpriteRenderer> ().sprite = sprites [(int)itemType];

		xform = transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
		if (this == Hero.inst.currItem) {
			//			xform.localPosition = xform.localPosition.EaseTowards (Vector3.zero, 0.1f); 
				}
	
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.white;

		Vector3 digPos = new Vector3 ((int)(xform.position.x + dir.x + 0.5f), (int)(xform.position.y + dir.y + 0.5f), 0);
		Gizmos.DrawWireCube(digPos, new Vector3(1.0f,1.0f,1.0f));

		Vector3 digPosRaw = new Vector3 ((xform.position.x + dir.x), (xform.position.y + dir.y), 0);
		Gizmos.DrawWireSphere (digPosRaw, 0.1f);
	}


	internal enum ItemType
	{
		Shovel,
		RocketLauncher,
		NumItemTypes
	}
	public Sprite [] sprites = new Sprite[(int)ItemType.NumItemTypes];
	internal ItemType itemType = ItemType.Shovel;

	internal void Operate(Vector2 dir)
	{
		if (itemType == ItemType.Shovel) {

			xform.localPosition = 1 * xform.right;

			WorldGen.inst.DigShovel((int)(basePos.x + dir.x + 0.5f),(int)(basePos.y + dir.y + 0.5f));

				}
		if (itemType == ItemType.RocketLauncher) {


			Transform inst = Dup (rocket);
			
			inst.transform.position = basePos;
			inst.GetComponent<Projectile>().initDir = new Vector3(dir.x,dir.y,0);
			
		}

	}


}
