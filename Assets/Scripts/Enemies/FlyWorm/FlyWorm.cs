using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyWorm : Enemy {

	float _life;
	EnemySpawner enemySpawner;

	private void Start()
	{
		enemySpawner = FindObjectOfType<EnemySpawner>();
	}
	override public void Initialize()
	{
		_life = life;
		var spawners = FindObjectOfType<EnemySpawner>().spawners;
		var index = FindObjectOfType<EnemySpawner>().enemiesSpawned;
		transform.position = spawners[index].transform.position;
	}
	void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.layer == 9)
			life -= c.gameObject.GetComponent<PlayerBullets>().damage;
	}
	void Update()
	{
		CheckLife();
		MoveCollider();
	}

	void CheckLife()
	{
		if (life <= 0)
		{
			enemySpawner.totalEnemies--;
			enemySpawner.enemiesAlive--;
			EnemySpawner.Instance.ReturnFlyWormToPool(this);
		}
	}

	void MoveCollider()
	{
		if (PlayerController.inTopDown && transform.position.y > 5f)
		{
			GetComponent<CapsuleCollider>().center = new Vector3(0f, -4.8f, 0f);
		}
		else if (!PlayerController.inTopDown && transform.position.y > 5f)
		{
			GetComponent<CapsuleCollider>().center = new Vector3(0f, 0f, 0f);
		}
	}
}
