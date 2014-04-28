using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Collider))]
public class BlastZone : CustomBehaviour {

	float killTimer = 0.1f;
	void Update()
	{
		killTimer -= Time.deltaTime;


		}

	void OnEnable()
	{
		killTimer = 0.1f;
		}

	void OnTriggerEnter(Collider c) 
	{
		if (killTimer > 0) {

						if (c.IsHero ()) {
								Hero.inst.RagdollKill (transform.position.xy ());
						}

						if (c.GetComponent<LandMine> () != null) {

								c.GetComponent<LandMine> ().Trip ();

						}

						if (c.GetComponent<Grenade> () != null) {

								Vector3 toGrenade = c.gameObject.transform.position - transform.position;
								toGrenade.Normalize ();

								toGrenade.y += 0.1f;

								c.GetComponent<Grenade> ().rigidbody.AddForce (toGrenade * 30, ForceMode.VelocityChange);
			
						}
						if (c.GetComponent<Dino> () != null) {

								c.GetComponent<Dino> ().Kill ();

						}
				}
	
	}

}
