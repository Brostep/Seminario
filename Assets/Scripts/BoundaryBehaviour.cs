using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryBehaviour : MonoBehaviour
{
    [SerializeField]
    private float displacementForce;

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.layer == 8)
        {
            c.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.back * displacementForce * 50);
        }
    }
}
