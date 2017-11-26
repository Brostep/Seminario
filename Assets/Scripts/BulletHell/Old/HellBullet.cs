using UnityEngine;

public class HellBullet : Bullet
{
    float _timeAlive;

    public override void Initialize()
    {
        _timeAlive = 0f;

        setTransform(BulletHellSpawner.Instance.SpawnPosition, BulletHellSpawner.Instance.SpawnRotation);
    }
    public void setTransform(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }
    void Update()
    {
        _timeAlive += Time.deltaTime;

    }

    private void FixedUpdate()
    {
        if (_timeAlive >= lifeSpan)
        {
            if (this.tag == "Indestructible")
                BulletHellSpawner.Instance.ReturnGodlikeBulletToPool(this);

            else
                BulletHellSpawner.Instance.ReturnBulletToPool(this);
        }

        else
            transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9/* && this.gameObject.layer == 14*/)
        {
            BulletHellSpawner.Instance.ReturnBulletToPool(this);
        }
    }
}
