using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : Enemy
{

	EnemySpawner enemySpawner;
	GameManager gameManager;
	GameObject player;
	GameObject playerHead;
	Animator anim;
	ParticleSystem deathParticles;
	Flocking flocking;
	BossController bossController;
	bool canAttack;
	float velocityLimit;
	public float cdAttack;
	public float leapDistance;
	float _cdAttack;
	float _failCdAttack;
	public float leapForce;
	[Range(0f, 100f)]
	public float leapProbability = 0.65f;
	public float damage;
	public float meleeRadius;
	public float boop; 
    Renderer matDissolve;
    float disolveValue = 1;
    bool isDeath;
	bool wormFromBoss;
	bool alreadyInitialized;

	private void Start()
	{
		if (!alreadyInitialized)
		{
			enemySpawner = FindObjectOfType<EnemySpawner>();
			gameManager = FindObjectOfType<GameManager>();
			player = FindObjectOfType<PlayerController>().gameObject;
			flocking = GetComponent<Flocking>();
			velocityLimit = flocking.velocityLimit;
			playerHead = player.GetComponentInChildren<Head>().gameObject;
			life = gameManager.wormLife;
			anim = GetComponent<Animator>();
			deathParticles = GetComponentInChildren<ParticleSystem>();
			bossController = FindObjectOfType<BossController>();
			disolveValue = 1;
			isDeath = false;
			matDissolve = GetComponentInChildren<Renderer>();
			matDissolve.material.SetFloat("_Dissolved", disolveValue);
			flocking.attacking = false;
			wormFromBoss = true;
		}
		
	}
	public override void Initialize()
	{
		enemySpawner = FindObjectOfType<EnemySpawner>();
		gameManager = FindObjectOfType<GameManager>();
		player = FindObjectOfType<PlayerController>().gameObject;
		flocking = GetComponent<Flocking>();
		velocityLimit = flocking.velocityLimit;
		playerHead = player.GetComponentInChildren<Head>().gameObject;
		var spawners = enemySpawner.spawners;
		var index = enemySpawner.enemiesSpawned;
		transform.position = spawners[index].transform.position;
		life = gameManager.wormLife;
		anim = GetComponent<Animator>();
		deathParticles = GetComponentInChildren<ParticleSystem>();
		bossController = FindObjectOfType<BossController>();
		disolveValue = 1;
		isDeath = false;
		matDissolve = GetComponentInChildren<Renderer>();
		matDissolve.material.SetFloat("_Dissolved", disolveValue);
		flocking.attacking = false;
		alreadyInitialized = true;
	}
	void OnCollisionEnter(Collision c)
	{
		//player bullet
		if (c.gameObject.layer == 9)
		{
			life -= c.gameObject.GetComponent<PlayerBullets>().damage;
			Instantiate(gameManager.bloodWorm, head.transform);
		}
		if (c.gameObject.layer == 8 && anim.GetBool("OnCharge"))
			c.gameObject.GetComponent<PlayerController>().TakeDamage(damage*2);
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
			if (wormFromBoss)
				bossController.wormsInScene.Remove(gameObject);
			anim.SetBool("OnDeath", true);
            isDeath = true;
            flocking.attacking = true;
            if (!deathParticles.isPlaying)
            {
                deathParticles.Play();
            }		
		}
        if (isDeath)
        {
            matDissolve = GetComponentInChildren<Renderer>();
            disolveValue = Mathf.Lerp(disolveValue, 0, Time.deltaTime*0.33f);
            matDissolve.material.SetFloat("_Dissolved", disolveValue);
			if (disolveValue <= 0.45f)
                if (bossController.fase2Enabled)
			        Destroy(this.gameObject);
				else
				{
					EnemySpawner.Instance.ReturnWormToPool(this);	
				}

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
			if (chance < (leapProbability / 100f))
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
		if (distance.magnitude < 4f)
		{
			transform.LookAt(player.transform);
			flocking.velocityLimit = 2.5f;
			GetComponent<CapsuleCollider>().center = new Vector3(0f, 0.4f, -0.3f);
			anim.SetBool("OnMeleeAttack", true);
			canAttack = false;
			flocking.attacking = true;
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
	void EndCharge()
	{
		anim.SetBool("OnCharge", false);

		var distance = playerHead.transform.position - transform.position;
		var direction = distance.normalized;
		GetComponent<Rigidbody>().AddForce((direction * leapForce * (distance.magnitude / 2)));

	}
	void EndDeath()
	{
		enemySpawner.totalEnemies--;
		enemySpawner.enemiesAlive--;
		anim.SetBool("OnDeath", false);
		deathParticles.Stop();
	}
	void OnMeleeAttack()
	{
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
		canAttack = true;
		anim.SetBool("OnMeleeAttack", false);
		GetComponent<CapsuleCollider>().center = new Vector3(0f, 0.4f, 0f);
		flocking.attacking = false;
	}

}
