using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
	public float speed;
	public float lifeSpan;
	private float _tick;
	private bool _alive;

    void Update()
	{
		_tick += Time.deltaTime;
		if (_tick >= lifeSpan)
		{
			BulletsSpawner.Instance.ReturnBulletToPool(this);
		}
		else
		{
			transform.position += transform.forward * speed * Time.deltaTime;
		}
	}

	public void Initialize()
	{
		_tick = 0;
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
