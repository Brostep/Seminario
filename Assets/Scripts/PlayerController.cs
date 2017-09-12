using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ThirdPersonController))]
[RequireComponent(typeof(TopDownMovement))]
public class PlayerController : MonoBehaviour {

	ThirdPersonController thirdPersonController;
	TopDownMovement topDownController;
	public GameObject thirdPersonCamera;
	public GameObject topDownCamera;
	public static bool cameraChanged = false;
	void Start()
	{
		thirdPersonController = GetComponent<ThirdPersonController>();
		topDownController = GetComponent<TopDownMovement>();
		ChangeMovement();
	}
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			cameraChanged = !cameraChanged;
			ChangeMovement();
		}
	}
	void ChangeMovement()
	{
		if (cameraChanged)
		{
			//TOP DOWN
			thirdPersonCamera.SetActive(false);
			thirdPersonController.enabled = false;
			topDownCamera.SetActive(true);
			topDownController.enabled = true;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			topDownController.enabled = false;
			topDownCamera.SetActive(false);
			thirdPersonController.enabled = true;
			thirdPersonCamera.SetActive(true);
		}	
	}
}
