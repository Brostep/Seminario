using UnityEngine;
using System.Collections;

public class BulletsSpawner : MonoBehaviour
{
	public Bullet bulletPrefab;
	public float cooldown = 10f;
	public GameObject firePoint;
	public float clampAngle = 0f;
	private Pool<Bullet> _bulletPool;
	float rotX, rotY;
	public float inputSensitivity = 150f;
	private static BulletsSpawner _instance;
	public static BulletsSpawner Instance { get { return _instance; } }

	void Awake()
	{
		_instance = this;
		_bulletPool = new Pool<Bullet>(15, BulletFactory, Bullet.InitializeBullet, Bullet.DisposeBullet, true);
		Vector3 rot = transform.localRotation.eulerAngles;
		rotY = rot.y;
		rotX = rot.x;

		StartCoroutine(Shoot());
	}
	void Update()
	{
		if (!PlayerController.cameraChanged)
		{
			GetInputs();
			RotateCamera();		
		}
		//transform.rotation = firePoint.transform.rotation;
	}
	void GetInputs()
	{
		var mouseX = Input.GetAxis("Mouse X");
		var mouseY = Input.GetAxis("Mouse Y");
		var stickX = Input.GetAxis("RightStickHorizontal");
		var stickY = Input.GetAxis("RightStickVertical");
		rotY += (stickX + mouseX) * inputSensitivity * Time.deltaTime;
		rotX += (stickY + mouseY) * inputSensitivity * Time.deltaTime;
	}
	void RotateCamera()
	{
		rotX = Mathf.Clamp(rotX, -clampAngle, 0);
		Quaternion localRotation = Quaternion.Euler(rotX, rotY+90, 0.0f);
		transform.rotation = localRotation;
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