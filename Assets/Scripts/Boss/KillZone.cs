using UnityEngine;

public class KillZone : MonoBehaviour
{
    public Transform respawnPoint;

    BossController _bossController;

    private void Awake()
    {
        _bossController = FindObjectOfType<BossController>();
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 8)
        {
            c.gameObject.transform.position = respawnPoint.position;
            c.gameObject.transform.rotation = respawnPoint.rotation;
        }

        if (c.gameObject.layer == 10)
        {
            _bossController.wormsInScene.Remove(c.gameObject);
            Destroy(c.gameObject);
            _bossController.SpawnWorm();
        }
    }
}
