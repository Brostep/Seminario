using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public int life = 2;
	public float speed;
	EnemySpawner enemySpawner;
	GameObject player;
	private void Start()
	{
		enemySpawner = FindObjectOfType<EnemySpawner>();
		player = FindObjectOfType<PlayerController>().gameObject;
	}
	private void Update()
	{

		var lookPos = player.transform.position - transform.position;
		lookPos.y = 0;
		var rotation = Quaternion.LookRotation(lookPos);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 20);

		transform.position += transform.forward * speed * Time.deltaTime;
	}
	public void Initialize()
	{
		var spawners = FindObjectOfType<EnemySpawner>().spawners;
		var index = FindObjectOfType<EnemySpawner>().enemiesSpawned;
		transform.position = spawners[index].transform.position;
	}
	public static void InitializeEnemy(Enemy enemyObj)
	{
		enemyObj.gameObject.SetActive(true);
		enemyObj.Initialize();
	}
	void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.layer == 9)
		{
			life--;
			if (life <= 0)
			{
				enemySpawner.totalEnemies--;
				enemySpawner.enemiesAlive--;
				EnemySpawner.Instance.ReturnBulletToPool(this);
			}	
		}
	}

	public static void DisposeEnemy(Enemy enemyObj)
	{
		enemyObj.gameObject.SetActive(false);
	}
}
