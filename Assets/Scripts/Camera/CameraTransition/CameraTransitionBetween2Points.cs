using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransitionBetween2Points : MonoBehaviour {

	public List<TransitionPoints> transitionPoints;
	public List<float> distances;
	public PlayerController player;
	public GameObject PivotThirdPersonCameraHolder;
	public GameObject ThirdPersonCameraHolder;
	public GameObject TopDownCameraHolder;
	public GameObject ThirdPersonLastRotAndPos;
	public float InstantTransitionTime;
	public Camera Cam;

	float playerRelativePos;
	int currentTransitionIndex;
	Vector3 ThirdPersonCamera;
	void Start()
	{
		for (int i = 0; i < transitionPoints.Count; i++)
		{
			distances[i] = Vector3.Distance(transitionPoints[i].StartPoint.transform.position, transitionPoints[i].EndPoint.transform.position);
		}
		currentTransitionIndex = player.currentTransitionIndex;
	}

	void Update()
	{
		if (currentTransitionIndex != player.currentTransitionIndex)
			currentTransitionIndex = player.currentTransitionIndex;

		
		if (!player.inBossFight)
		{
			if (currentTransitionIndex >= 0)
			{
				float total = CalculatePos();
				if (total > 0 && total < 0.15)
				{
					// TODO: make smooth start
				}
				else if (total >= 0.15 && total < 1)
				{
					if (player.fromThirdPersonToTopDown)
						FromThirdPersonToTopDown(total);
					else if (player.fromTopDownToThirdPerson)
						FromTopDownToThirdPerson(total);
				}
			}
		}
		else
		{
			if (player.fromThirdPersonToTopDown)
				FromThirdPersonToTopDown(InstantTransitionTime);
			else if (player.fromTopDownToThirdPerson)
				FromTopDownToThirdPerson(InstantTransitionTime);
		}
		
	}
	float CalculatePos()
	{
		playerRelativePos = Vector3.Distance(player.transform.position, transitionPoints[currentTransitionIndex].EndPoint.transform.position);
		return 1 - ((playerRelativePos * 100) / distances[currentTransitionIndex]) / 100;
		
	}
	void FromThirdPersonToTopDown(float total)
	{
		print("IN");
		ThirdPersonCameraHolder.GetComponent<ThirdPersonCameraController>().enabled = false;
		PivotThirdPersonCameraHolder.transform.position = Vector3.Lerp(ThirdPersonLastRotAndPos.transform.position, TopDownCameraHolder.transform.position, total);
		PivotThirdPersonCameraHolder.transform.rotation = Quaternion.Lerp(ThirdPersonLastRotAndPos.transform.rotation, TopDownCameraHolder.transform.rotation, total);

		PivotThirdPersonCameraHolder.transform.LookAt(player.transform);

		Cam.nearClipPlane = Mathf.Lerp(player.nearClipPlaneTP, player.nearClipPlaneTD, total);
		Cam.farClipPlane = Mathf.Lerp(player.farClipPlaneTP, player.farClipPlaneTD, total);
		Cam.fieldOfView = Mathf.Lerp(player.fieldOfViewTP, player.fieldOfViewTD, total);
	}

	void FromTopDownToThirdPerson(float total)
	{
		TopDownCameraHolder.GetComponent<TopDownCameraHolder>().enabled = false;
		PivotThirdPersonCameraHolder.transform.position = Vector3.Lerp(TopDownCameraHolder.transform.position, ThirdPersonLastRotAndPos.transform.position, total);
		PivotThirdPersonCameraHolder.transform.rotation = Quaternion.Lerp(TopDownCameraHolder.transform.rotation, ThirdPersonLastRotAndPos.transform.rotation, total);

		PivotThirdPersonCameraHolder.transform.LookAt(player.transform);

		Cam.nearClipPlane = Mathf.Lerp(player.nearClipPlaneTD, player.nearClipPlaneTP, total);
		Cam.farClipPlane = Mathf.Lerp(player.farClipPlaneTD, player.farClipPlaneTP, total);
		Cam.fieldOfView = Mathf.Lerp(player.fieldOfViewTD, player.fieldOfViewTP, total);
	}

}