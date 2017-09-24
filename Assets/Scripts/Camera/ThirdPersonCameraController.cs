using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
	public float CameraMoveSpeed = 120.0f;
	public GameObject CameraFollowObj;
	public float clampAngle = 80.0f;
	public float inputSensitivityX= 150.0f;
	public float inputSensitivityY = 150.0f;
	public GameObject test;
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
	Quaternion auxRotation;
	public float timeSinceNoRotation;
	float tickSinceNoRotation;
	GameObject nearestEnemy;
	bool isTargeting;
	Camera cam;

	void Start()
	{
		Vector3 rot = transform.localRotation.eulerAngles;
		rotY = rot.y;
		rotX = rot.x;
		target = CameraFollowObj.transform;
		transform.position = target.transform.position;
		cam = GetComponentInChildren<Camera>();
	}
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab)&&!isTargeting)
		{
			List<GameObject> enemiesInRadius = new List<GameObject>();
			Collider[] hitColliders = Physics.OverlapSphere(transform.position, 15f);
			float distanceNearestEnemy = 0f;
			foreach (var item in hitColliders)
			{
				if (item.gameObject.layer == 10)
				{
					var distance = Vector3.Distance(transform.position, item.transform.localPosition);
					if (distance < distanceNearestEnemy || distanceNearestEnemy == 0f)
					{
						distanceNearestEnemy = distance;
						nearestEnemy = item.gameObject;
					}
				}
			}
			isTargeting = true;
		}
		else if (isTargeting)
		{
			transform.LookAt(nearestEnemy.transform);
		}
		else 
		{
			GetInputs();
			RotateCamera();
		}
	}
	void GetInputs()
	{
		mouseX = Input.GetAxis("Mouse X");
		mouseY = Input.GetAxis("Mouse Y");
		stickX = Input.GetAxis("RightStickHorizontal");
		stickY = Input.GetAxis("RightStickVertical");
		rotY += (stickX + mouseX) * inputSensitivityY * Time.deltaTime;
		rotX += (stickY + mouseY) * inputSensitivityX * Time.deltaTime;
	}
	void RotateCamera()
	{
		
		rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

		Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
		transform.rotation = localRotation;

		if (auxRotation == transform.rotation)
		{
			tickSinceNoRotation += Time.deltaTime;
			if (timeSinceNoRotation < tickSinceNoRotation)
				rotateBack(1f);//que tan rapido rota hacia la espalda
		}
		else
			tickSinceNoRotation = 0f;

		if (Input.GetKey(KeyCode.R))
			rotateBack(6f);
			
		auxRotation = transform.rotation;
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
	void rotateBack(float multiplier)
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
		transform.rotation = Quaternion.Lerp(transform.rotation, rollRotation, turnSpeed * currentTurnAmount * Time.deltaTime * multiplier);

		var rot = transform.localRotation.eulerAngles;
		rotY = rot.y;
		rotX = rot.x;

		if (rotX < 360 && rotX > 360 - clampAngle)
			rotX = rotX - 360;
		
		Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
		transform.rotation = localRotation;

	}
}