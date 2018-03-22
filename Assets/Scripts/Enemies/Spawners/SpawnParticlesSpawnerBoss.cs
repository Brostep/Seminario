using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticlesSpawnerBoss : MonoBehaviour {

    ParticleSystem ps;

    void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>();
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.layer == 9)
            ps.Play();
    }
}
