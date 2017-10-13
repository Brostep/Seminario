using UnityEngine;

public class HellBulletSpawner : MonoBehaviour
{
    public AnimationCurve curve;

    public HellBullet hellBulletPrefab;

    public Transform[] spawnPoints;

    public float timeLapse;

    public float rotationAngleX;
    public float rotationAngleY;
    public float rotationSpeed;
    public float rotationOffset;
	[HideInInspector]
	public Vector3 spawnPosition;
	[HideInInspector]
	public Quaternion spawnRotation;
	public bool inRoom;
    private float currentTime;

    private int lastIndex;

	private Pool<HellBullet> _hellBulletPool;

	private static HellBulletSpawner _instance;
	public static HellBulletSpawner Instance { get { return _instance; } }

	void Start()
    {
		_instance = this;
		_hellBulletPool = new Pool<HellBullet>(80, BulletFactory, HellBullet.InitializeBullet, HellBullet.DisposeBullet, true);
		lastIndex = -1;
    }

    void Update()
    {
		if (inRoom)
		{
			transform.rotation = Quaternion.Euler(rotationAngleX * Mathf.Sin(Time.time * rotationSpeed), rotationAngleY * Mathf.Sin(Time.time * rotationSpeed) - rotationOffset, 0.0f);

			if (currentTime < timeLapse)
			{
				var t = currentTime / timeLapse;

				var index = Mathf.FloorToInt(spawnPoints.Length * curve.Evaluate(t));

				if (index != lastIndex)
				{
					_hellBulletPool.GetObjectFromPool();
					spawnPosition = spawnPoints[index].transform.position;
					spawnRotation = spawnPoints[index].transform.rotation;

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
	private HellBullet BulletFactory()
	{
		return Instantiate<HellBullet>(hellBulletPrefab);
	}

	public void ReturnBulletToPool(HellBullet bullet)
	{
		_hellBulletPool.DisablePoolObject(bullet);
	}
}
