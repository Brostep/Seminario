using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour {

	Rigidbody rb;
    public float movementSpeed;
	public Camera cam;
	Vector3 lookPos;
	Quaternion _rot;
	Vector3 aux;
	float horizontalInput;
	float verticalInput;
	Animator anim;
	float dashTimer;
	float dashDurationAux;
	public float dashCd;
	public float dashSpeed;
	public float dashDuration;

	bool onePress;
	bool isDashing;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		dashDurationAux = dashDuration;
	}
	void Update()
	{
		if (new Vector2(Input.GetAxis("RightStickHorizontal"), Input.GetAxis("RightStickVertical")) != Vector2.zero)
			joystickRotation();
		else if (aux!=Input.mousePosition)
			mouseRotation();

		aux = Input.mousePosition;

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

	}
	
	void FixedUpdate()
	{
		horizontalInput = Input.GetAxis("Horizontal");
		verticalInput = Input.GetAxis("Vertical");
		Vector3 inputMovement = new Vector3(horizontalInput, 0, verticalInput);
		var velocity = GetVelocity(inputMovement);
        rb.velocity = velocity;
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
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetAxis("RTrigger") < 0 && dashTimer > dashCd && dashDuration > 0f && onePress)
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

	void joystickRotation()
	{
		float _angle = Mathf.Atan2(Input.GetAxis("RightStickHorizontal"),-Input.GetAxis("RightStickVertical")) * Mathf.Rad2Deg;
		if (new Vector2(Input.GetAxis("RightStickHorizontal"), Input.GetAxis("RightStickVertical")) != Vector2.zero)
			_rot = Quaternion.AngleAxis(_angle, new Vector3(0, 1, 0));
		transform.rotation = Quaternion.Lerp(transform.rotation,_rot,15*Time.deltaTime);
	}
	void mouseRotation()
	{
		Ray rayCam = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(rayCam, out hit, 100))
			lookPos = hit.point;

		Vector3 lookDir = lookPos - transform.position;
		lookDir.y = 0;
		transform.LookAt(transform.position + lookDir, Vector3.up);
	}
}
