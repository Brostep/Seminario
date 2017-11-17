using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamAttack : MonoBehaviour {


    Animator anim;

    private void Start()
    {
        anim = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 8)
        {
            if (!anim.GetBool("SlamAttack"))
                anim.SetBool("SlamAttack",true);
        }
    }
    private void OnTriggerExit(Collider c)
    {
        if (c.gameObject.layer == 8)
        {
            if (anim.GetBool("SlamAttack"))
                anim.SetBool("SlamAttack", false);
        }
    }
}
