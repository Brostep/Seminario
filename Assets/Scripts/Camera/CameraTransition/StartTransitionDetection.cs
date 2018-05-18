using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTransitionDetection : MonoBehaviour
{
	private void OnTriggerEnter(Collider c)
	{
		// is the player? set camera promedy on and turn off normal movement
		if (c.gameObject.layer == 8)
		{
			var playerController = FindObjectOfType<PlayerController>();
			if (!playerController.isTransitioning)
			{
				if (playerController.GetComponent<ThirdPersonController>().enabled)
					playerController.fromThirdPersonToTopDown = true;
				else
					playerController.fromTopDownToThirdPerson = true;
				playerController.currentTransitionIndex++;
				playerController.isTransitioning = true;
			}

		}
	}
}
