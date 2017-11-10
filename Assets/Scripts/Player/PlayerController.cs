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
	int runHash;
	int onAttack1Hash;
	int onAttack2Hash;
	int onAttack3Hash;
	int onHeavyAttackHash;

	float timeBetweenAttacks;
	bool isJumping;
	bool onGround;
	public bool cameraChange;
	public float boop;
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

		runHash = Animator.StringToHash("Run");
		runHash = Animator.StringToHash("Run");
		onAttack1Hash = Animator.StringToHash("OnAttack1");
		onAttack2Hash = Animator.StringToHash("OnAttack2");
		onAttack3Hash = Animator.StringToHash("OnAttack3");
		onHeavyAttackHash = Animator.StringToHash("OnHeavyAttack");
	}
	void Update()
	{
		CheckGroundStatus();

		//print(_life);

		timeBetweenAttacks += Time.deltaTime;
		if (timeBetweenAttacks > 0.75f)
		{
			anim.SetBool(onAttack3Hash, false);
			anim.SetBool(onAttack2Hash, false);
			anim.SetBool(onAttack1Hash, false);
		}

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
	public void SetCameraForTopDown()
	{
		Cam.transform.SetParent(topDownCamera.transform);
		Cam.transform.rotation = topDownCamera.transform.rotation;
		Cam.nearClipPlane = nearClipPlaneTD;
		Cam.farClipPlane = farClipPlaneTD;
		Cam.fieldOfView = fieldOfViewTD;
	}
	public void SetCameraForThirdPerson()
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
		if (Input.GetKeyDown(KeyCode.E) || Input.GetButton("XButton"))
		{
			//print ("ESTOY ATACANDO");
			EnterAnimationAttack(true);
			anim.SetFloat("timeBetweenAttacks", 0f);
			timeBetweenAttacks = 0f;
			if (!inTopDown)
				transform.rotation = new Quaternion(transform.rotation.x, thirdPersonCamera.transform.rotation.y, transform.rotation.z, thirdPersonCamera.transform.rotation.w);
			var enemiesHited = Physics.OverlapSphere(melee.position, meleeRadius, LayerMask.GetMask("Enemy"));
			if (enemiesHited.Length > 0)
			{
				foreach (var enemy in enemiesHited)
				{
					enemy.GetComponent<Enemy>().life -= lightAttackDamage;
					if (enemy.GetComponent<Rigidbody>()!=null)
						enemy.GetComponent<Rigidbody>().AddForce(transform.forward.normalized * boop);
				}
					
			}
		}
		if (Input.GetKeyDown(KeyCode.F) || Input.GetButton("YButton"))
		{
			anim.SetBool(onHeavyAttackHash, true);

			if (!inTopDown)
				transform.rotation = new Quaternion(transform.rotation.x, thirdPersonCamera.transform.rotation.y, transform.rotation.z, thirdPersonCamera.transform.rotation.w);
			var enemiesHited = Physics.OverlapSphere(melee.position, meleeRadius, LayerMask.GetMask("Enemy"));
			if (enemiesHited.Length > 0)
			{
				foreach (var enemy in enemiesHited)
				{
					enemy.GetComponent<Enemy>().life -= heavyAttackDamage;
					if (enemy.GetComponent<Rigidbody>() != null)	
						enemy.GetComponent<Rigidbody>().AddForce(transform.forward.normalized * (boop*1.5f));
				}
				
			}
		}
	}

	void EnterAnimationAttack(bool isPlay)
	{
		if (isPlay)
		{
			if (!anim.GetBool(onAttack1Hash) && timeBetweenAttacks<1.5f)
			{
				anim.SetBool(onAttack1Hash, true);
			}
			else if (anim.GetBool(onAttack1Hash) && !anim.GetBool(onAttack2Hash) && timeBetweenAttacks < 1.5f)
			{
				anim.SetBool(onAttack2Hash, true);
			}
			else if (anim.GetBool(onAttack2Hash) && anim.GetBool(onAttack1Hash) && timeBetweenAttacks < 1.5f)
			{
				anim.SetBool(onAttack3Hash, true);
			}
		}
	}

	void EndAttack1(AnimationEvent e)
	{
		if (timeBetweenAttacks > 1.5f)
			anim.SetBool(onAttack1Hash, false);
	}

	void EndAttack2(AnimationEvent e)
	{
		if (timeBetweenAttacks > 1.5f)
		{
			anim.SetBool(onAttack2Hash, false);
			anim.SetBool(onAttack1Hash, false);
		}
		
	}
	void EndAttack3(AnimationEvent e)
	{
		if (timeBetweenAttacks > 1.5f)
		{
			anim.SetBool(onAttack3Hash, false);
			anim.SetBool(onAttack2Hash, false);
			anim.SetBool(onAttack1Hash, false);
		}
	}

	void EndHeavyAttack(AnimationEvent e)
	{
		anim.SetBool(onHeavyAttackHash, false);
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
