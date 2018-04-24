using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private static EnemySpawner _instance;
    public static EnemySpawner Instance { get { return _instance; } }
    [Header("Needed Variables")]
    public List<GameObject> spawners = new List<GameObject>();
    public float timeBeforeReSpawn;
    public int maxEnemiesInScreen; // max enemies en pantalla incrementa con el tiempo
    public int totalEnemiesForPool; // for pool
    public int timeBeforeIncreaseTotalEnemies; // 1 per call
    public float radiusOfTheRoom;

    [Header("Prefabs")]
    public GameObject door;
    public Worm wormPrefab;
    public FlyWorm flyWormPrefab;
    public Pool<Worm> wormPool;
    private Pool<FlyWorm> flyWormPool;

    [Header("Data Variables")]
    public int currentSpawner;
    public int enemiesAlive;
    public int deadEnemies;
    public bool allSpawnerDeads;
    public bool whanderComplete;

    bool canUpdate;
    bool doorLocked = true;
    bool checkEnemiesSpawned;

    void Awake()
    {
        _instance = this;
        wormPool = new Pool<Worm>(totalEnemiesForPool, WormFactory, wormPrefab.InitializePool, wormPrefab.DisposePool, true);
        flyWormPool = new Pool<FlyWorm>(totalEnemiesForPool / 2, FlyWormFactory, flyWormPrefab.InitializePool, flyWormPrefab.DisposePool, true);
    }

    void SpawnFirstWave()
    {
        Utility.KnuthShuffle<GameObject>(spawners);
        StartCoroutine(SpawnVawe());
        StartCoroutine(IncreaseEnemies());
        StartCoroutine(ReSpawnEnemy());
        enemiesAlive = maxEnemiesInScreen;
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
            if (allSpawnerDeads && enemiesAlive <= 0 && door.activeSelf)
            {
                door.SetActive(false);
                doorLocked = false;
            }
            if (allSpawnerDeads && !checkEnemiesSpawned)
            {
                int enemiesInScreen = 0;
                var hits = Physics.OverlapSphere(transform.position, radiusOfTheRoom);
                foreach (var hit in hits)
                {
                    if (hit.gameObject.layer == 10)
                        enemiesInScreen++;
                }
                if (enemiesAlive < enemiesInScreen)
                {
                    int missingEnemies = enemiesInScreen - enemiesAlive;
                    SpawnMissingEnemies(missingEnemies);
                }
                checkEnemiesSpawned = true;
                print(enemiesAlive);
                print(enemiesInScreen);
                print(enemiesAlive);
            }
        }
    }
    void SpawnMissingEnemies(int missingEnemies)
    {
        for (int i = 0; i < missingEnemies; i++)
        {
            currentSpawner++;
            enemiesAlive++;
            if (currentSpawner >= spawners.Count)
            {
                Utility.KnuthShuffle<GameObject>(spawners);
                currentSpawner = 0;
            }
            if (spawners[currentSpawner].transform.position.y > 5)
                flyWormPool.GetPoolObject();
            else
                wormPool.GetObjectFromPool();

        }
    }

    IEnumerator SpawnVawe()
    {
        for (int i = 0; i < maxEnemiesInScreen; i++)
        {
            currentSpawner++;

            if (currentSpawner >= spawners.Count)
            {
                Utility.KnuthShuffle<GameObject>(spawners);
                currentSpawner = 0;
            }
            if (spawners[currentSpawner].transform.position.y > 5)
                flyWormPool.GetPoolObject();
            else
                wormPool.GetObjectFromPool();

            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator ReSpawnEnemy()
    {
        do
        {
            if (allSpawnerDeads)
            {
                yield break; // deja de spawnear enemigos cuando los spawners estan muertos
            }
            else if (deadEnemies > 0)
            {
                currentSpawner++;

                if (currentSpawner >= spawners.Count)
                {
                    Utility.KnuthShuffle<GameObject>(spawners);
                    currentSpawner = 0;
                }
                if (spawners[currentSpawner].transform.position.y > 5)
                    flyWormPool.GetPoolObject();
                else
                    wormPool.GetObjectFromPool();

                deadEnemies--;
                enemiesAlive++;

                yield return new WaitForSeconds(timeBeforeReSpawn);
            }
            else
            {
                yield return null;
            }
        } while (doorLocked);
    }

    IEnumerator IncreaseEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBeforeIncreaseTotalEnemies);
            deadEnemies++;  // increase dead enemies so it will spawn another enemy
            if (timeBeforeReSpawn > 0)
                timeBeforeReSpawn -= 0.05f; // menos tiempo de re spawn more difficult
        }
    }
    public void CreateNewGroundEnemy(Vector3 newPos)
    {
        currentSpawner++;

        if (currentSpawner >= spawners.Count)
        {
            Utility.KnuthShuffle<GameObject>(spawners);
            currentSpawner = 0;
        }
        Vector3 prevPos = spawners[currentSpawner].transform.position;
        spawners[currentSpawner].transform.position = newPos;
        wormPool.GetObjectFromPool();
        spawners[currentSpawner].transform.position = prevPos;
    }
    // POOLS 
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
