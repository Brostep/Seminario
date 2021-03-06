using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraCollision : MonoBehaviour
{
	public float minDistance = 1.0f;
	public float maxDistance = 4.0f;
	public float smooth = 10.0f;
	Vector3 playerDir;
	float distance;
	
	void Awake()
	{
		playerDir = transform.localPosition.normalized;
		distance = transform.localPosition.magnitude;
	}

	void Update()
	{
		Vector3 desiredCameraPos = transform.parent.TransformPoint(playerDir * maxDistance);
		RaycastHit hit;

		if (Physics.Raycast(transform.parent.position, desiredCameraPos, out hit))
			distance = Mathf.Clamp((hit.distance * 0.8f), minDistance, maxDistance);
		else
			distance = maxDistance;

		transform.localPosition = Vector3.Lerp(transform.localPosition, playerDir * distance, Time.deltaTime * smooth);
	}
}
