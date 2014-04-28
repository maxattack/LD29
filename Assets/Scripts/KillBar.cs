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

		Vector3 p = CameraFX.inst.xform.position;

		SpriteRenderer warning = GetComponentInChildren<SpriteRenderer> ();

		
		if (p.y + CameraFX.inst.HalfHeight + 1.5f > xform.position.y) {
						warning.transform.localScale = warning.transform.localScale.EaseTowards (new Vector3 (1, 1, 1), 0.2f);
						flashTimer += Time.deltaTime;
						if (flashTimer > 1) {
								Jukebox.Play ("warning");

								warning.color = new Color (1, 1, 1, 1);
								flashTimer = 0;
						}

		
				} else {
						warning.transform.localScale = new Vector3 (1, 0, 1);
				}
		warning.color = warning.color.EaseTowards (new Color (1, 1, 1, 0), 0.2f);
		
		warning.transform.position = new Vector3(p.x, p.y + CameraFX.inst.HalfHeight * 0.9f,0);
	}

	float flashTimer = 0;

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
