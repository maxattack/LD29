using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Collider))]
public class BlastZone : CustomBehaviour {



	void OnTriggerEnter(Collider c) {
		if (c.IsHero()) {
			Hero.inst.RagdollKill(transform.position.xy());
		}

		if (c.GetComponent<LandMine> () != null) {

			c.GetComponent<LandMine>().Trip ();

				}

		if (c.GetComponent<Grenade> () != null) {

			Vector3 toGrenade = c.gameObject.transform.position - transform.position;
			toGrenade.Normalize();

			toGrenade.y += 0.1f;

			c.GetComponent<Grenade>().rigidbody.AddForce(toGrenade * 30,ForceMode.VelocityChange);
			
		}

	
	}

}
