using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(ThirdPersonController))]
[RequireComponent(typeof(TopDownMovement))]
public class PlayerController : MonoBehaviour {

	ThirdPersonController thirdPersonController;
	TopDownMovement topDownController;
	public GameObject thirdPersonCamera;
	public GameObject topDownCamera;
	public static bool cameraChanged = false;
	Animator anim;
	bool isJumping;
	bool onGround;
	Vector3 velocity;
	Rigidbody rb;
	public Image crosshair;

	void Start()
	{
		thirdPersonController = GetComponent<ThirdPersonController>();
		topDownController = GetComponent<TopDownMovement>();
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
		ChangeMovement();
;
	}
	void Update()
	{
		CheckGroundStatus();

		if (onGround)
		{
			velocity.y = 0f;
			isJumping = false;
			anim.SetBool("OnJump", false);
		}
		else
			velocity.y = velocity.y - 6f;

		if (Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("LButton"))
		{
			cameraChanged = !cameraChanged;
			ChangeMovement();
		}
	
		// chekea si esta en el piso, no aplica gravedad
		if ((Input.GetKeyDown(KeyCode.Space) || (Input.GetButton("AButton"))) && !isJumping)
		{
			anim.SetBool("OnJump", true);
			velocity.y = 200f;
		}

		rb.velocity = velocity;
	}
	void ChangeMovement()
	{
		if (cameraChanged)
		{
			//TOP DOWN
			thirdPersonCamera.SetActive(false);
			thirdPersonController.enabled = false;
			topDownCamera.SetActive(true);
			topDownController.enabled = true;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			crosshair.enabled = false;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			topDownController.enabled = false;
			topDownCamera.SetActive(false);
			thirdPersonController.enabled = true;
			thirdPersonCamera.SetActive(true);
			crosshair.enabled = true;
		}	
	}

	void CheckGroundStatus()
	{
		RaycastHit hitInfo;
		if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, float.MaxValue))
		{
			if (hitInfo.distance < 0.1f)
				onGround = true;
			else
				onGround = false;
		}
	}
}
