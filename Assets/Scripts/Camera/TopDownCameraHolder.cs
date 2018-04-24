using UnityEngine;

public class TopDownCameraHolder : MonoBehaviour
{
    public Transform cameraTarget;
    public float smoothness; // Displacement smoothness
    public float joystickOffset;
    public Vector3 positionOffset;

    private void Start()
    {
        transform.position = cameraTarget.position + positionOffset;
    }

    private void FixedUpdate()
    {
        var horizontalInput = Input.GetAxis("RightStickHorizontal");
        var verticalInput = Input.GetAxis("RightStickVertical");
        var mouseInpuntX = (Input.mousePosition.x / Screen.width) - 0.5f;
        var mouseInpuntY = (Input.mousePosition.y / Screen.height) - 0.5f;

        Vector3 inputMovement = new Vector3((horizontalInput + mouseInpuntX) * joystickOffset, 0,
                                             -(verticalInput - mouseInpuntY) * joystickOffset);
        Vector3 desiredPosition = cameraTarget.position + positionOffset + inputMovement;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothness);

        transform.position = smoothedPosition;
    }
}
