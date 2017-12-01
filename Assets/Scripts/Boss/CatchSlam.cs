using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchSlam : MonoBehaviour {

	Animator anim;
	public bool alreadySet;
	public GameObject pj;
	public Vector3 palmPos;
	private void Start()
	{
		anim = GetComponentInParent<Animator>();
	}
	private void Update()
	{
		palmPos = transform.position;
	}
	private void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.layer == 8 && anim.GetBool("SlamAttack") && !alreadySet)
		{
			setPlayerTransform(c.gameObject);
			alreadySet = true;
		}
	}
	void setPlayerTransform(GameObject player)
	{
		pj = player;
		player.transform.SetParent(this.transform);
		//pj.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		pj.transform.position = transform.parent.position;
	}

	
	
}
