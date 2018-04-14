using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullets : Bullet {

	float _timeAlive;
	Vector3 dir;
	Vector3 enemyPos;
	ThirdPersonCameraController tPCC;
	bool lockedOnTarget;
    GameObject objParticle;

    void Start()
    {
        objParticle = ParticleManager.Instance.GetParticle(ParticleManager.BULLET_NULL_PARTICLE);
        ParticleManager.Instance.DisposePool(objParticle);
    }

	void Update()
	{
		_timeAlive += Time.deltaTime;
		if (_timeAlive >= lifeSpan)
			BulletsSpawner.Instance.ReturnBulletToPool(this);

        /*if (objParticle.GetComponent<ParticleSystem>().isStopped)
            ParticleManager.Instance.ReturnParticle(ParticleManager.BULLET_NULL_PARTICLE, objParticle);*/
	}
	void FixedUpdate()
	{
	
		if (lockedOnTarget)
			transform.position += dir * speed * Time.deltaTime;
		else
			transform.position += dir * speed * Time.deltaTime;
			
	}
	void OnCollisionEnter(Collision c)
	{
        if (c.gameObject.layer == 11 || c.gameObject.layer == 12 || c.gameObject.layer == 13 || c.gameObject.layer == 18 || c.gameObject.layer == 20)
        {
            objParticle = ParticleManager.Instance.GetParticle(ParticleManager.BULLET_NULL_PARTICLE);
            objParticle.transform.position = this.transform.position;
        }

        BulletsSpawner.Instance.ReturnBulletToPool(this);
    }
	public override void Initialize()
	{
		_timeAlive = 0;
		dir = new Vector3(0f, 0f, 0f);
		enemyPos = new Vector3(0f, 0f, 0f);
		lockedOnTarget = false;

		//busca el bullet spawner y copia su direccion y rotacion y spawnea ahi.
		var bulletSpawner = FindObjectOfType<BulletsSpawner>().gameObject;
		tPCC = FindObjectOfType<ThirdPersonCameraController>();
		if (tPCC.isTargeting)
		{
			enemyPos = tPCC.nearestEnemy.transform.position;

			lockedOnTarget = true;
		}

		transform.position = FindObjectOfType<EyeBehaviour>().gameObject.transform.position;
		transform.rotation = bulletSpawner.transform.rotation;

		var player = FindObjectOfType<PlayerController>();

		if (lockedOnTarget)
			dir = (enemyPos - transform.position).normalized;
		else if (bulletSpawner.transform.rotation.x < 5f && !PlayerController.inTopDown)
		{
			var layerMask = ~(1 << 8);
			RaycastHit hit;
			if(Physics.Raycast(transform.position, tPCC.GetComponentInChildren<Camera>().transform.forward,out hit, float.MaxValue, layerMask))
			{
                dir = (hit.point - transform.position).normalized;
            }
            else
                dir = tPCC.GetComponentInChildren<Camera>().transform.forward.normalized;
        }
		else if (PlayerController.inTopDown)
			dir = player.transform.forward.normalized;
	}

}
