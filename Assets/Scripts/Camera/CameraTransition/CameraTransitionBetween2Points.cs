using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransitionBetween2Points : MonoBehaviour {

	public List<TransitionPoints> transitionPoints;
	public List<float> distances;
	public PlayerController player;
	float playerRelativePos;
	int currentTransitionIndex = -1;

	void Start()
	{
		for (int i = 0; i < transitionPoints.Count; i++)
		{
			distances[i] = Vector3.Distance(transitionPoints[i].StartPoint.transform.position, transitionPoints[i].EndPoint.transform.position);
		}
	}

	void Update()
	{
		if (currentTransitionIndex != player.currentTransitionIndex)
			currentTransitionIndex = player.currentTransitionIndex;

		float total = CalculatePos();
	
		if (total > 0 && total<=1)
			print("Total"+total);

	}
	float CalculatePos()
	{
		playerRelativePos = Vector3.Distance(player.transform.position, transitionPoints[currentTransitionIndex].EndPoint.transform.position);
		return 1 - ((playerRelativePos * 100) / distances[currentTransitionIndex]) / 100;
		
	}

}
