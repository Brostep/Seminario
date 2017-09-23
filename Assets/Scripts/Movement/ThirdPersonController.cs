using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonController : MonoBehaviour {

	Rigidbody rb;
	Vector3 camForward;
	Vector3 relativeMove;
	Vector3 groundNormal;
	//public float dashDistance;
	public Transform thirdPersonCam;
	float horizontalInput;
	float verticalInput;
	float turnAmount;
	float forwardAmount;
	float dashTimer;
	float dashDurationAux;
	public float movementSpeed;
	public float dashCd;
	public float dashSpeed;
	public float dashDuration;
	public float movingTurnSpeed = 360;
	public float stationaryTurnSpeed = 180;

	bool onePress;
	bool onGround;
	bool isDashing;
	Animator anim;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		dashDurationAux = dashDuration;
	}

	void FixedUpdate()
	{
		GetInputs();
		CalculateMoveDir();	
	}
	// read inputs
	void GetInputs()
	{
		horizontalInput = Input.GetAxis("Horizontal");
		verticalInput = Input.GetAxis("Vertical");
	}
	// calculate move direction
	void CalculateMoveDir()
	{	
		if (thirdPersonCam != null)
		{
			// calculate camera relative direction to move:
			camForward = Vector3.Scale(thirdPersonCam.forward, new Vector3(1, 0, 1)).normalized;
			relativeMove = verticalInput * camForward + horizontalInput * thirdPersonCam.right;
		}
		else
		{
			//use world directions 
			relativeMove = verticalInput * Vector3.forward + horizontalInput * Vector3.right;
		}
		//move towards Dir
		Move(relativeMove);
	}
	void Move(Vector3 relMove)
	{
		if (relMove.magnitude > 1f)
			relMove.Normalize();

		// si esta dasheando tiene otro velocity.
		var velocity = GetVelocity(relMove);
		
		// chekea si esta en el piso, no aplica gravedad
		if (onGround)
			velocity.y = 0f;
		else
			velocity.y = Physics.gravity.y;

		// aplico movimiento
		rb.velocity = velocity;
		//animaciones mal hechas jaja
		if (horizontalInput > 0 || verticalInput > 0 || horizontalInput < 0 || verticalInput < 0)
		{
			anim.SetBool("Run", true);
			anim.SetBool("Was Running", true);
		}
		else if (anim.GetBool("Was Running"))
		{
			anim.SetBool("Was Running", false);
			anim.Play("Run To Stop");
		}
		else
		{
			anim.SetBool("Run", false);
		}
			
		//rotation 
		relMove = transform.InverseTransformDirection(relMove);
		CheckGroundStatus();
		relMove = Vector3.ProjectOnPlane(relMove, groundNormal);
		turnAmount = Mathf.Atan2(relMove.x, relMove.z);
		forwardAmount = relMove.z;
		ApplyExtraTurnRotation();
	}

	Vector3 GetVelocity(Vector3 relMove)
	{
		Vector3 relVel;
		dashTimer += Time.deltaTime;
		isDashing = false;

		// bool para que tengas que soltar el trigger despues de cada dash
		if (Input.GetAxis("RTrigger") == 0 && !onePress)
			onePress = true;
	
		// mientras que mantega apretado el input del dash, si no esta en cd y si no cumplio la duracion del dash
		if (Input.GetKey(KeyCode.E) || Input.GetAxis("RTrigger") < 0 && dashTimer > dashCd && dashDuration > 0f && onePress)
			isDashing = true; // estoy dasheando

		// estoy dasheando ? y todavia hay duracion
		if (isDashing && dashDuration > 0f)
		{
			relVel = relMove * dashSpeed;
			dashDuration -= Time.deltaTime;
			return relVel;
		} // si solte el botton o me quede si duracion reseteo el dash.
		else if (!isDashing && dashDuration < dashDurationAux)
		{
			dashDuration = dashDurationAux;
			dashTimer = 0f;
			onePress = false;
		}
		// sino.. retorno la velocidad normal del player
		return relVel = relMove * movementSpeed;
	}

	void CheckGroundStatus()
	{
		RaycastHit hitInfo;
		if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, float.MaxValue))
		{
			if (hitInfo.distance < 1f)
			{
				groundNormal = hitInfo.normal;
				onGround = true;
			}
			else
			{
				groundNormal = Vector3.up;
				onGround = false;
			}
		}
	}
	// mas velocidad a la rotacion
	void ApplyExtraTurnRotation()
	{
		float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
		transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
	}


}
