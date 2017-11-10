using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawners : Enemy {

	[HideInInspector]
	public List<GameObject> spawners;
	public GameObject particlesWhileInGround;
	public float particlesSpeed;
	public LayerMask LayerSpawner;
	public GameObject wormPrefab;
	GameManager gm;
	GameObject currentParticle;
	Renderer render;
	int waves = 1;
	float _life;
	int currentSpawn;
	int totalSpawners;
	bool alive = true;
	bool onGround = false;
	Vector3 dir;
	Vector3 offset = new Vector3(0f, 1.8f, 0f);
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
		CheckSpawnerLife();
	}
	void CheckSpawnerLife()
	{
		if (life <= 0 && alive)
		{
			alive = false;
			gm.spawnersAlive--;
			totalSpawners = 0;

			if (currentParticle != null)
				Destroy(currentParticle);

			Destroy(gameObject);
		}

		if (life <= (_life - (waves * 12))&&alive)
		{
			GetComponent<MeshRenderer>().enabled = false;
			GetComponent<Renderer>().enabled = false;
			GetComponent<Collider>().enabled = false;
			totalSpawners = 0;
			onGround = false;
			currentParticle = Instantiate(particlesWhileInGround, transform.position - offset, transform.rotation);
			do
			{
				if (spawners[totalSpawners].GetComponent<Spawner>().open)
				{
					dir = (spawners[totalSpawners].transform.position - transform.position).normalized;
					StartCoroutine(moveParticles(dir));
					spawners[totalSpawners].GetComponent<Spawner>().open = false;
		
					currentSpawn = totalSpawners;
					waves++;
					onGround = true;
				}
				else
				{
					totalSpawners++;
				}
			
			} while (!onGround);		
		}	
	}
	IEnumerator moveParticles(Vector3 dir)
	{
		while (true)
		{
			if (!currentParticle.GetComponent<ParticlesArrived>().arrived && alive)
			{
				currentParticle.transform.position += dir * particlesSpeed * Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			else if(!alive&&!currentParticle.GetComponent<ParticlesArrived>().arrived )
			{
				Destroy(currentParticle);
				break;
			}
			else
			{
				Destroy(currentParticle);
				for (int i = 0; i < 3; i++)
				{
					var randPosX = Random.Range(spawners[currentSpawn].transform.position.x - 3, spawners[currentSpawn].transform.position.x + 3);
					var randPosZ = Random.Range(spawners[currentSpawn].transform.position.z - 3, spawners[currentSpawn].transform.position.z + 3);
					var randPos = new Vector3(randPosX, 0, randPosZ);
					Instantiate(wormPrefab, randPos, wormPrefab.transform.rotation);
				}
				transform.position = spawners[currentSpawn].transform.position;	
				spawners[currentSpawn].GetComponent<Spawner>().open = true;
				GetComponent<MeshRenderer>().enabled = true;
				GetComponent<Renderer>().enabled = true;
				GetComponent<Collider>().enabled = true;
				Utility.KnuthShuffle<GameObject>(spawners);
				break;
			}
		}
	}
}
