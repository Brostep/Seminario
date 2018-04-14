using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(ThirdPersonController))]
[RequireComponent(typeof(TopDownMovement))]
public class PlayerController : MonoBehaviour
{
	public Camera Cam;
	ThirdPersonController thirdPersonController;
	TopDownMovement topDownController;
	AnimationControllerPlayer animController;
	public GameObject thirdPersonCamera;
	public GameObject topDownCamera;

	public static bool inTopDown;
	public Transform meleeFront;
	public Transform playerResetBoss;
	public Transform playerResetRoom;
    public Transform spawnGroundParticles;
	public float meleeRadius;
	public float lightAttackDamage;
	public float heavyAttackDamage;
	Animator anim;
    
    List<ParticleSystem> particles;
    public GameObject groundParticles;

    TrailRenderer trail;
    GameObject objParticle;

    int runHash;
    int jumpHash;
    int deathHash;

	bool isJumping;
	bool onGround;
	bool playDeathAnim;
	public bool promedyTarget;
	public bool cameraChange { get; set; }
	public bool deathBySnuSnu;
	public bool inBossFight;
	public float boop;
	Vector3 velocity;
	public GameObject bloodHit;
	Rigidbody rb;
	public Image crosshair;
	public Image lifeBar;
	private float _currentLife;
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
		animController = GetComponent<AnimationControllerPlayer>();
		rb = GetComponent<Rigidbody>();
		ChangeMovement();
		movementSpeed = thirdPersonController.movementSpeed;

        particles = new List<ParticleSystem>();

        trail = GetComponentInChildren<TrailRenderer>();

        GetComponentsInChildren(false, particles);

        objParticle = ParticleManager.Instance.GetParticle(ParticleManager.GROUND_CRACKS);
        ParticleManager.Instance.DisposePool(objParticle);

