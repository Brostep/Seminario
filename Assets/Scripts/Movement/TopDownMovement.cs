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
	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}
	void Update()
	{
		if (new Vector2(Input.GetAxis("RightStickHorizontal"), Input.GetAxis("RightStickVertical")) != Vector2.zero)
			joystickRotation();
		else if (aux!=Input.mousePosition)
			mouseRotation();

		aux = Input.mousePosition;
	}
	void FixedUpdate()
	{
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		Vector3 inputMovement = new Vector3(horizontal, 0,vertical);
        Vector3 tempVelocity = inputMovement * movementSpeed;
        rb.velocity = tempVelocity;
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
