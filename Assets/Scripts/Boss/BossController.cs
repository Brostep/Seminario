using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {

	public GameObject slowArea;
	public GameObject boneThrower;
	public List<GameObject> spawnerLocation;
	public GameObject worm;
	public List<GameObject> vomitLeft;
	public List<GameObject> vomitRight;
	public BulletHellSpawner bulletHellSpawner;
	public int life = 200;
	public int wormsPerSpawner;
	int currentLife;
	List<GameObject> spawners;
	Animator anim;
	bool canTakeDamage;
	public bool fase2Enabled;

	void Start()
	{
		currentLife = life;
		anim = GetComponent<Animator>();
		spawners = new List<GameObject>();
	}
	void Update ()
	{
		if (currentLife != life)
		{
			currentLife = life;
			CheckBossLife();
		}	
		
		if (fase2Enabled)
		{
			// check all worms alive;
		}	
	}
	void CheckBossLife()
	{
		switch (life)
		{
			case 160:
				canTakeDamage = false;
				bulletHellSpawner.startShooting = false;
				SimpleRage();
				break;
			case 120:
				bulletHellSpawner.startShooting = false;
				canTakeDamage = false;
				SimpleRage();
				break;
			case 80:
				canTakeDamage = false;
				bulletHellSpawner.startShooting = false;
				boneThrower.SetActive(false);
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
	void ChangeBulletPatternTo(int pattern)
	{
		bulletHellSpawner.startShooting = true;
		bulletHellSpawner.pattern = pattern;
	}
	void EnableFase2()
	{
		bulletHellSpawner.startShooting = false;
		anim.SetBool("Slam2Hands", true);
		fase2Enabled = true;
	}
	void spawnWorms()
	{
		for (int i = 0; i < spawners.Count; i++)
		{
			for (int j = 0; j < wormsPerSpawner; j++)
			{
				var randPosX = Random.Range(spawners[i].transform.position.x - 3, spawners[i].transform.position.x + 3);
				var randPosZ = Random.Range(spawners[i].transform.position.z - 3, spawners[i].transform.position.z + 3);
				var randPos = new Vector3(randPosX, 12.5f, randPosZ);

				Instantiate(worm, randPos, worm.transform.rotation);
			}
		}
	
	}
	public void SpawnWorm()
	{
		var i = Random.Range(0,spawners.Count-1);
		var randPosX = Random.Range(spawners[i].transform.position.x - 3, spawners[i].transform.position.x + 3);
		var randPosZ = Random.Range(spawners[i].transform.position.z - 3, spawners[i].transform.position.z + 3);
		var randPos = new Vector3(randPosX, 12.5f, randPosZ);

		Instantiate(worm, randPos, worm.transform.rotation);
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
	void EndSlam2Hands()
	{
		anim.SetBool("Slam2Hands", false);
		anim.SetBool("SimpleRage", false);
	}
	void SpawnSpawners()
	{
		StartCoroutine(CleanVomitingLeft());
		StartCoroutine(CleanVomitingRight());
		slowArea.SetActive(false);
		for (int i = 0; i < 3; i++)
		{
			spawnerLocation[i].SetActive(true);
			spawners.Add(spawnerLocation[i]);
		}
		spawnWorms();


		var playerController = FindObjectOfType<PlayerController>();
		playerController.cameraChange = true;
		PlayerController.inTopDown = !PlayerController.inTopDown;
		if (playerController.promedyTarget && !PlayerController.inTopDown)
		{
			playerController.topDownCamera.GetComponent<TopDownPromedyTargets>().enabled = false;
		}
		else if (playerController.promedyTarget)
		{
			playerController.topDownCamera.GetComponent<TopDownPromedyTargets>().enabled = true;
		}
	}
	void EndSimpleRage()
	{
		switch (life)
		{
			case 160:
				anim.SetBool("SimpleRage", false);
				EnableVomit();
				ChangeBulletPatternTo(2);
				canTakeDamage = true;
				break;
			case 120:
				anim.SetBool("SimpleRage", false);
				EnableBoneThrower();
				ChangeBulletPatternTo(3);
				canTakeDamage = true;
				break;
			case 80:
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
	IEnumerator CleanVomitingLeft()
	{
		for (int i = vomitLeft.Count-1; i >= 0; i--)
		{
			vomitLeft[i].SetActive(false);
			yield return new WaitForSeconds(0.07f);
		}
	}
	IEnumerator CleanVomitingRight()
	{
		for (int i = vomitRight.Count - 1; i >= 0; i--)
		{
			vomitRight[i].SetActive(false);
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
	void endSlam()
	{
		if (GetComponentInChildren<CatchSlam>().alreadySet)
		{
			var catchSlam = GetComponentInChildren<CatchSlam>();
			catchSlam.pj.transform.parent = null;
			catchSlam.pj.transform.position = catchSlam.palmPos;
			catchSlam.pj.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
			catchSlam.alreadySet = false;
			catchSlam.pj.GetComponent<Rigidbody>().AddForce(new Vector3(250,125, 250), ForceMode.Impulse);
		}
	}
}
