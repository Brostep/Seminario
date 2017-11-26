using UnityEngine;
using System.Collections;

public class Flocking : Steering
{
	public float neighborhoodRadius = 10f;
	public float separationRadius = 2f;
	public float alignmentMult = 1f;
	public float cohesionMult = 1f;
	public float separationMult = 1f;
	public bool attacking;

	public bool drawFlockingGizmos = false;
	
	Vector3 _alignment, _cohesion, _separation;

	void FixedUpdate()
	{
		if (!attacking)
		{
			ResetForces();

			var hits = Physics.OverlapSphere(transform.position, neighborhoodRadius);

			var sumV = Vector3.zero;            //Suma de velocidades
			var sumP = Vector3.zero;            //Suma de posiciones
			var sumSepForce = Vector3.zero;     //Suma de fuerzas de separación (deltas / distancia)

			int nHits = 0;
			foreach (var hit in hits)
			{
				if (hit.gameObject == gameObject)
					continue;

				if (hit.gameObject.layer == columnLayer || hit.gameObject.layer == wallLayer)
				{
					var distance = hit.transform.position - transform.position;
					var distanceMag = distance.magnitude;
					if (distanceMag < columnRadius)
						AddForce(Avoidance(distance, columnRadius));
					else if (distanceMag < wallRadius)
						AddForce(Avoidance(distance, wallRadius));
				}
				else
				{
					var other = hit.GetComponent<Steering>();
					if (other == null)
						continue;

					var deltaP = transform.position - other.position;   //from other to self
					var distSqr = deltaP.sqrMagnitude;
					if (distSqr > 0f && distSqr < separationRadius * separationRadius)
					{
						sumSepForce += deltaP / distSqr;
					}

					nHits++;
					sumV += other.velocity;
					sumP += other.position;
				}
			}

			if (nHits > 0)
			{
				_alignment = sumV.normalized * maxVelocity - velocity;      //Promedio de "direcciones"
				_cohesion = Seek(sumP / nHits);                             //Seguir promedio de posiciones
				_separation = sumSepForce == Vector3.zero ? Vector3.zero : sumSepForce.normalized * maxVelocity - velocity; //Prmoedio de fuerzas

				AddForce(_alignment * alignmentMult);
				AddForce(_cohesion * cohesionMult);
				AddForce(_separation * separationMult);
			}

			//Seek Player
			AddForce(Seek(target.position));

			ApplyForces();

		}

	}

	int f = 0;
	override protected void OnDrawGizmos()
	{
		base.OnDrawGizmos();

		if (!drawFlockingGizmos)
			return;

		if (++f % 50 == 0)
		{
			Gizmos.color = Color.gray;
			Gizmos.DrawWireSphere(transform.position, neighborhoodRadius);
		}

		Gizmos.color = Color.black;
		Gizmos.DrawLine(transform.position, transform.position + _alignment * alignmentMult);
		Gizmos.color = Color.cyan;
		Gizmos.DrawLine(transform.position, transform.position + _cohesion * cohesionMult);
		Gizmos.color = Color.magenta;
		Gizmos.DrawLine(transform.position, transform.position + _separation * separationMult);
		Gizmos.DrawWireSphere(transform.position, separationRadius);
	}
}
