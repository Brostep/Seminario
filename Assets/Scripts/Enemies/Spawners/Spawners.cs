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
	float waves = 0;
	float _life;
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
		if (life < _life - 10 && waves == 0)
		{
			GetComponent<MeshRenderer>().enabled = false;
			GetComponent<Renderer>().enabled = false;
			currentParticle = Instantiate(particlesWhileInGround, transform.position - offset, transform.rotation);
			dir = (spawners[0].transform.position - transform.position).normalized;
			StartCoroutine(moveParticles(dir, 0));
			waves++;
		}
		if (life < _life - 20 && waves == 1)
		{
			GetComponent<MeshRenderer>().enabled = false;
			GetComponent<Renderer>().enabled = false;
			currentParticle = Instantiate(particlesWhileInGround, transform.position - offset, transform.rotation);
			dir = (spawners[1].transform.position - transform.position).normalized;
			StartCoroutine(moveParticles(dir, 1));
			waves++;
		}
		if (life < _life - 30 && waves == 3)
		{
			GetComponent<MeshRenderer>().enabled = false;
			GetComponent<Renderer>().enabled = false;
			currentParticle = Instantiate(particlesWhileInGround, transform.position - offset, transform.rotation);
			dir = (spawners[2].transform.position - transform.position).normalized;
			StartCoroutine(moveParticles(dir, 2));
			waves++;
		}
		if (life <= 0 && alive)
		{
			alive = false;
			gm.spawnersAlive--;
			Destroy(gameObject);
		}
	}
	IEnumerator moveParticles(Vector3 dir, int index)
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
				transform.position = spawners[index].transform.position;
				GetComponent<MeshRenderer>().enabled = true;
				GetComponent<Renderer>().enabled = true;
				break;
			}
		}
	}
}
