using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : Enemy{

	EnemySpawner enemySpawner;
	GameManager gameManager;
	GameObject player;
	GameObject playerHead;
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
			c.gameObject.GetComponent<PlayerController>().life -= damage;
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
			enemySpawner.totalEnemies--;
			enemySpawner.enemiesAlive--;
			EnemySpawner.Instance.ReturnWormToPool(this);
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
		var distance = playerHead.transform.position - transform.position;
		var direction = distance.normalized;
		GetComponent<Rigidbody>().AddForce((direction * leapForce * (distance.magnitude/2)));
	}

	public Worm Factory(Worm obj)
	{
		return Instantiate<Worm>(obj);
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, leapDistance);
	}
}
