using UnityEngine;
using System.Collections;

public class RocketLauncher : Item {

	public Rocket rocket;
	ParticleSystem particles;
	Transform muzzle;
	
	void Awake() {
		particles = GetComponentInChildren<ParticleSystem>();
		muzzle = particles.transform;
	}
	
	public override void Operate(Vector2 dir) {
	
		// SPAWN ROCKET W/ KINDA RANDOMIZED HEADING
		rocket.Alloc(
			muzzle.position,  
			Cmul( UnitVec(Random.Range(-0.025f * Mathf.PI, 0.125f * Mathf.PI) ), dir)
		);
		
		// SIDE-EFFECTS
		Jukebox.Play("ShootBazooka");
		dir *= -1;
		if (dir.y == 0) //hack to combat friction
				dir.y = 0.01f;
		Hero.inst.Kickback(dir * 20);

	}

	public override void OnPickUp() {
		particles.Play();
	}
	
	public override void OnDrop() {
		particles.Stop();
	}
	

}
