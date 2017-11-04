using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraCollision : MonoBehaviour
{
	public bool protecting { get; private set; }
	public float clipMoveTime = 0.05f;
	public float closestDistance = 0.5f;
	public float returnTime = 0.4f;
	public float sphereCastRadius = 0.1f;

	private float originalDist;
	private float modeVelocity;
	private float currentDist;

	private Ray ray = new Ray();
	private RaycastHit[] hits;
	private Transform cam;
	private Transform pivot;
	private RayHitComparer rayHitComparer;

	public LayerMask[] dontClipWith;
	int[] layerNumbers;

	void Start()
	{
		cam = GetComponentInChildren<Camera>().transform;
		pivot = cam.parent;
		originalDist = cam.localPosition.magnitude;
		currentDist = originalDist;
		rayHitComparer = new RayHitComparer();
		layerNumbers = new int[dontClipWith.Length];
		for (int i = 0; i < dontClipWith.Length; i++)
			layerNumbers[i] = Utility.LayerMaskToInt(dontClipWith[i]);
	}

	void LateUpdate()
	{
		float targetDist = originalDist;

		ray.origin = pivot.position + pivot.forward * sphereCastRadius;
		ray.direction = -pivot.forward;

		var cols = Physics.OverlapSphere(ray.origin, sphereCastRadius);

		bool initialIntersect = false;
		bool hitSomething = false;

		for (int i = 0; i < cols.Length; i++)
		{
			if ((!cols[i].isTrigger) 
				&& !(cols[i].attachedRigidbody != null
				&& cols[i].gameObject.layer == layerNumbers[0])
				&& cols[i].gameObject.layer != layerNumbers[1]
				&& cols[i].gameObject.layer != layerNumbers[2]
				&& cols[i].gameObject.layer != layerNumbers[3]
				&& cols[i].gameObject.layer != layerNumbers[4]
				&& cols[i].gameObject.layer != layerNumbers[5])
			{
				initialIntersect = true;
				break;
			}
		}
		if (initialIntersect)
		{
			ray.origin += pivot.forward * sphereCastRadius;

			hits = Physics.RaycastAll(ray, originalDist - sphereCastRadius);
		}
		else
		{
			hits = Physics.SphereCastAll(ray, sphereCastRadius, originalDist + sphereCastRadius);
		}

		Array.Sort(hits, rayHitComparer);
		float nearest = Mathf.Infinity;

		for (int i = 0; i < hits.Length; i++)
		{
			if (hits[i].distance < nearest && (!hits[i].collider.isTrigger)
					&& !(hits[i].collider.attachedRigidbody != null
					&& hits[i].collider.gameObject.layer == layerNumbers[0])
					&& hits[i].collider.gameObject.layer != layerNumbers[1]
					&& hits[i].collider.gameObject.layer != layerNumbers[2]
					&& hits[i].collider.gameObject.layer != layerNumbers[3]
					&& hits[i].collider.gameObject.layer != layerNumbers[4]
					&& hits[i].collider.gameObject.layer != layerNumbers[5])
			{
				nearest = hits[i].distance;
				targetDist = -pivot.InverseTransformPoint(hits[i].point).z;
				hitSomething = true;
			}
		}
		if (hitSomething)
		{
			Debug.DrawRay(ray.origin, -pivot.forward * (targetDist + sphereCastRadius), Color.red);
		}


		protecting = hitSomething;
		currentDist = Mathf.SmoothDamp(currentDist, targetDist, ref modeVelocity,
										 currentDist > targetDist ? clipMoveTime : returnTime);
		currentDist = Mathf.Clamp(currentDist, closestDistance, originalDist);
		cam.localPosition = -Vector3.forward * currentDist;
	}
	public class RayHitComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			return ((RaycastHit)x).distance.CompareTo(((RaycastHit)y).distance);
		}
	}
}