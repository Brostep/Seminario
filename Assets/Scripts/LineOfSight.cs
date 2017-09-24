using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class LineOfSight : MonoBehaviour
{
	public float sightDistance = 1f;
	[Range(0f, 360f)]
	public float sightAngle = 90f;
	public LayerMask targetLayer;
	public Transform target;
	GameObject currentTarget;
	Transform inSight;
	Camera cam;
	void Start()
	{
	//	cam = GetComponent<Camera>();
	//	target.position = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
	}

	public List<Transform> SensedObjects()
	{
		if (inSight != null)
			return new List<Transform>() { inSight };
		else
			return new List<Transform>() { };

	}

	void Update()
	{
		inSight = null; 
		if (currentTarget!=null)
			currentTarget.GetComponent<Renderer>().material.color = Color.white;
		Transform my = transform;
		Transform other = target;

		//Diferencia de posición = Dirección * distancia
		var deltaPos = other.position - my.position;

		//Angulo entre mi frente y la dirección
		var angle = Vector3.Angle(transform.forward, deltaPos);

		//El cuadrado de la distancia es más rápido de calcular
		var sqrDistance = deltaPos.sqrMagnitude;

		//Descarte previo
		//< en vez de <= por si queremos hacerlo chicato!
		if (sqrDistance < sightDistance * sightDistance && angle < sightAngle / 2f)
		{
			RaycastHit rch;
			if (Physics.Raycast(my.position, deltaPos, out rch, sightDistance))
			{
				if (Utility.LayerNumberToMask(rch.collider.gameObject.layer) == targetLayer)
				{
					inSight = other;
					rch.collider.gameObject.GetComponent<Renderer>().material.color = Color.red;
					currentTarget = rch.collider.gameObject;
				}
			}
		}
		
	}

	void OnDrawGizmos()
	{
		var p = transform.position;
		var f = transform.forward;
		Gizmos.color = Color.cyan;
		Gizmos.DrawLine(p, p + f * sightDistance);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(p, sightDistance);
		Gizmos.color = sightAngle > 180f ? Color.white : Color.yellow;
		Vector3 lastW = Vector3.zero;
		for (float a = 0; a <= 360f; a += 20f)
		{
			var v = new Vector3(
				0f,
				Mathf.Sin(Mathf.Deg2Rad * sightAngle / 2f) * sightDistance,
				Mathf.Cos(Mathf.Deg2Rad * sightAngle / 2f) * sightDistance
			);
			var w = transform.rotation * Quaternion.AngleAxis(a, Vector3.forward) * v;
			//var w = (a, transform.forward) * v;
			Gizmos.DrawLine(p, p + w);
			Gizmos.DrawLine(p + lastW, p + w);
			lastW = w;
		}
		if (inSight != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(inSight.position, 1f);
			Gizmos.DrawSphere(p + Vector3.up, 0.5f);
			Gizmos.DrawLine(p + Vector3.up, inSight.position);
		}

	}
}