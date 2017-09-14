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
	public float spinTurnLimit = 90;
	float currentTurnAmount;
	float lastFlatAngle;
	float turnSpeedVelocity;
	Vector3 rollUp = Vector3.up;
	public float rollSpeed = 0.2f;
	public float turnSpeed = 1f;
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
		rotX += (stickY + mouseY) * inputSensitivity * Time.deltaTime;
	}
	void RotateCamera()
	{
		if (Input.GetKey(KeyCode.E)) rotateBack();
		else
		{
			rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

			Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
			transform.rotation = localRotation;
		}

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
	void rotateBack()
	{
		if (!(Time.deltaTime > 0) || target == null)
			return;

		var targetForward = target.forward;
		var targetUp = target.up;

		var currentFlatAngle = Mathf.Atan2(targetForward.x, targetForward.z) * Mathf.Rad2Deg;

		if (spinTurnLimit > 0)
		{
			var targetSpinSpeed = Mathf.Abs(Mathf.DeltaAngle(lastFlatAngle, currentFlatAngle)) / Time.deltaTime;
			var desiredTurnAmount = Mathf.InverseLerp(spinTurnLimit, spinTurnLimit * 0.75f, targetSpinSpeed);
			var turnReactSpeed = (currentTurnAmount > desiredTurnAmount ? .1f : 1f);
			currentTurnAmount = Mathf.SmoothDamp(currentTurnAmount, desiredTurnAmount, ref turnSpeedVelocity, turnReactSpeed);
		}
		else
		{
			currentTurnAmount = 1;
		}

		lastFlatAngle = currentFlatAngle;

		var rollRotation = Quaternion.LookRotation(targetForward, rollUp);

		rollUp = rollSpeed > 0 ? Vector3.Slerp(rollUp, targetUp, rollSpeed * Time.deltaTime) : Vector3.up;
		transform.rotation = Quaternion.Lerp(transform.rotation, rollRotation, turnSpeed * currentTurnAmount * Time.deltaTime);

		var rot = transform.localRotation.eulerAngles;
		rotY = rot.y;
		rotX = rot.x;
		// aca salta el error (?
		if (rotX < 360 && rotX > 360 - clampAngle)
			rotX = rotX - 360;
		
		Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
		transform.rotation = localRotation;

	}
}