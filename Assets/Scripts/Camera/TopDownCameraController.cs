using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCameraController : MonoBehaviour {
	public Transform target;
	public float smoothSpeed = 0.125f;
	public float offsetJoystick;
	public Vector3 offset;
	
	void FixedUpdate()
	{
		var horizontalInput = Input.GetAxis("RightStickHorizontal");
		var verticalInput = Input.GetAxis("RightStickVertical");
		Vector3 inputMovement = new Vector3(horizontalInput * offsetJoystick, 0, -verticalInput * offsetJoystick);
	
		Vector3 desiredPosition = target.position + offset + inputMovement;
		Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
		transform.position = smoothedPosition;
	}

}
