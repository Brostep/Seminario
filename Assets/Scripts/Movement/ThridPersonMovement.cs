using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThridPersonMovement : MonoBehaviour {
	Rigidbody rb;
	public Camera thirdPersonCamera;
	public float movementSpeed;
	public float turnSpeed = 100;
	[Range(1f, 4f)]
	public float gravityMultiplayer = 2f;
	float fowardInput, turnInput;
	Quaternion targetRotation;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		targetRotation = transform.rotation;
	}
	void Update()
	{
		GetInputs();
		Turn();
	}
	void GetInputs()
	{
		fowardInput = Input.GetAxis("Vertical");
		turnInput = Input.GetAxis("Horizontal");
	}
	void Turn()
	{
		if (Mathf.Abs(turnInput) > 0)
			targetRotation *= Quaternion.AngleAxis(turnSpeed * turnInput * Time.deltaTime, Vector3.up);
		transform.rotation = targetRotation;
	}

	void FixedUpdate()
	{
		MovePlayer();
	}

	Vector3 ApplyExtraGravity()
	{
		Vector3 extraGravityForce = (Physics.gravity * gravityMultiplayer) - Physics.gravity;
		rb.AddForce(extraGravityForce);

		return extraGravityForce;
	}
	void MovePlayer()
	{
		if (Mathf.Abs(fowardInput) > 0)
		{
			var cameraFoward = Vector3.Scale(thirdPersonCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
			var moveDir = fowardInput*cameraFoward + turnInput * thirdPersonCamera.transform.right;
			var relativeVel = moveDir * movementSpeed + Physics.gravity;
			rb.velocity = relativeVel;
		}
	}

}
