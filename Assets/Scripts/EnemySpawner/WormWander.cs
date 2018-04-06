using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormWander : Enemy {

	GameManager gm;
	TrailRenderer _tr;
	Animator anim;
	ParticleSystem deathParticles;
	FlockingWander flocking;
	PlayerController player;
	float velocityLimit;
	public float damage;

	private void Start()
	{
		gm = FindObjectOfType<GameManager>();
		player = FindObjectOfType<PlayerController>();
		anim = GetComponent<Animator>();
		deathParticles = GetComponentInChildren<ParticleSystem>();
		flocking = GetComponent<FlockingWander>();
		velocityLimit = 4.5f;

	}
	void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.layer == 9)
		{
            Instantiate(gm.bloodWorm, head.transform);
            
			life -= c.gameObject.GetComponent<PlayerBullets>().damage;
		}

	}
	private void Update()
	{
		if (life <= 0)
		{
			anim.SetBool("OnDeath", true);

			if (!deathParticles.isPlaying)
				deathParticles.Play();
		}
		var distance = player.transform.position - transform.position;
		if (distance.magnitude < 1.3f)
		{
			transform.LookAt(player.transform);
			flocking.velocityLimit = 6f;
			GetComponent<BoxCollider>().center = new Vector3(0f, 0.4f, -0.3f);
			anim.SetBool("OnMeleeAttack", true);
			flocking.attacking = true;
		}
	}

	public void EndDeath()
	{
		gm.enemiesDead++;
		anim.SetBool("OnDeath", false);
		this.gameObject.SetActive(false);
		deathParticles.Stop();
	}

	void OnMeleeAttack()
	{
	//	var headPos = head.transform.position + new Vector3(0f, 0f, 1f);
		var enemiesHited = Physics.OverlapBox(head.transform.position, new Vector3(0.5f, 0.5f, 1f), transform.rotation, LayerMask.GetMask("Player"));
		if (enemiesHited.Length > 0)
		{
			foreach (var enemy in enemiesHited)
			{
				var e = enemy.GetComponent<PlayerController>();
				e.TakeDamage(damage);
			}
		}
		flocking.velocityLimit = velocityLimit;
		anim.SetBool("OnMeleeAttack", false);
		GetComponent<BoxCollider>().center = new Vector3(0f, 0.4f, 0f);
		flocking.attacking = false;
	}

}
