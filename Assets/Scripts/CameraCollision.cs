using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
	public Transform cam;
	public float minDistance = 1.0f;
	public float maxDistance = 4.0f;
	public float smooth = 10.0f;
	float distance;
	Vector3 defaultPos;
	Vector3 prevPos;

	private void Start()
	{
		defaultPos = cam.position;
	}


	void Update()
	{
		var deltaPos = cam.position - transform.position;

		RaycastHit hit;
		if (Physics.Raycast(transform.position, deltaPos, out hit, maxDistance))
			prevPos = hit.point;
		else
			prevPos = defaultPos;

		print(prevPos);

		Debug.DrawLine(transform.position,cam.position,Color.red);

		cam.position = Vector3.Lerp(cam.position, prevPos, Time.deltaTime * smooth);
	}

}
/*
	public LayerMask collisionLayer;
	public bool colliding = false;
	public Vector3[] adjustedCameraClipPoints;
	public Vector3[] desiredCameraClipPoints;
	Vector3 destination = Vector3.zero;
	Vector3 adjustedDestination = Vector3.zero;
	public float collisionSpace = 3.41f;

	Camera cam;

	void Start()
	{
		cam = FindObjectOfType<Camera>();
		UpdateCameraClipPoints(cam.transform.position, cam.transform.rotation, ref adjustedCameraClipPoints);
		UpdateCameraClipPoints(destination, cam.transform.rotation, ref desiredCameraClipPoints);
		adjustedCameraClipPoints = new Vector3[5];
		desiredCameraClipPoints = new Vector3[3];
	}

	void FixedUpdate()
	{
		UpdateCameraClipPoints(cam.transform.position, cam.transform.rotation, ref adjustedCameraClipPoints);
		UpdateCameraClipPoints(destination, cam.transform.rotation, ref desiredCameraClipPoints);
		for (int i = 0; i < 5; i++)
		{
			Debug.DrawLine(transform.position, desiredCameraClipPoints[i], Color.red);
			Debug.DrawLine(transform.position, adjustedCameraClipPoints[i], Color.green);
		}
		CheckColliding(transform.position);

	}

	void Update()
	{
		if (colliding)
		{

		}
		else
		{

		}
	}

	public void UpdateCameraClipPoints(Vector3 cameraPosition, Quaternion atRotation, ref Vector3[] intoArray)
	{
		if (!cam)
			return;

		intoArray = new Vector3[5];

		float z = cam.nearClipPlane;
		float x = Mathf.Tan(cam.fieldOfView / collisionSpace) * z;
		float y = x / cam.aspect;
		//top left
		intoArray[0] = (atRotation * new Vector3(-x, y, z)) + cameraPosition;
		//top right
		intoArray[1] = (atRotation * new Vector3(x, y, z)) + cameraPosition;
		//bottom left
		intoArray[2] = (atRotation * new Vector3(-x, -y, z)) + cameraPosition;
		//bottom right
		intoArray[3] = (atRotation * new Vector3(x, -y, z)) + cameraPosition;
		//camera position
		intoArray[4] = cameraPosition - cam.transform.forward;
	}

	bool CollisionDetectedAtClipPoins(Vector3[] clipPoints, Vector3 fromPosition)
	{
		for (int i = 0; i < clipPoints.Length; i++)
		{
			Ray ray = new Ray(fromPosition, clipPoints[i] - fromPosition);
			float distance = Vector3.Distance(clipPoints[i], fromPosition);
			if (Physics.Raycast(ray, distance, collisionLayer))
				return true;
		}
		return false;
	}

	public float getAdjustedDistanceWithRayFrom(Vector3 from)
	{
		float distance = -1;

		for (int i = 0; i < desiredCameraClipPoints.Length; i++)
		{
			Ray ray = new Ray(from, desiredCameraClipPoints[i] - from);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				if (distance == -1)
					distance = hit.distance;
				else
				{
					if (hit.distance < distance)
						distance = hit.distance;
				}
			}
		}

		if (distance == -1)
			return 0;
		else
			return distance;
	}

	public void CheckColliding(Vector3 targetPosition)
	{
		if (CollisionDetectedAtClipPoins(desiredCameraClipPoints, targetPosition))
			colliding = true;
		else
			colliding = false;
	}
	*/

