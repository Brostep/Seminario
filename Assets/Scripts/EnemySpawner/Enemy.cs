using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public float life = 3;
	EnemySpawner enemySpawner;
	private void Start()
	{
		enemySpawner = FindObjectOfType<EnemySpawner>();
	}
	public void Initialize()
	{
		var spawners = FindObjectOfType<EnemySpawner>().spawners;
		var index = FindObjectOfType<EnemySpawner>().enemiesSpawned;
		transform.position = spawners[index].transform.position;
		life = 3;
	}
	public static void InitializeEnemy(Enemy enemyObj)
	{
		enemyObj.gameObject.SetActive(true);
		enemyObj.Initialize();
	}
	void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.layer == 9)
			life-=c.gameObject.GetComponent<Bullet>().damage;
	}
	void Update()
	{
		if (life <= 0)
		{
			enemySpawner.totalEnemies--;
			enemySpawner.enemiesAlive--;
			EnemySpawner.Instance.ReturnBulletToPool(this);
		}
	}

	public static void DisposeEnemy(Enemy enemyObj)
	{
		enemyObj.gameObject.SetActive(false);
	}
}
