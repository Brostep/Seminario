using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OLDThirdPersonCameraController : MonoBehaviour
{
	public float CameraMoveSpeed = 120.0f;
	public GameObject CameraFollowObj;
	public float clampAngle = 80.0f;
	public float inputSensitivity = 150.0f;
	float mouseX, stickX;
	float mouseY, stickY;
	float rotY = 0f;
	float rotX = 0f;
	Transform target;
	Vector3 stoped;
	void Start()
	{
		Vector3 rot = transform.localRotation.eulerAngles;
		rotY = rot.y;
		rotX = rot.x;
		target = CameraFollowObj.transform;
		transform.position = target.transform.position;
	}

	void Update()
	{
		GetInputs();
		RotateCamera();
	}
	void GetInputs()
	{
		mouseX = Input.GetAxis("Mouse X");
		mouseY = Input.GetAxis("Mouse Y");
		stickX = Input.GetAxis("RightStickHorizontal");
		stickY = Input.GetAxis("RightStickVertical");
		rotY += (stickX + mouseX) * inputSensitivity * Time.deltaTime;
		rotX += (stickY + mouseY )* inputSensitivity * Time.deltaTime;
	}
	void RotateCamera()
	{
		rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

		Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
		transform.rotation = localRotation;
	}

	void FixedUpdate()
	{
		CameraUpdater();
	}

	void CameraUpdater()
	{
		if (!(Time.deltaTime > 0) || target == null)
			return;

		if (target.transform.position == stoped)
			transform.position = Vector3.Lerp(transform.position, target.position, (Time.deltaTime * (CameraMoveSpeed/ 1.5f)));
		else
		{
			stoped = target.transform.position;
			transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * CameraMoveSpeed);
		}
	}
}