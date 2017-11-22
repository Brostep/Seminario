using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneThrower : MonoBehaviour {

	public GameObject bone;
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
			Instantiate(bone, randPos, Quaternion.identity);
			timePass = 0f;
		}
	}
}
