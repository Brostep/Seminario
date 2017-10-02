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

    private int currentIndex;

    void Start()
    {
        currentIndex = -1;
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(0.0f, rotationAngle * Mathf.Sin(Time.time * rotationSpeed)-180, 0.0f);

        if (currentTime < timeLapse)
        {
            var t = currentTime / timeLapse;

            var index = Mathf.FloorToInt(spawnPoints.Length * curve.Evaluate(t));

            if (index != currentIndex)
            {
                Instantiate(prefab, spawnPoints[index].transform.position, spawnPoints[index].transform.rotation);

                currentIndex = index;

                // Debug.Log("Spawn Point " + (index + 1));
            }
        }

        else
        {
            currentTime = 0.0f;
        }

        currentTime += Time.deltaTime;
    }
}
