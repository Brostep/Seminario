using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawners : Enemy {

	public GameObject particlesWhileInGround;
	public float particlesSpeed;
	public LayerMask LayerSpawner;
	public GameObject wormPrefab;
	GameManager gm;
	GameObject currentParticle;
	Renderer render;
	Animator anim;
	int waves = 1;
	float _life;
	int totalSpawners;
	bool alive = true;
	bool onGround = false;
	bool startMoving = false;
	Vector3 dir;
	Vector3 offset = new Vector3(0f, 2.65f, 0f);
	private void Start()
	{
		_life = life;
		gm = FindObjectOfType<GameManager>();
		anim = GetComponent<Animator>();

	}
	void Update()
	{
		CheckSpawnerLife();
		moveParticles();
	}
	void CheckSpawnerLife()
	{
		if (life <= 0 && alive)
		{
			alive = false;
			gm.spawnersAlive--;
			totalSpawners = 0;

			if (currentParticle != null)
				Destroy(currentParticle.gameObject);

			Destroy(gameObject);
		}

		if (life <= (_life - (waves * 10))&&alive)
		{
			anim.SetBool("Dig", true);
			GetComponent<CapsuleCollider>().enabled = false;
			totalSpawners = 0;
			onGround = false;
		
			do
			{
				if (totalSpawners < gm.spawners.Count)
				{
					if (gm.spawners[totalSpawners].GetComponent<Spawner>().open && totalSpawners != gm.currentSpawn)
					{
						dir = (gm.spawners[totalSpawners].transform.position - transform.position).normalized;

						if (dir != new Vector3(0f, 0f, 0f)&& dir.y<0.5f)
						{
							gm.spawners[totalSpawners].GetComponent<Spawner>().open = false;
							gm.spawners[gm.currentSpawn].GetComponent<Spawner>().open = true;

							gm.currentSpawn = totalSpawners;
							waves++;
							startMoving = true;
							onGround = true;
						}
					}
					else
					{
						totalSpawners++;
					}

				}
				else
				{
					throw new System.Exception("error con los spawn location ninguna esta libre");
				}

			} while (!onGround);		
		}	
	}
	void moveParticles()
	{
		if (startMoving&&currentParticle!=null)
		{
			if (!currentParticle.GetComponent<ParticlesArrived>().arrived && alive)
			{
				currentParticle.transform.position += dir * particlesSpeed * Time.deltaTime;
			}
			else if (!alive && !currentParticle.GetComponent<ParticlesArrived>().arrived)
			{
				Destroy(currentParticle.gameObject);
			}
			else
			{
				Destroy(currentParticle.gameObject);
				for (int i = 0; i < 3; i++)
				{
					//var randPosX = Random.Range(gm.spawners[gm.currentSpawn].transform.position.x - 3, gm.spawners[gm.currentSpawn].transform.position.x + 3);
					//var randPosZ = Random.Range(gm.spawners[gm.currentSpawn].transform.position.z - 3, gm.spawners[gm.currentSpawn].transform.position.z + 3);
					//var randPos = new Vector3(randPosX, 0, randPosZ);

					//Instantiate(wormPrefab, randPos, wormPrefab.transform.rotation);
				}
				transform.position = gm.spawners[gm.currentSpawn].transform.position-offset;
		
				anim.SetBool("DigOut", true);
				startMoving = false;
				GetComponent<MeshRenderer>().enabled = true;
				GetComponent<Renderer>().enabled = true;
				GetComponent<Collider>().enabled = true;
				Utility.KnuthShuffle<GameObject>(gm.spawners);
			}
		}

	}
	void EndDig()
	{
		anim.SetBool("Dig", false);
		currentParticle = Instantiate(particlesWhileInGround, transform.position-new Vector3(0f,0.3f,0f), transform.rotation);
	}
	void EndDigOut()
	{
		anim.SetBool("DigOut", false);
		GetComponent<CapsuleCollider>().enabled = true;
		transform.LookAt(gm.transform);
	}

}
