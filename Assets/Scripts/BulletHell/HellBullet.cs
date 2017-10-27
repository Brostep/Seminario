using UnityEngine;

public class HellBullet : Bullet
{
	float _timeAlive;

	public override void Initialize()
	{
		_timeAlive = 0f;

		setTransform(HellBulletSpawner.Instance.spawnPosition, HellBulletSpawner.Instance.spawnRotation);
	}
	public void setTransform(Vector3 position, Quaternion rotation)
	{
		transform.position = position;
		transform.rotation = rotation;
	}
	void Update()
	{
		_timeAlive += Time.deltaTime;

	}

	private void FixedUpdate()
	{
		if (_timeAlive >= lifeSpan)
			HellBulletSpawner.Instance.ReturnBulletToPool(this);
		else
			transform.position += transform.forward * speed * Time.deltaTime;
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == 9)
		{
			HellBulletSpawner.Instance.ReturnBulletToPool(this);
		}
	}
}
