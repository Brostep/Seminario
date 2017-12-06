using UnityEngine;

public class CameraTransitionBehaviour : MonoBehaviour
{
	public Transform target;
	public Transform cameraHolder1;
	public Transform cameraHolder2;

	private Vector3 _startPos;
	private Vector3 _endPos;
	private Vector3 _offset;

	private Quaternion _startRot;
	private Quaternion _endRot;

	public float timeLapse;

	private float _currentTime;

	public bool IsMoving { get; private set; }
	public bool IsTopDown { get; private set; }

	private void Start()
	{
		transform.position = cameraHolder1.position;
		transform.rotation = cameraHolder1.rotation;
		_offset = transform.position - target.position;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && !IsMoving)
		{
			IsMoving = !IsMoving;
			IsTopDown = !IsTopDown;
		}

		if (IsMoving)
			_currentTime += Time.deltaTime;
	}

	private void LateUpdate()
	{
		if (IsMoving)
		{
			_startPos = IsTopDown ? cameraHolder1.position : cameraHolder2.position;
			_endPos = IsTopDown ? cameraHolder2.position : cameraHolder1.position;

			_startRot = IsTopDown ? cameraHolder1.rotation : cameraHolder2.rotation;
			_endRot = IsTopDown ? cameraHolder2.rotation : cameraHolder1.rotation;

			transform.position = Vector3.Slerp(_startPos, _endPos, _currentTime / timeLapse);
			transform.rotation = Quaternion.Lerp(_startRot, _endRot, _currentTime / timeLapse);

			if (_currentTime >= timeLapse)
			{
				IsMoving = !IsMoving;
				_offset = transform.position - target.position;
				_currentTime = 0;
			}
		}

		else
			transform.position = target.position + _offset;
	}
}
