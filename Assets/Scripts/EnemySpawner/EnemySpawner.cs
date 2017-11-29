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
	public int enemiesAlive;
	public GameObject door;
	public bool allSpawnerDeads;
	public bool whanderComplete;
	bool canUpdate;

    public Worm wormPrefab;
    public FlyWorm flyWormPrefab;
	public Pool<Worm> wormPool;
    private Pool<FlyWorm> flyWormPool;

    int enemiesPerWave;
    int wave = 0;

    List<int> spawnPerVawe = new List<int>();

    void Awake()
    {
        _instance = this;
        wormPool = new Pool<Worm>(totalEnemies, WormFactory, wormPrefab.InitializePool, wormPrefab.DisposePool, true);
		flyWormPool = new Pool<FlyWorm>(totalEnemies/3, FlyWormFactory, flyWormPrefab.InitializePool, flyWormPrefab.DisposePool, true);
    }

    void SpawnFirstWave()
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
		if (whanderComplete)
		{
			SpawnFirstWave();
			canUpdate = true;
			whanderComplete = false;
		}
		if (canUpdate)
		{
			if (allSpawnerDeads && enemiesAlive == 0 && door.activeSelf)
			{
				door.SetActive(false);
			}

			if (!allSpawnerDeads && enemiesAlive == 0 && totalEnemies > 0 && wave < waves)
			{
				enemiesSpawned = 0;
				wave++;
				StartCoroutine(SpawnVawe(spawnPerVawe[wave]));

			}
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

            yield return new WaitForSeconds(1f);
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
