using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : Enemy{

	EnemySpawner enemySpawner;
	GameManager gameManager;

	public override void Initialize()
	{
		enemySpawner = FindObjectOfType<EnemySpawner>();
		gameManager = FindObjectOfType<GameManager>();
		var spawners = enemySpawner.spawners;
		var index = enemySpawner.enemiesSpawned;
		transform.position = spawners[index].transform.position;
		life = gameManager.wormLife;
	}
	void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.layer == 9)
			life -= c.gameObject.GetComponent<PlayerBullets>().damage;
	}
	void Update()
	{
		if (life <= 0)
		{
			enemySpawner.totalEnemies--;
			enemySpawner.enemiesAlive--;
			EnemySpawner.Instance.ReturnWormToPool(this);
		}
	}
	public Worm Factory(Worm obj)
	{
		return Instantiate<Worm>(obj);
	}
}
