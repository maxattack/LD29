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
	
		Jukebox.Play("ShootBazooka");
	
		// SPAWN ROCKET (TODO: POOL)
		rocket.Alloc(
			muzzle.position,  
			
			// DIRECTION RANDOMIZED A LITTLE
			Cmul(UnitVec(Random.Range(-0.1f * Mathf.PI, 0.1f * Mathf.PI)), dir)
			
		);
			
		// KICKBACK
		Hero.inst.Kickback();
	}

	public override void OnPickUp() {
		print("Pickup Bazooka");
		particles.Play();
	}
	
	public override void OnDrop() {
		particles.Stop();
	}
	

}
