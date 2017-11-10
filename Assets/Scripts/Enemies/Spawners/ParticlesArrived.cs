using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesArrived : MonoBehaviour {
	public bool arrived;
	public LayerMask particleLayer;
	public bool canCollision;
	float exitTimer = 2.5f;
	float timeAlive;

	private void Update()
	{
		timeAlive += Time.deltaTime;
		if (timeAlive > exitTimer)
			canCollision = true;
	}
	private void OnCollisionEnter(Collision c)
	{
		if (canCollision)
		{
			if (c.gameObject.layer == Utility.LayerMaskToInt(particleLayer))
				arrived = true;
		}
	}
}
