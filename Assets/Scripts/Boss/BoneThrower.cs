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
		{
			var x = Random.Range(transform.position.x - 16f, transform.position.x + 16f);
			var z = Random.Range(transform.position.z - 22f, transform.position.z + 10.5f);
			Vector3 randPos = new Vector3(x, transform.position.y,z);
			Vector3 randPosY = new Vector3(x, 11.53f, z);
			Instantiate(bone, randPos, Quaternion.identity);
			Instantiate(shadowBone, randPosY, Quaternion.identity);
			timePass = 0f;
		}
	}
}
