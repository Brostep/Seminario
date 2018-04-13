using UnityEngine;
using System.Collections;

public class BulletsSpawner : MonoBehaviour
{
	public Bullet bulletPrefab;
	public float cooldown = 10f;
	public float inputSensitivity = 150f;
	public float clampAngle = 0f;
	public bool isShooting = false;
	float cd;
	float rotX, rotY;
	public GameObject eye;
	public GameObject firePoint;
	private Pool<Bullet> _bulletPool;

	private static BulletsSpawner _instance;
	public static BulletsSpawner Instance { get { return _instance; } }

	void Awake()
	{
		if (_instance == null)
			_instance = this;
		_bulletPool = new Pool<Bullet>(20, BulletFactory, bulletPrefab.InitializePool, bulletPrefab.DisposePool, true);
		Vector3 rot = transform.localRotation.eulerAngles;
		rotY = rot.y;
		rotX = rot.x;
	}
	void Update()
	{
		
		if (!PlayerController.inTopDown)
			RotateSpawner();
		else
			transform.rotation = transform.parent.rotation;

		cd += Time.deltaTime;

		if (((Input.GetMouseButton(2)|| (Input.GetButton("RButton"))) && (cooldown/10f)<cd ))
		{
			isShooting = true;
			_bulletPool.GetObjectFromPool();
			cd = 0f;
		}
		else if ((cooldown / 10f) < cd)
			isShooting = false;
	}
	void RotateSpawner()
	{
		Vector3 rot = firePoint.transform.localRotation.eulerAngles;
		rotY = rot.y;
		rotX = rot.x;
		if (rotX <= 360 && rotX >= 360 - clampAngle)
			rotX = rotX - 360;

		rotX = Mathf.Clamp(rotX, -clampAngle, 10f);
		Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
		transform.rotation = localRotation;
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