using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour {

	public static TopDownMovement instace;
	Rigidbody rb;
    public float movementSpeed;
	public Camera cam;
	Vector3 lookPos;
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		instace = this;
	}
	void Update()
	{
		Ray rayCam = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(rayCam,out hit, 100))
			lookPos = hit.point;

		Vector3 lookDir = lookPos - transform.position;
		lookDir.y = 0;
		transform.LookAt(transform.position + lookDir, Vector3.up);
	}
	void FixedUpdate()
	{
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		Vector3 inputMovement = new Vector3(horizontal, 0, vertical);
        Vector3 tempVelocity = inputMovement * movementSpeed;
        rb.velocity = tempVelocity;
	} 
}
