using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBoss : Enemy
{
	public GameObject particlesWhileInGround;
	public float particlesSpeed;
	public GameObject spawnLocation;
	List<GameObject> spawners;
	GameObject currentParticle;
	Renderer render;
	Animator anim;
	BossController boss;
    ParticleSystem ps;
	int waves = 1;
	float _life;
	bool alive = true;
	bool startMoving = false;
	Vector3 dir;
	private void Start()
	{
		_life = life;
		anim = GetComponent<Animator>();
		boss = FindObjectOfType<BossController>();
        ps = GetComponentInChildren<ParticleSystem>();
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
			if (currentParticle != null)
				Destroy(currentParticle.gameObject);
			boss.spawnersAlive--;

			Destroy(gameObject);
		}

		if (life <= (_life - (waves * 10)) && alive)
		{
			anim.SetBool("Dig", true);
			GetComponent<CapsuleCollider>().enabled = false;
			startMoving = true;
			waves++;
			dir = (spawnLocation.transform.position - transform.position).normalized;
		}
	}
	void moveParticles()
	{
		if (startMoving && currentParticle != null)
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

				transform.position = spawnLocation.transform.position;
				anim.SetBool("DigOut", true);
				startMoving = false;
				GetComponent<MeshRenderer>().enabled = true;
				GetComponent<Renderer>().enabled = true;
				GetComponent<Collider>().enabled = true;
			}
		}

	}

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.layer == 9)
            ps.Play(true);
            
    }

    void EndDig()
	{
		anim.SetBool("Dig", false);
		currentParticle = Instantiate(particlesWhileInGround, transform.position - new Vector3(0f, 0.3f, 0f), transform.rotation);
	}
	void EndDigOut()
	{
		anim.SetBool("DigOut", false);
		GetComponent<CapsuleCollider>().enabled = true;
	}

}
