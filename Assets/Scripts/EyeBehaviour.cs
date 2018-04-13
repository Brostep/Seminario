using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBehaviour : MonoBehaviour {

	public GameObject player;// pivot
	public BulletsSpawner bulletSpawner;
	public Vector3 offsetThirdPerson;
	public Vector3 offsetTopDown;

	bool alreadyPositionedThird;
	bool alreadyPositionedTopDown;
	void Update()
	{
		InitialPositionWhenModeChange();
	}
	void InitialPositionWhenModeChange()
	{
		if (!PlayerController.inTopDown && !alreadyPositionedThird)
		{
			transform.position = player.transform.position + offsetThirdPerson;
			alreadyPositionedThird = true;
			alreadyPositionedTopDown = false;
		}
		else if (PlayerController.inTopDown && !alreadyPositionedTopDown)
		{
			transform.position = player.transform.position + offsetTopDown;
			alreadyPositionedTopDown = true;
			alreadyPositionedThird = false;
		}
	}
}
