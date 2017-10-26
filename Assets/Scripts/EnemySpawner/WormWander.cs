using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormWander : Enemy {

	GameManager gm;
	private void Start()
	{
		gm = FindObjectOfType<GameManager>();
	}
	void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.layer == 9)
			life -= c.gameObject.GetComponent<PlayerBullets>().damage;
	}
	private void Update()
	{
		if (life <= 0)
		{
			gm.enemiesDead++;
			this.gameObject.SetActive(false);
		}
	}
}
