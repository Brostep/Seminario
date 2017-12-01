using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour {

	public float lifeTime;
	public float damage;
	float timePass;
	// Update is called once per frame
	void Update() {
		timePass += Time.deltaTime;
		if (timePass > lifeTime)
			Destroy(this.gameObject);
	}

	void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.layer == 8)
			c.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
	}
}
