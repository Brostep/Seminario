using System.Collections.Generic;
using UnityEngine;

public class BulletHellSpawner : MonoBehaviour
{
    public enum PatternStrategies
    {
        Pattern1,
        Pattern2,
        Pattern3
    }

    public PatternStrategies bulletHellPatterns;

    private IPatternBehaviour bulletHellStrategy;

    public HellBullet normalBullet;

    public HellBullet godlikeBullet;

    public float fireRate;

    private float currentTime;

    private float currentTime2;

    public List<Transform> spawners;

    private Pool<HellBullet> bulletHellPool;

    private Pool<HellBullet> godlikePool;

    private static BulletHellSpawner instance;

    public static BulletHellSpawner Instance { get { return instance; } }

    public Vector3 SpawnPosition { get; private set; }

    public Quaternion SpawnRotation { get; private set; }

    private int index;

    private void Awake()
    {
        switch (bulletHellPatterns)
        {
            case PatternStrategies.Pattern1:
                bulletHellStrategy = new Pattern1();
                break;

            case PatternStrategies.Pattern2:
                bulletHellStrategy = new Pattern2();
                break;

            case PatternStrategies.Pattern3:
                bulletHellStrategy = new Pattern3();
                break;
        }
    }

    private void Start()
    {
        instance = this;

        bulletHellPool = new Pool<HellBullet>(20, BulletFactory, normalBullet.InitializePool, normalBullet.DisposePool, true);

        godlikePool = new Pool<HellBullet>(10, GodlikeBulletFactory, godlikeBullet.InitializePool, godlikeBullet.DisposePool, true);

        currentTime = fireRate;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        currentTime2 += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            bulletHellStrategy = new Pattern1();

        else if (Input.GetKeyDown(KeyCode.Alpha2))
            bulletHellStrategy = new Pattern2();

        else if (Input.GetKeyDown(KeyCode.Alpha3))
            bulletHellStrategy = new Pattern3();

        if (currentTime >= fireRate)
        {
            PerformPattern();

            currentTime = 0.0f;
        }

        if (bulletHellStrategy.Shoot() == 2 && currentTime2 >= fireRate / 3)
        {
            ExecutePattern();

            currentTime2 = 0.0f;
        }
    }

    public void PerformPattern()
    {
        switch (bulletHellStrategy.Shoot())
        {
            case 1:
                SpawnPosition = spawners[0].transform.position;
                SpawnRotation = spawners[0].transform.rotation;
                bulletHellPool.GetObjectFromPool();

                SpawnPosition = spawners[1].transform.position;
                SpawnRotation = spawners[1].transform.rotation;
                bulletHellPool.GetObjectFromPool();

                SpawnPosition = spawners[2].transform.position;
                SpawnRotation = spawners[2].transform.rotation;
                bulletHellPool.GetObjectFromPool();

                SpawnPosition = spawners[3].transform.position;
                SpawnRotation = spawners[3].transform.rotation;
                bulletHellPool.GetObjectFromPool();

                SpawnPosition = spawners[4].transform.position;
                SpawnRotation = spawners[4].transform.rotation;
                bulletHellPool.GetObjectFromPool();
                break;

            case 3:
                SpawnPosition = spawners[0].transform.position;
                SpawnRotation = spawners[0].transform.rotation;
                bulletHellPool.GetObjectFromPool();

                SpawnPosition = spawners[1].transform.position;
                SpawnRotation = spawners[1].transform.rotation;
                bulletHellPool.GetObjectFromPool();

                SpawnPosition = spawners[2].transform.position;
                SpawnRotation = spawners[2].transform.rotation;
                godlikePool.GetObjectFromPool();

                SpawnPosition = spawners[3].transform.position;
                SpawnRotation = spawners[3].transform.rotation;
                bulletHellPool.GetObjectFromPool();

                SpawnPosition = spawners[4].transform.position;
                SpawnRotation = spawners[4].transform.rotation;
                bulletHellPool.GetObjectFromPool();
                break;
        }
    }

    private void ExecutePattern()
    {
        var rndPrefab = Random.Range(0, 10);

        if (rndPrefab <= 5)
        {
            SpawnPosition = spawners[index].transform.position;
            SpawnRotation = spawners[index].transform.rotation;
            bulletHellPool.GetObjectFromPool();
        }

        else
        {
            SpawnPosition = spawners[index].transform.position;
            SpawnRotation = spawners[index].transform.rotation;
            godlikePool.GetObjectFromPool();
        }

        index++;

        if (index == 4)
        {
            spawners.Reverse();

            index = 0;
        }
    }

    private HellBullet BulletFactory()
    {
        return Instantiate(normalBullet);
    }

    private HellBullet GodlikeBulletFactory()
    {
        return Instantiate(godlikeBullet);
    }

    public void ReturnBulletToPool(HellBullet bullet)
    {
        bulletHellPool.DisablePoolObject(bullet);
    }

    public void ReturnGodlikeBulletToPool(HellBullet godlikeBullet)
    {
        godlikePool.DisablePoolObject(godlikeBullet);
    }
}