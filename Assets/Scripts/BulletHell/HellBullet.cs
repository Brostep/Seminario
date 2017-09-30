using UnityEngine;

public class HellBullet : MonoBehaviour
{
    public float movementSpeed;

    public float lifeTime;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += transform.forward * movementSpeed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            Destroy(gameObject);
        }
    }
}
