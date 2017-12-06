using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spit : MonoBehaviour {

	public bool canMove;
	public float timeAliveWhileColliding;
	PlayerController player;
	BossController boss;
	float speed = 12;
	Vector3 totalDistance;
	float timeToTravel;

	float currentTime;
	float currentTimeColliding;

	private void Awake()
	{
		player = FindObjectOfType<PlayerController>();
		boss = FindObjectOfType<BossController>();
		totalDistance = player.transform.position - transform.position;
		timeToTravel = (totalDistance.magnitude / speed) * 6f;
	}

	void Update()
	{
		if (canMove)
		{
			currentTime += Time.deltaTime;
			var scaleX = Mathf.Lerp(transform.localScale.x, 5, currentTime / timeToTravel);
			var scaleY = Mathf.Lerp(transform.localScale.y, 5, currentTime / timeToTravel);
			var scaleZ = Mathf.Lerp(transform.localScale.z, 5, currentTime / timeToTravel);
			transform.localScale = new Vector3(scaleX, scaleY, scaleZ);


			transform.position += Persuit().normalized * speed * Time.deltaTime;
		}
	
	}
	Vector3 Persuit()
	{
		var deltaPos = player.transform.position - transform.position;
		var targetPosition = player.transform.position + player.GetComponent<Rigidbody>().velocity * deltaPos.magnitude / player.movementSpeed;
		return Seek(targetPosition);
	}
	Vector3 Seek(Vector3 targetPosition)
	{
		var deltaPos = targetPosition - transform.position;
		var desiredVel = deltaPos.normalized * player.movementSpeed;
		return desiredVel - player.GetComponent<Rigidbody>().velocity;
	}

	private void OnTriggerStay(Collider c)
	{
		if (c.gameObject.layer == 20)
		{
			currentTimeColliding += Time.deltaTime;
			speed = Mathf.Lerp(speed, 0, Time.deltaTime*0.5f);
			if (currentTimeColliding >= timeAliveWhileColliding)
			{
				boss.spitDestroy = true;
				Destroy(gameObject);
			}
		}
	}
	private void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.layer == 8)
		{
			player.deathBySnuSnu = true;
			Destroy(gameObject);
		}
	}
}
