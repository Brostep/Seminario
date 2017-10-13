using UnityEngine;

public class HellBullet : MonoBehaviour
{
    public float movementSpeed;

    public float lifeTime;
	float _timeAlive;

	public void Initialize(Vector3 position,Quaternion rotation)
    {
		_timeAlive = 0f;
		transform.position = position;
		transform.rotation = rotation;
    }

    void Update()
    {
		_timeAlive += Time.deltaTime;
		if (_timeAlive >= lifeTime)
			HellBulletSpawner.Instance.ReturnBulletToPool(this);
		else
			transform.position += transform.forward * movementSpeed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
			HellBulletSpawner.Instance.ReturnBulletToPool(this);
		}
    }

	public static void InitializeBullet(HellBullet bulletObj)
	{
		bulletObj.gameObject.SetActive(true);
		bulletObj.Initialize(HellBulletSpawner.Instance.spawnPosition, HellBulletSpawner.Instance.spawnRotation);
	}

	public static void DisposeBullet(HellBullet bulletObj)
	{
		bulletObj.gameObject.SetActive(false);
	}
}
