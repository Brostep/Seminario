using UnityEngine;
using System.Collections;

public class BulletsSpawner : MonoBehaviour
{
    public Bullet bulletPrefab;
    public float cooldown = 10f;
    public float inputSensitivity = 150f;
    public float clampAngle = 0f;
    float cd;
    float rotX, rotY;

    public GameObject firePoint;
    private Pool<Bullet> _bulletPool;

    private static BulletsSpawner _instance;
    public static BulletsSpawner Instance { get { return _instance; } }

    void Awake()
    {
        _instance = this;
        _bulletPool = new Pool<Bullet>(15, BulletFactory, Bullet.InitializeBullet, Bullet.DisposeBullet, true);
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;

        //	StartCoroutine(Shoot());
    }
    void Update()
    {
        if (!PlayerController.cameraChanged)
            RotateSpawner();
        else
            transform.rotation = transform.parent.rotation;

        cd += Time.deltaTime;

        if (((Input.GetKey(KeyCode.Mouse0) || (Input.GetButton("RButton"))) && (cooldown / 10f) < cd))
        {
            _bulletPool.GetObjectFromPool();
            cd = 0f;
        }
    }
    void RotateSpawner()
    {
        Vector3 rot = firePoint.transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        if (rotX <= 360 && rotX >= 360 - clampAngle)
            rotX = rotX - 360;

        rotX = Mathf.Clamp(rotX, -clampAngle, 0);
        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;
    }
    //	IEnumerator Shoot()
    //	{
    //		while (Input.GetKey(KeyCode.Mouse0) || Input.GetButton("RButton"))
    //		{
    //			_bulletPool.GetObjectFromPool();
    //			yield return new WaitForSeconds(cooldown / 10f);
    //		}
    //	}
    private Bullet BulletFactory()
    {
        return Instantiate<Bullet>(bulletPrefab);
    }

    public void ReturnBulletToPool(Bullet bullet)
    {
        _bulletPool.DisablePoolObject(bullet);
    }
}
