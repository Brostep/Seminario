using System.Collections;
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

	public HellBullet prefab;

	public float fireRate;

	private float currentTime;

	public Transform[] spawners;

	private Pool<HellBullet> bulletHellPool;

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

		bulletHellPool = new Pool<HellBullet>(20, BulletFactory, prefab.InitializePool, prefab.DisposePool, true);

		currentTime = fireRate;
	}

	private void Update()
	{
		currentTime += Time.deltaTime;

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

			case 2:
				StartCoroutine(ExecutePattern());
				break;

			case 3:
				SpawnPosition = spawners[0].transform.position;
				SpawnRotation = spawners[0].transform.rotation;
				bulletHellPool.GetObjectFromPool();

				SpawnPosition = spawners[1].transform.position;
				SpawnRotation = spawners[1].transform.rotation;
				bulletHellPool.GetObjectFromPool();
				/*
				SpawnPosition = spawners[2].transform.position;
				SpawnRotation = spawners[2].transform.rotation;
				bulletHellPool.GetObjectFromPool();
				*/
				SpawnPosition = spawners[3].transform.position;
				SpawnRotation = spawners[3].transform.rotation;
				bulletHellPool.GetObjectFromPool();

				SpawnPosition = spawners[4].transform.position;
				SpawnRotation = spawners[4].transform.rotation;
				bulletHellPool.GetObjectFromPool();
				break;
		}
	}

	IEnumerator ExecutePattern()
	{
		SpawnPosition = spawners[index].transform.position;
		SpawnRotation = spawners[index].transform.rotation;
		bulletHellPool.GetObjectFromPool();

		if (index >= 0 && index < 4)
		{
			index++;
		}

		else
		{
			index = 0;
		}
		
		yield return null;
	}

	private HellBullet BulletFactory()
	{
		return Instantiate(prefab);
	}

	public void ReturnBulletToPool(HellBullet bullet)
	{
		bulletHellPool.DisablePoolObject(bullet);
	}
}
