using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

	Rigidbody rBody;
	public Camera thirdPersonCamera;
	public Camera topDownCamera;
	public float movementSpeed;
	public float smoothingCameraTime;
	Vector3 inputMovement;
	public static bool cameraChanged=false;
	float fowardInput, turnInput;
	public float rotationVel;

	Camera topDown;
	Camera thirdPerson;
	public Transform originalThirdPerson;
	public Transform originalTopDown;

	Quaternion targetRotation;
	public Quaternion TargetRotation
	{
		get { return targetRotation; }
	}

	void Start()
	{
		rBody = GetComponent<Rigidbody>();
		targetRotation = transform.rotation;
		topDown = topDownCamera.GetComponent<Camera>();
		topDown.enabled = false;
		thirdPerson = thirdPersonCamera.GetComponent<Camera>();
		thirdPerson.enabled = true;
	}
	void GetInputs()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
			cameraChanged = !cameraChanged;

		fowardInput = Input.GetAxis("Vertical");
		turnInput = Input.GetAxis("Horizontal");
	}
	void Update()
	{
		GetInputs();
		smoothCameraChange();

		if (cameraChanged)
			TopDownPlayerRotation();
		else
			ThirdPersonRotation();
	}

	void FixedUpdate()
	{
		if (cameraChanged)
			MovePlayerThirdPerson();
		else
			MovePlayerTopDown();
	}
	void smoothCameraChange()
	{
		if (cameraChanged&&thirdPerson.enabled)
		{
			thirdPerson.transform.position = Vector3.Lerp(thirdPerson.transform.position, originalTopDown.position, Time.deltaTime * smoothingCameraTime);
			thirdPerson.transform.LookAt(transform.position);
			var actualPos = topDown.transform.position - thirdPerson.transform.position;
			if (actualPos.y <= 0.2f )
			{
				thirdPerson.transform.position = originalThirdPerson.position;
				thirdPerson.transform.rotation = originalThirdPerson.rotation;
				topDownCamera.GetComponent<Camera>().enabled = true;
				thirdPersonCamera.GetComponent<Camera>().enabled = false;
			}
		}
		if (!cameraChanged && topDown.enabled)
		{
			topDown.transform.position = Vector3.Lerp(topDown.transform.position, originalThirdPerson.position, Time.deltaTime * smoothingCameraTime);
			topDown.transform.LookAt(transform.position);
			var actualPos = topDown.transform.position - thirdPerson.transform.position;
			if (actualPos.y <= 0.5f)
			{
				topDown.transform.position = originalTopDown.position;
				topDown.transform.rotation = originalTopDown.rotation;
				thirdPerson.GetComponent<Camera>().enabled = true;
				topDown.GetComponent<Camera>().enabled = false;
			}
		}
	}
	void MovePlayerThirdPerson()
	{
		rBody.velocity = transform.forward * fowardInput * movementSpeed;
	}
	void MovePlayerTopDown()
	{
		inputMovement = new Vector3(turnInput, 0f, fowardInput);
		rBody.velocity = inputMovement * movementSpeed;
	}
	void ThirdPersonRotation()
	{
		targetRotation *= Quaternion.AngleAxis(rotationVel * turnInput * Time.deltaTime,Vector3.up);
		transform.rotation = targetRotation;
	}
	void TopDownPlayerRotation()
	{
		Ray cameraRay = topDownCamera.ScreenPointToRay(Input.mousePosition);
		Plane groudPlane = new Plane(Vector3.up, Vector3.zero);
		float rayLength;
		if (groudPlane.Raycast(cameraRay,out rayLength))
		{
			Vector3 pointToLook = cameraRay.GetPoint(rayLength);
			transform.LookAt(new Vector3(pointToLook.x,transform.position.y, pointToLook.z));
		}
	}
}