        runHash = Animator.StringToHash("Run");
        jumpHash = Animator.StringToHash("OnJump");
        deathHash = Animator.StringToHash("Death");
    }
	void Update()
	{
		CheckGroundStatus();

        if (!objParticle.GetComponent<ParticleSystem>().isPlaying)
            ParticleManager.Instance.ReturnParticle(ParticleManager.GROUND_CRACKS, objParticle);
            


        if (deathBySnuSnu && life > 0)
		{
			TakeDamage(1);
		}
		if (deathBySnuSnu || life <= 0 && !playDeathAnim)
		{
			playDeathAnim = true;
			anim.SetBool(deathHash, true);
		}

		if (onGround)
		{
			velocity.y = 0f;
			isJumping = false;
			//	anim.SetBool("OnJump", false);
		}
		else
			velocity.y = velocity.y - 6f;

		if (Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("LButton"))
		{
			inTopDown = !inTopDown;
			cameraChange = true;

			if (promedyTarget && !inTopDown)
				topDownCamera.GetComponent<TopDownPromedyTargets>().enabled = false;

			else if (promedyTarget)
				topDownCamera.GetComponent<TopDownPromedyTargets>().enabled = true;
		}

		if (cameraChange)
			ChangeMovement();
		// chekea si esta en el piso, no aplica gravedad
		if ((Input.GetKeyDown(KeyCode.Space) || (Input.GetButton("AButton"))) && !isJumping)
		{
			isJumping = true;
			//anim.SetBool(jumpHash, true);
            anim.SetTrigger("OnJump");
			velocity.y = 600f;
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
			thirdPersonCamera.GetComponentInParent<ThirdPersonCameraController>().SetCameraAtTheBack();
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
		if (Input.GetMouseButtonDown(0) || Input.GetButton("XButton"))
		{
			if (!thirdPersonController.isDashing)
			{
                trail.gameObject.SetActive(true);
				animController.EnterAnimationLightAttack();
				thirdPersonController.movementSpeed = 2.0f;
			}
		
		}
		if ((Input.GetMouseButtonUp(1) && animController.canUseHeavyAttack) || Input.GetButton("YButton"))
		{
			if (!thirdPersonController.isDashing)
			{
                trail.gameObject.SetActive(true);
                animController.EnterAnimationHeavyAttack();
				thirdPersonController.movementSpeed = 0.0f;

			}

		}
	}

	void DoDamageLightAttack1()
	{
		if (!inTopDown)
			transform.rotation = new Quaternion(transform.rotation.x, thirdPersonCamera.transform.rotation.y, transform.rotation.z, thirdPersonCamera.transform.rotation.w);
		var enemiesHited = Physics.OverlapSphere(meleeFront.position, meleeRadius, LayerMask.GetMask("Enemy"));
		if (enemiesHited.Length > 0)
		{
			foreach (var enemy in enemiesHited)
			{
				var e = enemy.GetComponent<Enemy>();
				e.life -= lightAttackDamage;
				Instantiate(bloodHit, e.head.transform);
				if (enemy.GetComponent<Rigidbody>() != null)
					enemy.GetComponent<Rigidbody>().AddForce(transform.forward.normalized * boop);

			}
		}
	}

	void DoDamageLightAttack2()
	{
		if (!inTopDown)
			transform.rotation = new Quaternion(transform.rotation.x, thirdPersonCamera.transform.rotation.y, transform.rotation.z, thirdPersonCamera.transform.rotation.w);
		var enemiesHited = Physics.OverlapSphere(transform.position, meleeRadius * 2, LayerMask.GetMask("Enemy"));
		if (enemiesHited.Length > 0)
		{
			foreach (var enemy in enemiesHited)
			{
				var e = enemy.GetComponent<Enemy>();
				e.life -= lightAttackDamage;
				Instantiate(bloodHit, e.head.transform);
				if (enemy.GetComponent<Rigidbody>() != null)
					enemy.GetComponent<Rigidbody>().AddForce(transform.forward.normalized * boop);
			}

		}
	}

	void DoDamageLightAttack3()
	{
        if (!inTopDown)
			transform.rotation = new Quaternion(transform.rotation.x, thirdPersonCamera.transform.rotation.y, transform.rotation.z, thirdPersonCamera.transform.rotation.w);
		var enemiesHited = Physics.OverlapSphere(meleeFront.position, meleeRadius, LayerMask.GetMask("Enemy"));
		if (enemiesHited.Length > 0)
		{
			foreach (var enemy in enemiesHited)
			{
				var e = enemy.GetComponent<Enemy>();
				e.life -= lightAttackDamage;
				Instantiate(bloodHit, e.head.transform);
				if (enemy.GetComponent<Rigidbody>() != null)
					enemy.GetComponent<Rigidbody>().AddForce(transform.forward.normalized * boop);
			}

		}
	}

    void RotatePlayerAtHeavyAttack()
    {
        if (!inTopDown)
            transform.rotation = new Quaternion(transform.rotation.x, thirdPersonCamera.transform.rotation.y, transform.rotation.z, thirdPersonCamera.transform.rotation.w);
    }

	private void DoDamageHeavyAttack1()
	{
        //Fixear esto. Buscar la forma de como hacer que desactive su respectiva instancia.
        ParticleManager.Instance.InitializePool(objParticle);

        objParticle.transform.position = spawnGroundParticles.position;
        objParticle.transform.rotation = spawnGroundParticles.rotation;

        /* VIEJO: Recordatorio de como era antes (no perfomante).
        var obj = ParticleManager.Instance.GetParticle(ParticleManager.GROUND_CRACKS);
        obj.transform.position = spawnGroundParticles.position;
        obj.transform.rotation = spawnGroundParticles.rotation;*/

        /*if (!inTopDown)
			transform.rotation = new Quaternion(transform.rotation.x, thirdPersonCamera.transform.rotation.y, transform.rotation.z, thirdPersonCamera.transform.rotation.w);*/
        var enemiesHited = Physics.OverlapSphere(meleeFront.position, meleeRadius, LayerMask.GetMask("Enemy"));
		if (enemiesHited.Length > 0)
		{
			foreach (var enemy in enemiesHited)
			{
				var e = enemy.GetComponent<Enemy>();
				e.life -= heavyAttackDamage;

				Instantiate(bloodHit, e.head.transform);

				if (enemy.GetComponent<Rigidbody>() != null)
					enemy.GetComponent<Rigidbody>().AddForce(transform.forward.normalized * boop * 1.5f);
			}

		}
	}

	public void TakeDamage(float damage)
	{
		life -= damage;
		damage = damage / 100;

		lifeBar.fillAmount -= damage;

        for (int i = 0; i < particles.Count - 1; i++)
        {
            if (particles[i].name == "BloodPlayerHitEffect")
                particles[i].Play();
        }

    }

	private void EndJump()
	{
		//anim.SetBool(jumpHash, false);
		isJumping = false;
	}

	private void EndDeath()
	{
		//anim.SetBool(jumpHash, false);
		anim.SetBool("Run", false);
		anim.SetBool("OnAttack1", false);
		anim.SetBool("OnAttack2", false);
		anim.SetBool("OnAttack3", false);
		anim.SetBool("OnHeavyAttack", false);
		anim.SetBool("OnDash", false);
		anim.SetBool(deathHash, false);
		anim.SetBool("Alive", true);
		life = 100;
		lifeBar.fillAmount = 100;
		playDeathAnim = false;
		deathBySnuSnu = false;

		if (inBossFight)
			ResetBoss();

		else
			ResetRoom();
	}

	private void ResetBoss()
	{
		transform.position = playerResetBoss.position;
	}

	private void ResetRoom()
	{
		transform.position = playerResetRoom.position;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;

		if (Input.GetKey(KeyCode.E))
			Gizmos.DrawSphere(meleeFront.position, meleeRadius);
	}
}
