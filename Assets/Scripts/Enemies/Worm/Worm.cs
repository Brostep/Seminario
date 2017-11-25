using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : Enemy{

	EnemySpawner enemySpawner;
	GameManager gameManager;
	GameObject player;
	GameObject playerHead;
	Animator anim;
	ParticleSystem deathParticles;
	bool canAttack;
	public float cdAttack;
	public float leapDistance;
	float _cdAttack;
	float _failCdAttack;
	public float leapForce;
	[Range(0f,100f)]
	public float leapProbability = 0.65f;
	public float damage;

	public override void Initialize()
	{
		enemySpawner = FindObjectOfType<EnemySpawner>();
		gameManager = FindObjectOfType<GameManager>();
		player = FindObjectOfType<PlayerController>().gameObject;
		playerHead = player.GetComponentInChildren<Head>().gameObject;
		var spawners = enemySpawner.spawners;
		var index = enemySpawner.enemiesSpawned;
		transform.position = spawners[index].transform.position;
		life = gameManager.wormLife;
		anim = GetComponent<Animator>();
		deathParticles = GetComponentInChildren<ParticleSystem>();
	}
	void OnCollisionEnter(Collision c)
	{
		//player bullet
		if (c.gameObject.layer == 9)
		{
			life -= c.gameObject.GetComponent<PlayerBullets>().damage;
			Instantiate(gameManager.bloodWorm, head.transform);
		}
		
		//player
		if (c.gameObject.layer == 8)
			c.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
	}
	void Update()
	{
		_cdAttack += Time.deltaTime;
		if (_cdAttack >= cdAttack && !canAttack)
			canAttack = true;

		if (canAttack)
			checkDistanceToPlayer();

		if (life <= 0)
		{
			anim.SetBool("OnDeath", true);

			if (!deathParticles.isPlaying)
				deathParticles.Play();
		}

		//Agrego esto por si el spawn del gusano es una distancia donde no encuentra al player.

		if (player == null) 
		{
			player = FindObjectOfType<PlayerController>().gameObject;
			playerHead = player.GetComponentInChildren<Head>().gameObject;
		}
	}
	void checkDistanceToPlayer()
	{
		var distance = player.transform.position - transform.position;
		
		if (distance.magnitude < leapDistance)
		{
			var chance = Utility.random.NextDouble();
			if (chance < (leapProbability/100f))
			{
				_cdAttack = 0f;
				canAttack = false;
			}
			else
			{
				_cdAttack = 2f;
				canAttack = false;
				leapAttack();
			}
		}
	}

	void leapAttack()
	{
        anim.SetBool("OnCharge", true);
	}

	public Worm Factory(Worm obj)
	{
		return Instantiate<Worm>(obj);
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, leapDistance);
	}

    public void EndCharge()
    {
        anim.SetBool("OnCharge", false);

        var distance = playerHead.transform.position - transform.position;
        var direction = distance.normalized;
        GetComponent<Rigidbody>().AddForce((direction * leapForce * (distance.magnitude / 2)));

    }


    public void EndDeath() 
	{
		Debug.Log ("DIE WORM");
		enemySpawner.totalEnemies--;
		enemySpawner.enemiesAlive--;
		anim.SetBool("OnDeath", false);
		deathParticles.Stop();
		EnemySpawner.Instance.ReturnWormToPool(this);

	}
}
