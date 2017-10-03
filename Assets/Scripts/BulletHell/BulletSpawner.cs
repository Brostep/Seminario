using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
	public AnimationCurve curve;

	public HellBullet prefab;

	public Transform[] spawnPoints;

	public float timeLapse;

	public float rotationAngle;

	public float rotationSpeed;

	private float currentTime;

	private int lastIndex;

	void Start()
	{
		lastIndex = -1;
	}

	void Update()
	{
		transform.rotation = Quaternion.Euler(0.0f, rotationAngle * Mathf.Sin(Time.time * rotationSpeed) - 180, 0.0f);

		if (currentTime < timeLapse)
		{
			var t = currentTime / timeLapse;

			var index = Mathf.FloorToInt(spawnPoints.Length * curve.Evaluate(t));

			if (index != lastIndex)
			{
				Instantiate(prefab, spawnPoints[index].transform.position, spawnPoints[index].transform.rotation);

				lastIndex = index;

				// Debug.Log(index);
			}
		}

		else
		{
			currentTime = 0.0f;
		}

		currentTime += Time.deltaTime;
	}
}
