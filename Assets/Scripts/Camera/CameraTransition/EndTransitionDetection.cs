using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTransitionDetection : MonoBehaviour
{

	private void OnTriggerEnter(Collider c)
	{
		// is the player? set camera promedy on and turn off normal movement
		if (c.gameObject.layer == 8)
		{
			var playerController = FindObjectOfType<PlayerController>();
			if (playerController.isTransitioning)
			{
				if (playerController.GetComponent<ThirdPersonController>().enabled)
				{
					playerController.fromThirdPersonToTopDown = false;
					playerController.Cam.transform.SetParent(playerController.topDownCamera.transform);
					playerController.thirdPersonCamera.SetActive(false);
					playerController.GetComponent<ThirdPersonController>().enabled = false;
					playerController.GetComponent<TopDownController>().enabled = true;
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
					playerController.crosshair.enabled = false;
					PlayerController.inTopDown = !PlayerController.inTopDown;
					playerController.isTransitioning = false;
				}
				//Top Down TODO:
				/*else
				{
					playerController.fromTopDownToThirdPerson = false;
					playerController.Cam.transform.SetParent(playerController.thirdPersonCamera.transform);
					playerController.thirdPersonCamera.SetActive(true);
					playerController.GetComponent<TopDownController>().enabled = false;
					playerController.GetComponent<ThirdPersonController>().enabled = true;
					playerController.thirdPersonCamera.GetComponentInParent<ThirdPersonCameraController>().SetCameraAtTheBack();
					Cursor.lockState = CursorLockMode.Locked;
					Cursor.visible = false;
					playerController.crosshair.enabled = true;
					PlayerController.inTopDown = !PlayerController.inTopDown;
					playerController.isTransitioning = false;
				}*/
			}
		}
	}
}