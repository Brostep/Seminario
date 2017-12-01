using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : MonoBehaviour {

	float ms;
	float dashMs;
	public float damage;

	private void Start()
	{
		ms = FindObjectOfType<ThirdPersonController>().movementSpeed;
		dashMs = FindObjectOfType<ThirdPersonController>().dashSpeed;
	}
	private void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.layer == 8)
		{
			var tPC = c.gameObject.GetComponent<ThirdPersonController>();
			var tPM = c.gameObject.GetComponent<TopDownMovement>();
			tPC.movementSpeed = ((ms / 2) - 0.5f);
			tPM.movementSpeed = ((ms / 2) - 0.5f);
			tPC.dashSpeed = ((tPC.dashSpeed / 2) - 0.5f);
			tPM.dashSpeed = ((tPM.dashSpeed / 2) - 0.5f);
		}
	}
	private void OnTriggerStay(Collider c)
	{
		if (c.gameObject.layer == 8)
		{
			c.GetComponent<PlayerController>().TakeDamage(0.05f);

		}
	}
	private void OnTriggerExit(Collider c)
	{
		if (c.gameObject.layer == 8)
		{
			c.gameObject.GetComponent<ThirdPersonController>().movementSpeed = ms;
			c.gameObject.GetComponent<TopDownMovement>().movementSpeed = ms;
			c.gameObject.GetComponent<ThirdPersonController>().dashSpeed = dashMs;
			c.gameObject.GetComponent<TopDownMovement>().dashSpeed = dashMs;

		}
	}
}
