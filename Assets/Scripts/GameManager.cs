using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject activateEnemySpawners;
	public GameObject spawnerObj;
	public GameObject bloodWorm;

	public int cantSpawners;
	PlayerController player;
	public List<GameObject> spawners;
	public List<WormWander> enemiesOnStart;

	public float wormLife;
	public float flyWormLife;
	public Vector3 offsetSpawner;
	public Pool<FlyWormBullets> FlyWormBulletPool;
	public FlyWormBullets FlyWormBulletPrefab;

	[HideInInspector]
	public EnemySpawner enemySpawners;
	[HideInInspector]
	public int enemiesDead;
	[HideInInspector]
	public int spawnersAlive;

	bool activated;
	[HideInInspector]
	public int spawn = 1;
    public int currentSpawn;
	private static GameManager _instance;
	public static GameManager Instance { get { return _instance; } }

	private void Awake()
	{
		if(_instance == null)
			_instance = this;
		FlyWormBulletPool = new Pool<FlyWormBullets>(30, BulletFactory, FlyWormBulletPrefab.InitializePool, FlyWormBulletPrefab.DisposePool, false);
	}
	private void Start()
	{
		player = FindObjectOfType<PlayerController>();
		enemySpawners = activateEnemySpawners.GetComponent<EnemySpawner>();
		spawnersAlive = cantSpawners;
		Utility.KnuthShuffle<GameObject>(spawners);
	}
	private void Update()
	{
		if (!activated && enemiesDead >= enemiesOnStart.Count)
		{
			activateEnemySpawners.SetActive(true);
			activated = true;
			for (int i = 0; i < cantSpawners; i++)
			{
				Instantiate(spawnerObj, spawners[i].transform.position - offsetSpawner, Quaternion.identity);
				spawners[i].GetComponent<Spawner>().open = false;
			}
			//make transition
		//	PlayerController.inTopDown = !PlayerController.inTopDown;
		//	player.cameraChange = true;
		}
		if ((cantSpawners + spawn) == spawners.Count)
		{
			spawn = 0;
			Utility.KnuthShuffle<GameObject>(spawners);
		}
		if (spawnersAlive <= 0)
		{
			enemySpawners.allSpawnerDeads = true;
		}
	}
	private FlyWormBullets BulletFactory()
	{
		return Instantiate<FlyWormBullets>(FlyWormBulletPrefab);
	}

	public void ReturnBulletToPool(FlyWormBullets bullet)
	{
		FlyWormBulletPool.DisablePoolObject(bullet);
	}
}
