using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public AnimationCurve curve;

    public HellBullet prefab;

    public Transform[] spawnPoints;

    public float timeLapse;

    public float rotationAngle;
    public float rotationSpeed;
    public float rotationOffset;

    private float currentTime;

    private int lastIndex;

    void Start()
    {
        lastIndex = -1;
    }

    void Update()
    {
<<<<<<< HEAD
        transform.rotation = Quaternion.Euler(rotationAngle * Mathf.Sin(Time.time * rotationSpeed) - rotationOffset,0.0f, 0.0f);
=======
        transform.rotation = Quaternion.Euler(default(float), rotationAngle * Mathf.Sin(Time.time * rotationSpeed) - rotationOffset, default(float));
>>>>>>> 75b0fb2c1b361bda54e8ba78ea74e0b6aa7deb6b

        if (currentTime < timeLapse)
        {
            var t = currentTime / timeLapse;

            var index = Mathf.FloorToInt(spawnPoints.Length * curve.Evaluate(t));

            if (index != lastIndex)
            {
                Instantiate(prefab, spawnPoints[index].transform.position, spawnPoints[index].transform.rotation);

                lastIndex = index;
            }
        }

        else
        {
            currentTime = default(float);
        }

        currentTime += Time.deltaTime;
    }
}
