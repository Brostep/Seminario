using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {

	public GameObject slowArea;
	public GameObject boneThrower;
	public List<GameObject> vomitLeft;
	public List<GameObject> vomitRight;
	public int life = 100;
	int currentLife;
	public BulletHellSpawner bulletHellSpawner;
	Animator anim; 

	void Start()
	{
		currentLife = life;
		anim = GetComponent<Animator>();		
	}
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Space)&&!anim.GetBool("EatingIntro"))
			bossIntro();

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
			case 45:
				enableBoneThrower();
				changeBulletPatternTo(3);
				break;
		}
	}
	void enableVomit()
	{
		slowArea.SetActive(true);
		StartCoroutine(VomitingLeft());
		StartCoroutine(VomitingRight());
	}
	void bossIntro()
	{
		anim.SetBool("EatingIntro", true);
	}
	void enableBoneThrower()
	{
		boneThrower.SetActive(true);
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
	void StartShooting()
	{
		changeBulletPatternTo(1);
		bulletHellSpawner.startShooting = true;
	}
}
