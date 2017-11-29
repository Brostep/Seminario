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
	bool canTakeDamage;

	void Start()
	{
		currentLife = life;
		anim = GetComponent<Animator>();		
	}
	void Update ()
	{
		if (currentLife != life)
		{
			currentLife = life;
			CheckBossLife();
		}		
	}
	void CheckBossLife()
	{
		switch (life)
		{
			case 80:
				canTakeDamage = false;
				bulletHellSpawner.startShooting = false;
				SimpleRage();
				break;
			case 60:
				bulletHellSpawner.startShooting = false;
				canTakeDamage = false;
				SimpleRage();
				break;
			case 40:
				canTakeDamage = false;
				bulletHellSpawner.startShooting = false;
				SimpleRage();
				break;
		}
	}
	void EnableVomit()
	{
		slowArea.SetActive(true);
		StartCoroutine(VomitingLeft());
		StartCoroutine(VomitingRight());
	}
	public void BossIntro()
	{
		anim.SetBool("EatingIntro", true);
	}
	void SimpleRage()
	{
		anim.SetBool("SimpleRage", true);
	}
	void EnableBoneThrower()
	{
		boneThrower.SetActive(true);
	}
	void ChangeBulletPatternTo(int pattern)
	{
		bulletHellSpawner.startShooting = true;
		bulletHellSpawner.pattern = pattern;
	}
	void EnableFase2()
	{
		bulletHellSpawner.startShooting = false;
		anim.SetBool("Slam2Hands", true);
		//create spawners
	}
	void EndSlam()
	{
		anim.SetBool("Slam2Hands", false);
		anim.SetBool("SimpleRage", false);
	}
	void EndSimpleRage()
	{
		switch (life)
		{
			case 80:
				anim.SetBool("SimpleRage", false);
				EnableVomit();
				ChangeBulletPatternTo(2);
				canTakeDamage = true;
				break;
			case 60:
				anim.SetBool("SimpleRage", false);
				EnableBoneThrower();
				ChangeBulletPatternTo(3);
				canTakeDamage = true;
				break;
			case 40:
				EnableFase2();
				break;
		}
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
		if (c.gameObject.layer == 9 && canTakeDamage)
		{
			life--;
		}
	}
	void StartShooting()
	{
		ChangeBulletPatternTo(1);
		bulletHellSpawner.startShooting = true;
		canTakeDamage = true;
	}
}
