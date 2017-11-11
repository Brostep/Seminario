using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBoundaries : MonoBehaviour
{
    public float displacementForce;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            collision.rigidbody.AddForce(Vector3.back * displacementForce * 100);
        }
    }
}
