using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraCollision : MonoBehaviour
{
	public float clipMoveTime = 0.05f;           
	public float returnTime = 0.4f;               
	public float sphereCastRadius = 0.1f;           
	public bool visualiseInEditor;                  
	public float closestDistance = 0.5f;           
	public bool protecting { get; private set; }
//	public List<LayerMask>
//	public LayerMask dontClipPlayer; 
//	public LayerMask dontClipBullet; 
//	public LayerMask dontClipEnemy;
	public string dontClipTagPlayer = "Player";
	public string dontClipTagEnemy = "Enemy";
	private Transform cam;                 
	private Transform pivot;                
	private float originalDist;             
	private float modeVelocity;           
	private float currentDist;              
	private Ray ray = new Ray();                
	private RaycastHit[] hits;            
	private RayHitComparer rayHitComparer;  

	void Start()
	{
		cam = GetComponentInChildren<Camera>().transform;
		pivot = cam.parent;
		originalDist = cam.localPosition.magnitude;
		currentDist = originalDist;
		rayHitComparer = new RayHitComparer();
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
				&& cols[i].attachedRigidbody.CompareTag(dontClipTagPlayer)))
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
					&& hits[i].collider.attachedRigidbody.CompareTag(dontClipTagPlayer)))
			{
				print("in");
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