using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThridPersonMovement : MonoBehaviour {
	Rigidbody rb;
	public float movementSpeed;
	public float rotationVel = 100;
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
			targetRotation *= Quaternion.AngleAxis(rotationVel * turnInput * Time.deltaTime, Vector3.up);
		transform.rotation = targetRotation;
	}
	void FixedUpdate()
	{
		MovePlayer();
	}
	void MovePlayer()
	{
		if (Mathf.Abs(fowardInput) > 0)
			rb.velocity = transform.forward * fowardInput * movementSpeed;
		else
			rb.velocity = Vector3.zero;
	}

}
