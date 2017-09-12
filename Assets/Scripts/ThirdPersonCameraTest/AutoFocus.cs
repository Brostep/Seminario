using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFocus : MonoBehaviour
{
	public Transform target;            
	public float movementSpeed;


	void FixedUpdate()
	{
		FollowTarget();
	}

	void FollowTarget()
	{
		if (!(Time.deltaTime > 0) || target == null)
			return;

//		var targetForward = target.forward;
//		var targetUp = target.up;
		
/*		var currentFlatAngle = Mathf.Atan2(targetForward.x, targetForward.z) * Mathf.Rad2Deg;

		if (spinTurnLimit > 0)
		{
			var targetSpinSpeed = Mathf.Abs(Mathf.DeltaAngle(lastFlatAngle, currentFlatAngle)) / Time.deltaTime;
			var desiredTurnAmount = Mathf.InverseLerp(spinTurnLimit, spinTurnLimit * 0.75f, targetSpinSpeed);
			var turnReactSpeed = (currentTurnAmount > desiredTurnAmount ? .1f : 1f);
			currentTurnAmount = Mathf.SmoothDamp(currentTurnAmount, desiredTurnAmount, ref turnSpeedVelocity, turnReactSpeed);
		}
		else
		{
			currentTurnAmount = 1;
		}
		*/
	//	lastFlatAngle = currentFlatAngle;
		
		transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * movementSpeed);

	//	var rollRotation = Quaternion.LookRotation(targetForward, rollUp);

	//	rollUp = rollSpeed > 0 ? Vector3.Slerp(rollUp, targetUp, rollSpeed * Time.deltaTime) : Vector3.up;
	//	transform.rotation = Quaternion.Lerp(transform.rotation, rollRotation, turnSpeed * currentTurnAmount * Time.deltaTime);
	}
}