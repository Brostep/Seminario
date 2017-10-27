using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyWormBullets : Bullet {

	float _timeAlive;
	Vector3 dir;
	Vector3 targetPosition;
	GameObject player;
	ThirdPersonCameraController tPCC;
	public bool placed;

	public void setTransform(Vector3 position, Quaternion rotation)
	{
		if (!placed)
		{
			transform.position = position;
			transform.rotation = rotation;
			var deltaPos = player.transform.position - transform.position;
			targetPosition = player.transform.position + player.GetComponent<Rigidbody>().velocity * deltaPos.magnitude / player.GetComponent<PlayerController>().movementSpeed;
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
				dir = (targetPosition - transform.position).normalized;
				transform.position += dir * speed * Time.deltaTime;
			}
		}
	}
	void OnCollisionEnter(Collision c)
	{
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
