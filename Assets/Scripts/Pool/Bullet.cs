using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
	public float speed;
	public float lifeSpan;
	private float _timeAlive;
	private bool _alive;

    void Update()
	{
		_timeAlive += Time.deltaTime;
		if (_timeAlive >= lifeSpan)
			BulletsSpawner.Instance.ReturnBulletToPool(this);
		else
			transform.position += transform.forward* speed * Time.deltaTime;
	}
	void OnCollisionEnter(Collision c)
	{
		BulletsSpawner.Instance.ReturnBulletToPool(this);
	}
	public void Initialize()
	{
		_timeAlive = 0;

		//busca el bullet spawner y copia su direccion y rotacion y spawnea ahi.
		var bulletSpawner = FindObjectOfType<BulletsSpawner>().gameObject;
        transform.position = bulletSpawner.transform.position;
        transform.rotation = bulletSpawner.transform.rotation; 
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
