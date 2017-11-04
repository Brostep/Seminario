using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(ThirdPersonController))]
[RequireComponent(typeof(TopDownMovement))]
public class PlayerController : MonoBehaviour
{
	public Camera Cam;
	ThirdPersonController thirdPersonController;
	TopDownMovement topDownController;
	public GameObject thirdPersonCamera;
	public GameObject topDownCamera;
	public static bool inTopDown = false;
	public Transform melee;
	public float meleeRadius;
	public float lightAttackDamage;
	public float heavyAttackDamage;
	Animator anim;
	bool isJumping;
	bool onGround;
	public bool cameraChange;
	Vector3 velocity;
	Rigidbody rb;
	public Image crosshair;
	[HideInInspector]
	public float movementSpeed;
	[SerializeField]
	private float _life;
	public float life
	{
		set { _life = value; }
		get { return _life; }
	}

	[Header("TopDown Camera Settings")]
	public float nearClipPlaneTD;
	public float farClipPlaneTD;
	[Range(1f, 179f)]
	public float fieldOfViewTD;

	[Header("ThirdPerson Camera Settings")]
	public float nearClipPlaneTP;
	public float farClipPlaneTP;
	[Range(1f, 179f)]
	public float fieldOfViewTP;

	void Start()
	{
		life = _life;
		thirdPersonController = GetComponent<ThirdPersonController>();
		topDownController = GetComponent<TopDownMovement>();
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
		ChangeMovement();
		movementSpeed = thirdPersonController.movementSpeed;
	}
	void Update()
	{
		CheckGroundStatus();

		print(_life);

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
			inTopDown = !inTopDown;
			cameraChange = true;
		}

		if (cameraChange)
			ChangeMovement();
		// chekea si esta en el piso, no aplica gravedad
		if ((Input.GetKeyDown(KeyCode.Space) || (Input.GetButton("AButton"))) && !isJumping)
		{
			anim.SetBool("OnJump", true);
			velocity.y = 200f;
		}

		MeleeHit();
	}
	private void FixedUpdate()
	{
		rb.velocity = velocity;
	}
	void ChangeMovement()
	{
		if (inTopDown)
		{
			//TOP DOWN
			SetCameraForTopDown();
			
			thirdPersonCamera.SetActive(false);
			thirdPersonController.enabled = false;
			topDownController.enabled = true;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			crosshair.enabled = false;
			cameraChange = false;
		}
		else
		{
			SetCameraForThirdPerson();
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			topDownController.enabled = false;
			thirdPersonController.enabled = true;
			thirdPersonCamera.SetActive(true);
			thirdPersonCamera.GetComponentInParent<ThirdPersonCameraController>().setCameraAtTheBack();
			crosshair.enabled = true;
			cameraChange = false;
		}
	}
	void SetCameraForTopDown()
	{
		Cam.transform.SetParent(topDownCamera.transform);
		Cam.transform.rotation = topDownCamera.transform.rotation;
		Cam.nearClipPlane = nearClipPlaneTD;
		Cam.farClipPlane = farClipPlaneTD;
		Cam.fieldOfView = fieldOfViewTD;
	}
	void SetCameraForThirdPerson()
	{
		Cam.transform.SetParent(thirdPersonCamera.transform);
		Cam.transform.rotation = thirdPersonCamera.transform.rotation;
		Cam.nearClipPlane = nearClipPlaneTP;
		Cam.farClipPlane = farClipPlaneTP;
		Cam.fieldOfView = fieldOfViewTP;
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
	void MeleeHit()
	{
		if (Input.GetKeyDown(KeyCode.E)||Input.GetButton("XButton"))
		{
			if(!inTopDown)
				transform.rotation = new Quaternion(transform.rotation.x, thirdPersonCamera.transform.rotation.y, transform.rotation.z,thirdPersonCamera.transform.rotation.w);
			var enemiesHited = Physics.OverlapSphere(melee.position, meleeRadius, LayerMask.GetMask("Enemy"));
			if (enemiesHited.Length > 0)
			{
				foreach (var enemy in enemiesHited)
				{
					var currEnemy = enemy.GetComponent<Enemy>();
					if (currEnemy.transform.position.y < 3f)
						currEnemy.life -= lightAttackDamage;
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.F) || Input.GetButton("YButton"))
		{
			if (!inTopDown)
				transform.rotation = new Quaternion(transform.rotation.x, thirdPersonCamera.transform.rotation.y, transform.rotation.z, thirdPersonCamera.transform.rotation.w);
			var enemiesHited = Physics.OverlapSphere(melee.position, meleeRadius, LayerMask.GetMask("Enemy"));
			if (enemiesHited.Length > 0)
			{
				foreach (var enemy in enemiesHited)
				{
					var currEnemy = enemy.GetComponent<Enemy>();
					if (currEnemy.transform.position.y < 3f)
						currEnemy.life -= heavyAttackDamage;
				}
				
			}
		}
	}
	void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		if (Input.GetKey(KeyCode.E))
		{
			Gizmos.DrawSphere(melee.position, meleeRadius);
		}
	}
}
