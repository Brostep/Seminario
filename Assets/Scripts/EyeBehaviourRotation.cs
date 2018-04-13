using UnityEngine;

public class EyeBehaviourRotation : MonoBehaviour {

	public GameObject ThirdPersonCamera;
	public float rotationSpeed;
	BulletsSpawner bulletSpawner;
	EyeBehaviour eyeBehaviour;
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
		transform.position = eyeBehaviour.player.GetComponentInChildren<Head>().transform.position;

		if (bulletSpawner.isShooting)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, ThirdPersonCamera.transform.rotation, Time.deltaTime * rotationSpeed*2);
		}
		else
			transform.rotation = Quaternion.Slerp(transform.rotation, rotationZero, Time.deltaTime * rotationSpeed);
	}
}
