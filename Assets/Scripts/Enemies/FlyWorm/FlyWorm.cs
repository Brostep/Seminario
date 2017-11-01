using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyWorm : Enemy {

	EnemySpawner enemySpawner;
	GameManager gm;
	public float fireRate;
	public float damage;
	private void Start()
	{
		StartCoroutine(Shoot());
	}
	override public void Initialize()
	{
		enemySpawner = FindObjectOfType<EnemySpawner>();
		gm = FindObjectOfType<GameManager>();
		life = gm.flyWormLife;
		var spawners = FindObjectOfType<EnemySpawner>().spawners;
		var index = FindObjectOfType<EnemySpawner>().enemiesSpawned;
		transform.position = spawners[index].transform.position;
	}
	void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.layer == 9)
			life -= c.gameObject.GetComponent<PlayerBullets>().damage;
		//player
		if (c.gameObject.layer == 8)
			c.gameObject.GetComponent<PlayerController>().life -= damage;
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
	IEnumerator Shoot()
	{
		while (true)
		{
			gm.FlyWormBulletPool.GetPoolObject();

			var bullets = FindObjectsOfType<FlyWormBullets>();
			for (int i = 0; i < bullets.Length; i++)
			{
				if (!bullets[i].placed)
				{
					bullets[i].setTransform(transform.position, transform.rotation);
					bullets[i].placed = true;
				}
			}
			yield return new WaitForSeconds(fireRate);
		}
	}
}
