using UnityEngine;

public class EyeBehaviourRotation : MonoBehaviour {

	public GameObject ThirdPersonCamera;
	public float rotationSpeed;
	public BulletsSpawner bulletSpawner;
	EyeBehaviour eyeBehaviour;
	public GameObject player;
	Vector3 offsetThirdPerson;
	Vector3 playerPostion;
	Quaternion rotationZero;

	private void Start()
	{
		bulletSpawner	  = GetComponentInChildren<BulletsSpawner>();		
		eyeBehaviour	  = GetComponentInChildren<EyeBehaviour>();
		offsetThirdPerson = eyeBehaviour.offsetThirdPerson;
		rotationZero = Quaternion.Euler(0f, 0f, 0f);
	}
	void Update()
	{
		transform.position = new Vector3(player.transform.position.x, player.transform.position.y + offsetThirdPerson.y, player.transform.position.z);
		float currentRotation = (((360 - ThirdPersonCamera.transform.eulerAngles.x) * 100) / 45) / 100;
		if (bulletSpawner.isShooting && currentRotation <= 0.8)
			transform.rotation = Quaternion.Slerp(transform.rotation, ThirdPersonCamera.transform.rotation, Time.deltaTime * rotationSpeed*2);
		else
			transform.rotation = Quaternion.Slerp(transform.rotation, rotationZero, Time.deltaTime * rotationSpeed);
	}
}
