using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
	public float speed;
	public float lifeSpan;
	float _timeAlive;
	private bool _alive;
	Vector3 offset;
	ThirdPersonCameraController tPCC;
    void Update()
	{
		_timeAlive += Time.deltaTime;
		if (_timeAlive >= lifeSpan)
			BulletsSpawner.Instance.ReturnBulletToPool(this);
		else
		{
		//	if (tPCC.isTargeting)
		//	{
		//		if (Input.GetAxis("Horizontal") >= 0)
		//			offset = new Vector3(-0.5f, 0f, 0f);
		//		else
		//			offset = new Vector3(0.5f, 0f, 0f);
		//	}

			transform.position += transform.forward + offset * speed * Time.deltaTime;
		}
		

			
	}
	void OnCollisionEnter(Collision c)
	{
		BulletsSpawner.Instance.ReturnBulletToPool(this);
	}
	public void Initialize()
	{
		_timeAlive = 0;
		offset = new Vector3(0f, 0f, 0f);

		//busca el bullet spawner y copia su direccion y rotacion y spawnea ahi.
		var bulletSpawner = FindObjectOfType<BulletsSpawner>().gameObject;
		tPCC = FindObjectOfType<ThirdPersonCameraController>();
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
