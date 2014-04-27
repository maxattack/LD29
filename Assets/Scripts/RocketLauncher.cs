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
	
		Dup(rocket, muzzle.position).initDir = 
			QDegrees(Random.Range(-10f, 10f)) * Vec(dir.x,dir.y,0);
	}

	public override void OnPickUp() {
		print("Pickup Bazooka");
		particles.Play();
	}
	
	public override void OnDrop() {
		particles.Stop();
	}
	

}
