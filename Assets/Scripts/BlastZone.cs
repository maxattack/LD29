using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Collider))]
public class BlastZone : CustomBehaviour {

	void OnTriggerEnter(Collider c) {
		if (c.IsHero()) {
			Hero.inst.RagdollKill(transform.position.xy());
		}
	}

}
