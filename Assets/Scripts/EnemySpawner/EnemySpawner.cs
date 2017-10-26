using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private static EnemySpawner _instance;
    public static EnemySpawner Instance { get { return _instance; } }
    public List<GameObject> spawners = new List<GameObject>();
    public int waves;
    public int totalEnemies;
    public int enemiesSpawned;
    public GameObject door;
	public bool allSpawnerDeads;
    public int enemiesAlive;
    public Worm wormPrefab;
    public FlyWorm flyWormPrefab;
    private Pool<Worm> wormPool;
    private Pool<FlyWorm> flyWormPool;

    int enemiesPerWave;
    int wave = 0;
	int currentWave;

    List<int> spawnPerVawe = new List<int>();

    void Awake()
    {
        _instance = this;
        wormPool = new Pool<Worm>(totalEnemies, WormFactory, wormPrefab.InitializePool, wormPrefab.DisposePool, true);
		flyWormPool = new Pool<FlyWorm>(totalEnemies/3, FlyWormFactory, flyWormPrefab.InitializePool, flyWormPrefab.DisposePool, true);
    }

    void Start()
    {
        Utility.KnuthShuffle<GameObject>(spawners);
        enemiesPerWave = totalEnemies / waves;

        for (int i = 0; i <= waves; i++)
        {
            if (i == 0)
            {
                enemiesPerWave = enemiesPerWave - (enemiesPerWave / 2);
            }

            else
            {
                enemiesPerWave = enemiesPerWave + (enemiesPerWave / 2);
            }

            spawnPerVawe.Add(enemiesPerWave);
        }	
        StartCoroutine(SpawnVawe(spawnPerVawe[wave]));
		wave++;
    }

    void Update()
    {
		if (allSpawnerDeads && enemiesAlive == 0 && door.activeSelf)
        {
            door.SetActive(false);
        }

        if (!allSpawnerDeads&&enemiesAlive == 0 && totalEnemies > 0 && wave < waves)
        {
            enemiesSpawned = 0;
			wave++;
			StartCoroutine(SpawnVawe(spawnPerVawe[wave]));
		
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
			if (spawners[enemiesSpawned].transform.position.y > 5)
				flyWormPool.GetPoolObject();
			else
				wormPool.GetObjectFromPool();

            enemiesAlive++;

            yield return new WaitForSeconds(0.3f);
        }
    }

    private Worm WormFactory()
    {
        return Instantiate<Worm>(wormPrefab);
    }
    private FlyWorm FlyWormFactory()
    {
        return Instantiate<FlyWorm>(flyWormPrefab);
    }

    public void ReturnWormToPool(Worm worm)
    {
        wormPool.DisablePoolObject(worm);
    }
    public void ReturnFlyWormToPool(FlyWorm flyWorm)
    {
        flyWormPool.DisablePoolObject(flyWorm);
    }
}
