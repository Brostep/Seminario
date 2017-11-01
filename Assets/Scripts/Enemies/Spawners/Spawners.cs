using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawners : Enemy {

	[HideInInspector]
	public List<GameObject> spawners;
	public GameObject particlesWhileInGround;
	public float particlesSpeed;
	public LayerMask LayerSpawner;
	GameManager gm;
	GameObject currentParticle;
	Renderer render;
	int waves = 1;
	float _life;
	int currentSpawn;
	bool alive = true;
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
		if (life <= (_life - (waves * 12))&&alive)
		{
			GetComponent<MeshRenderer>().enabled = false;
			GetComponent<Renderer>().enabled = false;
			GetComponent<Collider>().enabled = false;
			currentParticle = Instantiate(particlesWhileInGround, transform.position - offset, transform.rotation);
			dir = (spawners[gm.cantSpawners + gm.spawn].transform.position - transform.position).normalized;
			StartCoroutine(moveParticles(dir));
			currentSpawn = gm.cantSpawners + gm.spawn;
			waves++;
			gm.spawn++;
		}
		if (life <= 0 && alive)
		{
			alive = false;
			gm.spawnersAlive--;
			if (currentParticle != null)
				Destroy(currentParticle);

			Destroy(gameObject);	
		}
	}
	IEnumerator moveParticles(Vector3 dir)
	{
		while (true)
		{
			if (!currentParticle.GetComponent<ParticlesArrived>().arrived)
			{
				currentParticle.transform.position += dir * particlesSpeed * Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			else
			{
				Destroy(currentParticle);
				transform.position = spawners[currentSpawn].transform.position;
				GetComponent<MeshRenderer>().enabled = true;
				GetComponent<Renderer>().enabled = true;
				GetComponent<Collider>().enabled = true;
				break;
			}
		}
	}
}
