using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(ThirdPersonController))]
[RequireComponent(typeof(TopDownMovement))]
public class PlayerController : MonoBehaviour
{
	public Camera Cam;
	ThirdPersonController thirdPersonController;
	TopDownMovement topDownController;
	public GameObject thirdPersonCameraBase;
	public GameObject thirdPersonCameraPivot;
	public GameObject topDownCameraBase;
	public GameObject cameraFollow;
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

	public float timeLerp;

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


	Quaternion saveCamRot;
	Vector3 saveCamPos;
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

	//	print(_life);

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
			turnOffSettings();
		}

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
	private void LateUpdate()
	{
		if (cameraChange)
		{
			if (inTopDown)
			{
				Cam.transform.position = Vector3.Slerp(Cam.transform.position, topDownCameraBase.transform.position, Time.deltaTime * timeLerp);
				Cam.transform.rotation = Quaternion.Slerp(Cam.transform.rotation, topDownCameraBase.transform.rotation, Time.deltaTime * timeLerp);
				var distance = topDownCameraBase.transform.position - Cam.transform.position;

				if (distance.magnitude <5f)
				{
					ChangeMovement();
				}
			}
			else
			{
				Cam.transform.position = Vector3.Slerp(Cam.transform.position, thirdPersonCameraBase.transform.position, Time.deltaTime * timeLerp);
				Cam.transform.rotation = Quaternion.Slerp(Cam.transform.rotation, thirdPersonCameraBase.transform.rotation, Time.deltaTime * timeLerp);
				var distance = topDownCameraBase.transform.position - Cam.transform.position;
			
				if (distance.magnitude < 0.5f)
				{
					ChangeMovement();
				}
			
			}
		
		}
	}
	void turnOffSettings()
	{
		if (inTopDown)
		{
			//TOP DOWN	
			print("in1");
			SetCameraForTopDown();
			saveCamPos = Cam.transform.position;
			saveCamRot = Cam.transform.rotation;
			Cam.transform.parent = null;
			Cam.transform.position = saveCamPos;
			Cam.transform.rotation = saveCamRot;
			Cam.GetComponent<ThirdPersonCameraCollision>().enabled = false;
			thirdPersonCameraPivot.GetComponentInParent<ThirdPersonCameraController>().enabled = false;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			crosshair.enabled = false;		
		}
		else
		{

			SetCameraForThirdPerson();
			saveCamPos = Cam.transform.position;
			saveCamRot = Cam.transform.rotation;
			Cam.transform.parent = null;
			Cam.transform.position = saveCamPos;
			Cam.transform.rotation = saveCamRot;
			topDownController.enabled = false;
			thirdPersonController.enabled = true;
			thirdPersonCameraBase.SetActive(true);
			crosshair.enabled = true;
		}

	}
	void ChangeMovement()
	{
		if (inTopDown)
		{
			//TOP DOWN		
			thirdPersonCameraPivot.SetActive(false);
			thirdPersonController.enabled = false;
			topDownController.enabled = true;
			crosshair.enabled = false;
			cameraChange = false;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			topDownController.enabled = false;
			thirdPersonController.enabled = true;
			thirdPersonCameraPivot.SetActive(true);
			Cam.GetComponent<ThirdPersonCameraCollision>().enabled = true;
			thirdPersonCameraPivot.GetComponentInParent<ThirdPersonCameraController>().enabled = true;
			thirdPersonCameraPivot.GetComponentInParent<ThirdPersonCameraController>().setCameraAtTheBack();
			crosshair.enabled = true;
			cameraChange = false;
		}
	}
	void SetCameraForTopDown()
	{
		Cam.transform.SetParent(topDownCameraBase.transform);
		Cam.transform.rotation = topDownCameraBase.transform.rotation;
		Cam.nearClipPlane = nearClipPlaneTD;
		Cam.farClipPlane = farClipPlaneTD;
		Cam.fieldOfView = fieldOfViewTD;
	}
	void SetCameraForThirdPerson()
	{
		Cam.transform.SetParent(thirdPersonCameraBase.transform);
		Cam.transform.rotation = thirdPersonCameraBase.transform.rotation;
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
				transform.rotation = new Quaternion(transform.rotation.x, thirdPersonCameraBase.transform.rotation.y, transform.rotation.z,thirdPersonCameraBase.transform.rotation.w);
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
				transform.rotation = new Quaternion(transform.rotation.x, thirdPersonCameraBase.transform.rotation.y, transform.rotation.z, thirdPersonCameraBase.transform.rotation.w);
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
