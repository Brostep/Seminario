using System.Collections.Generic;
using UnityEngine;

public class TopDownAverageTarget : MonoBehaviour
{
    public float followUpTime;
    public float screenEdge;
    public float minimunFieldOfView;

    private Camera mainCamera;
    private List<Transform> targets;
    private float zoomSpeed;
    private Vector3 moveVelocity;

    private void Start()
    {
        targets = new List<Transform>();

        var player = FindObjectOfType<PlayerController>().gameObject;
        var boss = FindObjectOfType<BossController>().gameObject;

        targets.Add(player.transform);
        targets.Add(boss.transform);
    }

    private void FixedUpdate()
    {
        if (mainCamera == null)
        {
            mainCamera = GetComponentInChildren<Camera>();
        }

        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, AveragePosition(), ref moveVelocity, followUpTime);
            mainCamera.fieldOfView = Mathf.SmoothDamp(mainCamera.fieldOfView, FindRequiredSize(), ref zoomSpeed, followUpTime);
        }
    }

    private Vector3 AveragePosition()
    {
        Vector3 avgPosition = new Vector3();

        int targetsAmount = 0;

        for (int i = 0; i < targets.Count; i++)
        {
            if (!targets[i].gameObject.activeSelf)
            {
                continue;
            }

            avgPosition += targets[i].position;

            targetsAmount++;
        }

        if (targetsAmount > 0)
        {
            avgPosition /= targetsAmount;
        }

        avgPosition.y = transform.position.y;

        return avgPosition;
    }

    private float FindRequiredSize()
    {
        float size = 0;

        for (int i = 0; i < targets.Count; i++)
        {
            if (!targets[i].gameObject.activeSelf)
            {
                continue;
            }

            Vector3 targetLocalPosition = transform.InverseTransformPoint(targets[i].position);

            Vector3 desiredLocalPosition = transform.InverseTransformPoint(AveragePosition());
            // Find the position of the target from the desired position of the camera's local space.
            Vector3 desiredPosToTarget = targetLocalPosition - desiredLocalPosition;

            // Choose the largest out of the current size and the distance of the tank 'up' or 'down' from the camera.
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

            // Choose the largest out of the current size and the calculated size based on the tank being to the left or right of the camera.
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / mainCamera.aspect);
        }

        // Add the edge buffer to the size.
        size += screenEdge;

        // Make sure the camera's size isn't below the minimum.
        size = Mathf.Max(size, minimunFieldOfView);

        return size;
    }
}
