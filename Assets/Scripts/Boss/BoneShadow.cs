using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneShadow : MonoBehaviour {

	void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.layer == 14)
		{
			Destroy(this.gameObject);
		}
	}
}
