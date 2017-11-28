using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {

	public GameObject slowArea;
	public List<GameObject> vomitLeft;
	public List<GameObject> vomitRight;
	public int life = 100;
	int currentLife;
	public BulletHellSpawner bulletHellSpawner;

	void Start()
	{
		currentLife = life;
	}
	void Update ()
	{
		if (currentLife != life)
		{
			currentLife = life;
			checkBossLife();
		}		
	}
	void checkBossLife()
	{
		switch (life)
		{
			case 75:
				enableVomit();
				changeBulletPatternTo(2);
				break;
		}
	}
	void enableVomit()
	{
		slowArea.SetActive(true);
		StartCoroutine(VomitingLeft());
		StartCoroutine(VomitingRight());
	}
	void changeBulletPatternTo(int pattern)
	{
		bulletHellSpawner.pattern = pattern;
	}
	IEnumerator VomitingLeft()
	{
		for (int i = 0; i < vomitLeft.Count; i++)
		{
			vomitLeft[i].SetActive(true);
			yield return new WaitForSeconds(0.07f);
		}

	}
	IEnumerator VomitingRight()
	{
		for (int i = 0; i < vomitRight.Count; i++)
		{
			vomitRight[i].SetActive(true);
			yield return new WaitForSeconds(0.07f);
		}
	}
	void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.layer == 9)
		{
			life--;
			print(life);
		}
		
	}
}
