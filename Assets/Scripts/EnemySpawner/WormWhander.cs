using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormWhander : Enemy {

	GameManager gm;
	private void Start()
	{
		gm = FindObjectOfType<GameManager>();
	}
	void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.layer == 9)
			life -= c.gameObject.GetComponent<Bullet>().damage;
	}
	private void Update()
	{
		if (life <= 0)
		{
			gm.enemiesDead++;
			Destroy(this.gameObject);
		}
	}
}
