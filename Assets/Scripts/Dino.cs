using UnityEngine;
using System;
using System.Collections;

public class Dino : PooledObject {

	public Sprite run;
	public Sprite die;
	public Sprite stand;

	Transform xform;
	Rigidbody body;


	
	void Awake() {
		xform = this.transform;
		body = this.rigidbody;

		
	}

	public float runSpeed;
	float targetRunningSpeed = 0f;

	float frameTimer = 0;
	void FixedUpdate()
	{
		// RUNNING
		var vel = body.velocity;
		var easing = 0.2f;


		Vector3 toHero = Vector3.zero; 
		if (!dead)
		{
			float dist = Vector3.SqrMagnitude(Hero.inst.xform.position - xform.position);
			if(dist < 25 && Math.Abs( Hero.inst.xform.position.y - xform.position.y) < 7)
			{

				toHero = Hero.inst.xform.position - xform.position;
			}

		}

	
		Vector2 s = xform.localScale;
		if(toHero.x < 0) {
			s.x = 1;

			targetRunningSpeed = targetRunningSpeed.EaseTowards(-runSpeed, easing);
		} else if(toHero.x > 0) {
			s.x = -1;
			targetRunningSpeed = targetRunningSpeed.EaseTowards(runSpeed, easing);
		} else {
			targetRunningSpeed = targetRunningSpeed.EaseTowards(0, easing);
		}
		xform.localScale = s;
		body.AddForce(Vec(targetRunningSpeed - vel.x, 0, 0), ForceMode.VelocityChange);



	}

	bool dead = false;
	internal bool IsDead { get { return dead; } }

	internal void Kill()
	{
		dead = true;
		this.GetComponent<SpriteRenderer> ().sprite = die;
	}

	// Update is called once per frame
	void Update () {
		if (!dead)
		{
			if(targetRunningSpeed != 0)
			{
				frameTimer += Time.deltaTime;

				if(frameTimer > 0.1f)
				{
					if(this.GetComponent<SpriteRenderer> ().sprite == stand)
					{
						this.GetComponent<SpriteRenderer> ().sprite = run;
					}
					else
					{
						this.GetComponent<SpriteRenderer> ().sprite = stand;
					}
					frameTimer = 0;
				}
			}
			else
			{
				this.GetComponent<SpriteRenderer> ().sprite = stand; 
			}
			
			
			
		}


	}




}
