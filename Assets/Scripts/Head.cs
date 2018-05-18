using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour {

	public GameObject Eye;
	public GameObject Character;
	Vector3 headPosition;
	float offsetPositionY;
	Vector3 eyePosition;
	public ThirdPersonCameraController ThirdPersonCamera;
	float currentPercentage;
	BulletsSpawner bulletSpawn;
	bool wasShooting;

	private void Start()
	{
		offsetPositionY = this.transform.position.y;
		bulletSpawn = Eye.GetComponent<EyeBehaviourRotation>().bulletSpawner;
	}
	private void Update()
	{
		headPosition = new Vector3(Character.transform.position.x, Character.transform.position.y + offsetPositionY, Character.transform.position.z);
		eyePosition = new Vector3(headPosition.x + Eye.GetComponentInChildren<EyeBehaviour>().offsetThirdPerson.x, headPosition.y, headPosition.z);
		CalculateCurrentPercentage();
		if (currentPercentage<=1)
			this.transform.position = Vector3.Lerp(headPosition, eyePosition, currentPercentage);
	}
	void CalculateCurrentPercentage()
	{
		currentPercentage = (((360 - ThirdPersonCamera.transform.eulerAngles.x) * 100) / 45) / 100;
	}
}
