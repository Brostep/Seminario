using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
	public float speed;
	public float lifeSpan;
	float _timeAlive;
	private bool _alive;
	Vector3 dir;
	Vector3 enemyPos;
	ThirdPersonCameraController tPCC;
    void Update()
	{
		_timeAlive += Time.deltaTime;
		if (_timeAlive >= lifeSpan)
			BulletsSpawner.Instance.ReturnBulletToPool(this);
		else
		{
			if (tPCC.isTargeting)
			{
				transform.position +=  dir * speed * Time.deltaTime;
			}
			else
				transform.position += transform.forward * speed * Time.deltaTime;
		}

	}
	void OnCollisionEnter(Collision c)
	{
		BulletsSpawner.Instance.ReturnBulletToPool(this);
	}
	public void Initialize()
	{
		_timeAlive = 0;
		dir = new Vector3(0f, 0f, 0f);
		enemyPos = new Vector3(0f, 0f, 0f);

		//busca el bullet spawner y copia su direccion y rotacion y spawnea ahi.
		var bulletSpawner = FindObjectOfType<BulletsSpawner>().gameObject;
		tPCC = FindObjectOfType<ThirdPersonCameraController>();
		if (tPCC.isTargeting)
			enemyPos = tPCC.nearestEnemy.transform.position;

        transform.position = bulletSpawner.transform.position;
        transform.rotation = bulletSpawner.transform.rotation;

		dir = (enemyPos - transform.position).normalized;
	}

	public static void InitializeBullet(Bullet bulletObj)
	{
		bulletObj.gameObject.SetActive(true);
		bulletObj.Initialize();
	}

	public static void DisposeBullet(Bullet bulletObj)
	{
		bulletObj.gameObject.SetActive(false);
	}
}
