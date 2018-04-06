using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyWormBullets : Bullet {

	float _timeAlive;
	Vector3 dir;
	Vector3 distance;
	Vector3 targetPosition;
	GameObject player;
	ThirdPersonCameraController tPCC;
	public bool placed;
	public float playerRadiusDetection;
	bool stopChasing;

	public void setTransform(Vector3 position, Quaternion rotation)
	{
		if (!placed)
		{
			transform.position = position;
			transform.rotation = rotation;
			targetPosition = player.transform.position;
			//var deltaPos = player.transform.position - transform.position;

		}
	}

	void Update()
	{
		if (placed)
		{
			_timeAlive += Time.deltaTime;
		}

	}
	private void FixedUpdate()
	{
		if (placed)
		{
			if (_timeAlive >= lifeSpan)
				GameManager.Instance.ReturnBulletToPool(this);
			else
			{		
				if (distance.magnitude < playerRadiusDetection&&!stopChasing)
				{
					targetPosition = player.transform.position + player.GetComponent<Rigidbody>().velocity * distance.magnitude / player.GetComponent<PlayerController>().movementSpeed;
				}
				else if (!stopChasing)
				{
					targetPosition = player.transform.position;
				}
				distance = targetPosition - transform.position;
				dir = distance.normalized;
				transform.position += dir * speed * Time.deltaTime;
			}
		}
	}
	void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.layer == 8)
		{
			c.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
		}

		GameManager.Instance.ReturnBulletToPool(this);
	}
	public override void Initialize()
	{
		placed = false;
		_timeAlive = 0;
		dir = new Vector3(0f, 0f, 0f);
		player = null;
		player = FindObjectOfType<PlayerController>().gameObject;

	}
}
