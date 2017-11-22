using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour {

	public float lifeTime;
	float timePass;
	// Update is called once per frame
	void Update() {
		timePass += Time.deltaTime;
		if (timePass > lifeTime)
			Destroy(this.gameObject);
	}
}
