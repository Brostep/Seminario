using UnityEngine;
using System.Collections;

public class BulletsSpawner : MonoBehaviour
{
	public Bullet bulletPrefab;
	private Pool<Bullet> _bulletPool;

	private static BulletsSpawner _instance;
	public static BulletsSpawner Instance { get { return _instance; } }

	void Awake()
	{
		_instance = this;
		_bulletPool = new Pool<Bullet>(15, BulletFactory, Bullet.InitializeBullet, Bullet.DisposeBullet, true);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			_bulletPool.GetObjectFromPool();
		}
	}

	private Bullet BulletFactory()
	{
		return Instantiate<Bullet>(bulletPrefab);
	}

	public void ReturnBulletToPool(Bullet bullet)
	{
		_bulletPool.DisablePoolObject(bullet);
	}
}