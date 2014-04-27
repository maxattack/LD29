using UnityEngine;
using System.Collections;

public class RocketLauncher : Item {

	public Projectile rocket;
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
			Cmul( UnitVec(Random.Range(-0.1f * Mathf.PI, 0.1f * Mathf.PI) ), dir)
		);
		
		// SIDE-EFFECTS
		Jukebox.Play("ShootBazooka");
		Hero.inst.Kickback();
	}

	public override void OnPickUp() {
		particles.Play();
	}
	
	public override void OnDrop() {
		particles.Stop();
	}
	

}
