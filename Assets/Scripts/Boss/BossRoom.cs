using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour {

	public GameObject bridge;

	void OnTriggerEnter(Collider c)
	{
		// is the player? set camera promedy on and turn off normal movement
		if (c.gameObject.layer == 8)
		{
			var playerController = c.gameObject.GetComponent<PlayerController>();
			playerController.topDownCamera.GetComponentInParent<TopDownCameraController>().enabled=false;
			playerController.topDownCamera.GetComponentInParent<TopDownPromedyTargets>().enabled=true;
			bridge.SetActive(false);
		}

	}
}
