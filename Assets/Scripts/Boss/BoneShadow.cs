using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneShadow : MonoBehaviour {

	public Vector3 maxScale;
	[Range(0f,1f)]
	public float distanceNormalized = 0;
	float currentTime;
	public float timeLapse;

	public float lifeTime;
	float timePass;
	private void Update()
	{
		if (currentTime < timeLapse)
		{
			currentTime += Time.deltaTime;
			distanceNormalized = Mathf.Lerp(0, 1, currentTime / timeLapse);
		}
		transform.localScale = maxScale * distanceNormalized;

		timePass += Time.deltaTime;
		if (timePass > lifeTime)
			Destroy(this.gameObject);
	}
}
