using UnityEngine;
using System.Collections;

public class GrenadeLauncher : Item {

	public Grenade grenade;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	

	}



	public override void Operate(Vector2 dir) {
		
		// SPAWN grenade W/ KINDA RANDOMIZED HEADING
		Grenade inst = grenade.Alloc (
			fx.GetChild(0).transform.position) as Grenade;
			//Cmul( UnitVec(Random.Range(-0.1f * Mathf.PI, 0.1f * Mathf.PI) ), dir)
			//);
		inst.SetInitDir(Cmul (UnitVec (Random.Range (-0.05f * Mathf.PI, 0.05f * Mathf.PI)), dir));
		
		// SIDE-EFFECTS
		Jukebox.Play("ShootBazooka");
		Hero.inst.Kickback();
	}
}
