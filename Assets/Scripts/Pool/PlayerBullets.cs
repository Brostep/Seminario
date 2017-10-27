using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullets : Bullet {

	float _timeAlive;
	Vector3 dir;
	Vector3 enemyPos;
	ThirdPersonCameraController tPCC;
	bool lockedOnTarget;

	void Update()
	{
		_timeAlive += Time.deltaTime;
		if (_timeAlive >= lifeSpan)
			BulletsSpawner.Instance.ReturnBulletToPool(this);
		else
		{
			if (lockedOnTarget)
			{
				transform.position += dir * speed * Time.deltaTime;
			}
			else
				transform.position += transform.forward * speed * Time.deltaTime;
		}

	}
	void OnCollisionEnter(Collision c)
	{
		BulletsSpawner.Instance.ReturnBulletToPool(this);
	}
	public override void Initialize()
	{
		_timeAlive = 0;
		dir = new Vector3(0f, 0f, 0f);
		enemyPos = new Vector3(0f, 0f, 0f);
		lockedOnTarget = false;

		//busca el bullet spawner y copia su direccion y rotacion y spawnea ahi.
		var bulletSpawner = FindObjectOfType<BulletsSpawner>().gameObject;
		tPCC = FindObjectOfType<ThirdPersonCameraController>();
		if (tPCC.isTargeting)
		{
			enemyPos = tPCC.nearestEnemy.transform.position;

			lockedOnTarget = true;
		}
		transform.position = bulletSpawner.transform.position;
		transform.rotation = bulletSpawner.transform.rotation;
		dir = (enemyPos - transform.position).normalized;

	}

}
