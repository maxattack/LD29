using UnityEngine;
using System.Collections;

public class KillBar : MonoBehaviour {

	public static KillBar inst;
	int haltingSemaphore = 0;
	public float killSpeed = 1f;

	internal Transform xform;
	void Awake()
	{
				inst = this;
		xform = transform;
		}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		Vector3 p0 = xform.position;
		if (!Halted) {
				p0.y -= Time.deltaTime * killSpeed;
			}

		xform.position = p0;
	}

	//--------------------------------------------------------------------------------
	// Y-MOVE HALTING
	//--------------------------------------------------------------------------------
	
	public bool Halted { get { return haltingSemaphore > 0; } }
	
	public void Halt() {
		++haltingSemaphore;
	}
	
	public void Unhalt() {
		if (haltingSemaphore > 0) { --haltingSemaphore; }
	}


}
