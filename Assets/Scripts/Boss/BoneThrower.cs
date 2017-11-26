using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneThrower : MonoBehaviour {

	public GameObject bone;
	public GameObject shadowBone;
	public float timeBeteweenSpawn;
	float timePass;

	private void Update()
	{
		timePass += Time.deltaTime;
		if (timePass> timeBeteweenSpawn)
		{												//16						//16
			var x = Random.Range(transform.position.x - 8f, transform.position.x + 8f);
														//22						//10.5
			var z = Random.Range(transform.position.z - 8f, transform.position.z + 8f);
			Vector3 randPos = new Vector3(x, transform.position.y,z);
			Vector3 randPosY = new Vector3(x, 11.53f, z);
			Instantiate(bone, randPos, Quaternion.identity);
			Instantiate(shadowBone, randPosY, Quaternion.identity);
			timePass = 0f;
		}

	}
}
