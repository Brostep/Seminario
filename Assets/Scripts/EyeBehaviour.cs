using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBehaviour : MonoBehaviour {

	public GameObject player;
	public Vector3 offsetThirdPerson;
	public Vector3 offsetTopDown;
	void Update () {

		if (!PlayerController.inTopDown)
		{
			transform.position = player.transform.position + offsetThirdPerson;
		}
		else if (PlayerController.inTopDown)
		{
			transform.position = player.transform.position + offsetTopDown;
		}
		
	}
}
