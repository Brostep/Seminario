using UnityEngine;
using System.Collections;

public class BulletsSpawner : MonoBehaviour
{
	public Bullet bulletPrefab;
	public float cooldown = 10f;
	public GameObject firePoint;
	private Pool<Bullet> _bulletPool;

	private static BulletsSpawner _instance;
	public static BulletsSpawner Instance { get { return _instance; } }

	void Awake()
	{
		_instance = this;
		_bulletPool = new Pool<Bullet>(15, BulletFactory, Bullet.InitializeBullet, Bullet.DisposeBullet, true);
		StartCoroutine(Shoot());
	}
	void Update()
	{
		transform.rotation = firePoint.transform.rotation;
	}
	IEnumerator Shoot()
	{
		if (Input.GetKey(KeyCode.Mouse0) || Input.GetButton("RButton"))
		{
			_bulletPool.GetObjectFromPool();
		}

		yield return new WaitForSeconds(cooldown/10f);
		StartCoroutine(Shoot());
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