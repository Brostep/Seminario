using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawners : Enemy {

	public List<GameObject> spawners;
	GameManager gm;
	float waves=0;
	float _life;
	bool alive = true;
	private void Start()
	{
		_life = life;
		gm = FindObjectOfType<GameManager>();
		for (int i = 0; i < gm.spawners.Count; i++)
		{
			spawners.Add(gm.spawners[i]);
		}
		Utility.KnuthShuffle<GameObject>(spawners);
	}
	void Update()
	{
		if (life < _life-10 && waves==0)
		{
			transform.position = spawners[0].transform.position;
			waves ++;
		}
		if (life < _life-20 && waves==1)
		{
			transform.position = spawners[1].transform.position;
			waves ++;
		}
		if (life < _life-30 && waves==3)
		{
			transform.position = spawners[1].transform.position;
			waves ++;
		}
		if (life<=0 && alive)
		{
			alive = false;
			gm.spawnersAlive--;
			Destroy(gameObject);
		}
	}
}
