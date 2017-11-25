using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormWander : Enemy {

	GameManager gm;
	TrailRenderer _tr;
	Animator anim;
	ParticleSystem deathParticles;
	public float damage;

	private void Start()
	{
		gm = FindObjectOfType<GameManager>();
		anim = GetComponent<Animator>();
		deathParticles = GetComponentInChildren<ParticleSystem>();

		//Debug.Log(_tr.positionCount);

	}
	void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.layer == 9)
		{
			Instantiate(gm.bloodWorm, head.transform);
			life -= c.gameObject.GetComponent<PlayerBullets>().damage;
		}
		
		//player
		if (c.gameObject.layer == 8)
			c.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
	}
	private void Update()
	{
		if (life <= 0)
		{
			anim.SetBool("OnDeath", true);

			if (!deathParticles.isPlaying)
				deathParticles.Play();
		}
	}

	public void EndDeath()
	{
		gm.enemiesDead++;
		anim.SetBool("OnDeath", false);
		this.gameObject.SetActive(false);
		deathParticles.Stop();
	}
}
