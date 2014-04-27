using UnityEngine;
using System.Collections;

public class RocketLauncher : Item {

	public Transform rocket;
	
	public override void Operate(Vector2 dir) {
	
		Transform inst = Dup (rocket);
		
		//inst.transform.position = basePos;
		inst.GetComponent<Projectile>().initDir = new Vector3(dir.x,dir.y,0);
	}


}
