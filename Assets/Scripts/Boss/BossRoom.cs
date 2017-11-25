using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour {

	public GameObject bridge;
	public GameObject vomit;
	public GameObject slow;

	void OnTriggerEnter(Collider c)
	{
		// is the player? set camera promedy on and turn off normal movement
		if (c.gameObject.layer == 8)
		{
			var playerController = FindObjectOfType<PlayerController>();
			playerController.topDownCamera.GetComponent<TopDownCameraController>().enabled=false;
			playerController.topDownCamera.GetComponent<TopDownPromedyTargets>().enabled=true;
			playerController.promedyTarget = true;
			playerController.cameraChange = true;
			PlayerController.inTopDown = !PlayerController.inTopDown;
			FindObjectOfType<HellBulletSpawner>().inRoom = true;
			vomit.SetActive(true);
			slow.SetActive(true);
			bridge.SetActive(false);

            GetComponent<BoxCollider>().enabled = false;
		}

	}
}
