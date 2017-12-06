using UnityEngine;

public class CameraHolderBehaviour : MonoBehaviour
{
	public Transform target;

	private Vector3 _offset;

	private void Start()
	{
		_offset = transform.position - target.position;
	}

	private void FixedUpdate()
	{
		transform.position = target.position + _offset;
	}
}
