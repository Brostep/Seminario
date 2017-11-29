using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFloor : MonoBehaviour {

	public Transform playerReset;
	BossController bossController;

	private void Awake()
	{
		bossController = FindObjectOfType<BossController>();
	}

	void OnTriggerEnter(Collider c)
	{
		// is the player? set camera promedy on and turn off normal movement
		if (c.gameObject.layer == 8)
		{
			c.gameObject.transform.position = playerReset.position;
			c.gameObject.transform.rotation = playerReset.rotation;
		}

		if(c.gameObject.layer == 10)
		{
			Destroy(c.gameObject);
			bossController.SpawnWorm();
		}

	}
}
