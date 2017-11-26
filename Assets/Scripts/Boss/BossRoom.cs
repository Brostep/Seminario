using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour {

	public GameObject bridge;
	public List<GameObject> vomitLeft;
	public List<GameObject> vomitRight;
	public GameObject slow;

	void OnTriggerEnter(Collider c)
	{
		// is the player? set camera promedy on and turn off normal movement
		if (c.gameObject.layer == 8)
		{
			var playerController = FindObjectOfType<PlayerController>();
			// camera change
			playerController.topDownCamera.GetComponent<TopDownCameraController>().enabled=false;
			playerController.topDownCamera.GetComponent<TopDownPromedyTargets>().enabled=true;
			playerController.promedyTarget = true;
			playerController.cameraChange = true;
//
			PlayerController.inTopDown = !PlayerController.inTopDown;
			FindObjectOfType<HellBulletSpawner>().inRoom = true;
			StartCoroutine(VomitingLeft());
			StartCoroutine(VomitingRight());
			slow.SetActive(true);
			bridge.SetActive(false);

            GetComponent<BoxCollider>().enabled = false;
		}
	}

	IEnumerator VomitingLeft()
	{
		for (int i = 0; i < vomitLeft.Count; i++)
		{
			vomitLeft[i].SetActive(true);
			yield return new WaitForSeconds(0.07f);
		}
	
	}
	IEnumerator VomitingRight()
	{
		for (int i = 0; i < vomitRight.Count; i++)
		{
			vomitRight[i].SetActive(true);
			yield return new WaitForSeconds(0.07f);
		}
	
	}
}
