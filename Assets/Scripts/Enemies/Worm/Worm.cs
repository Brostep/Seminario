using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : Enemy {
	float _life;
	EnemySpawner enemySpawner;
	private void Start()
	{
		enemySpawner = FindObjectOfType<EnemySpawner>();
		_life = life;
	}
	override public void Initialize()
	{
		var spawners = FindObjectOfType<EnemySpawner>().spawners;
		var index = FindObjectOfType<EnemySpawner>().enemiesSpawned;
		transform.position = spawners[index].transform.position;
		life = 5;
	}
	public static void InitializeEnemy(Enemy enemyObj)
	{
		enemyObj.gameObject.SetActive(true);
		enemyObj.Initialize();
	}
	void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.layer == 9)
			life -= c.gameObject.GetComponent<Bullet>().damage;
	}
	void Update()
	{
		if (life <= 0)
		{
			enemySpawner.totalEnemies--;
			enemySpawner.enemiesAlive--;
			EnemySpawner.Instance.ReturnBulletToPool(this);
		}

		if (PlayerController.inTopDown && transform.position.y > 5f)
		{
			GetComponent<CapsuleCollider>().center = new Vector3(0f, -4.5f, 0f);
		}
		else if (!PlayerController.inTopDown && transform.position.y > 5f)
		{
			GetComponent<CapsuleCollider>().center = new Vector3(0f, 0f, 0f);
		}
	}

	public static void DisposeEnemy(Enemy enemyObj)
	{
		enemyObj.gameObject.SetActive(false);
	}
}
