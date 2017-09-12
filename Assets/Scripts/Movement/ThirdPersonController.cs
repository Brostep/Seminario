using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonController : MonoBehaviour {

	Rigidbody rb;
	public Transform thirdPersonCam;
	public float movementSpeed;
	Vector3 camForward;
	Vector3 relativeMove;
	float horizontalInput;
	float verticalInput;
	float turnAmount;
	float forwardAmount;
	public float movingTurnSpeed = 360;
	public float stationaryTurnSpeed = 180;
	Vector3 groundNormal;
	bool onGround;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		GetInputs();
		CalculateMoveDir();	
	}
	// read inputs
	void GetInputs()
	{
		horizontalInput = Input.GetAxis("Horizontal");
		verticalInput = Input.GetAxis("Vertical");
	}
	// calculate move direction
	void CalculateMoveDir()
	{	
		if (thirdPersonCam != null)
		{
			//look at camera foward
			//var lookAt = new Quaternion(0,thirdPersonCam.rotation.y,0,thirdPersonCam.rotation.w);
			//transform.rotation = lookAt;

			// calculate camera relative direction to move:
			camForward = Vector3.Scale(thirdPersonCam.forward, new Vector3(1, 0, 1)).normalized;
			relativeMove = verticalInput * camForward + horizontalInput * thirdPersonCam.right;
		}
		else
		{
			//use world directions 
			relativeMove = verticalInput * Vector3.forward + horizontalInput * Vector3.right;
		}
		//move towards Dir
		Move(relativeMove);
	}
	void Move(Vector3 relMove)
	{
		if (relMove.magnitude > 1f)
			relMove.Normalize();
		var velocity = relMove * movementSpeed;
		if (onGround)
			velocity.y = 0f;
		else
			velocity.y = Physics.gravity.y;

		rb.velocity = velocity;

		//rotation 
		relMove = transform.InverseTransformDirection(relMove);
		CheckGroundStatus();
		relMove = Vector3.ProjectOnPlane(relMove, groundNormal);
		turnAmount = Mathf.Atan2(relMove.x, relMove.z);
		forwardAmount = relMove.z;
		ApplyExtraTurnRotation();
	}

	void CheckGroundStatus()
	{
		RaycastHit hitInfo;
		if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, float.MaxValue))
		{
			if (hitInfo.distance < 1f)
			{
				groundNormal = hitInfo.normal;
				onGround = true;
			}
			else
			{
				groundNormal = Vector3.up;
				onGround = false;
			}
		}
	}
	void ApplyExtraTurnRotation()
	{
		float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
		transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
	}


}
