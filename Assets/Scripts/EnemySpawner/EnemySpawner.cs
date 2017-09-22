using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	private static EnemySpawner _instance;
	public static EnemySpawner Instance { get { return _instance; } }
	public List<GameObject> spawners = new List<GameObject>();
	public int Totalvawes;
	public int totalEnemies;
	public int enemiesSpawned;
	public GameObject door;
	public int enemiesAlive;
	public Enemy enemyPrefab;
	private Pool<Enemy> enemyPool;

	int enemiesPervawe;
	int vawe=0;
	List<int> spawnPerVawe = new List<int>();
	void Awake()
	{
		_instance = this;
		enemyPool = new Pool<Enemy>(totalEnemies/2, EnemyFactory, Enemy.InitializeEnemy, Enemy.DisposeEnemy, true);
	}
	
	void Start()
	{
		Utility.KnuthShuffle<GameObject>(spawners);
		enemiesPervawe = totalEnemies / Totalvawes;
		for (int i = 0; i <=Totalvawes; i++)
		{
			if (i == 0)
				enemiesPervawe = enemiesPervawe - (enemiesPervawe / 2);
			else
				enemiesPervawe = enemiesPervawe + (enemiesPervawe / 2);

			spawnPerVawe.Add(enemiesPervawe);
		}
		StartCoroutine(SpawnVawe(spawnPerVawe[vawe]));
	}
	void Update()
	{
		if (vawe == Totalvawes && enemiesAlive == 0)
			door.SetActive(false);

		if (enemiesAlive == 0 && totalEnemies>0)
		{
			enemiesSpawned = 0;
			vawe++;
			StartCoroutine(SpawnVawe(spawnPerVawe[vawe]));
		}
	}
	IEnumerator SpawnVawe(int cantEnemies)
	{
		for (int i = 0; i < cantEnemies; i++)
		{
			enemiesSpawned++;
			if (enemiesSpawned >= spawners.Count)
			{
				Utility.KnuthShuffle<GameObject>(spawners);
				enemiesSpawned = 0;
			}
			enemyPool.GetObjectFromPool();
			enemiesAlive++;
			yield return new WaitForSeconds(0.5f);
		}
	}
	private Enemy EnemyFactory()
	{
		return Instantiate<Enemy>(enemyPrefab);
	}

	public void ReturnBulletToPool(Enemy enemy)
	{
		enemyPool.DisablePoolObject(enemy);
	}
}
