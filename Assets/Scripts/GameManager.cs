using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public GameObject activateEnemySpawners;
	public GameObject spawnerObj;
	public int cantSpawners;
	PlayerController player;
	public List<GameObject> spawners;
	public List<WormWhander> enemiesOnStart;
	[HideInInspector]
	public EnemySpawner enemySpawners;
	[HideInInspector]
	public int enemiesDead;
	bool activated;
	public int spawnersAlive;
	private void Start()
	{
		player = FindObjectOfType<PlayerController>();
		enemySpawners = activateEnemySpawners.GetComponent<EnemySpawner>();
		spawnersAlive = cantSpawners;
	}
	private void Update()
	{
		if (!activated && enemiesDead >= enemiesOnStart.Count)
		{
			activateEnemySpawners.SetActive(true);
			activated = true;
			for (int i = 0; i < cantSpawners; i++)
			{
				Instantiate(spawnerObj, spawners[i].transform.position, Quaternion.identity);
			}
			PlayerController.inTopDown = !PlayerController.inTopDown;
			player.cameraChange = true;
		}

		if (spawnersAlive <= 0)
		{
			enemySpawners.allSpawnerDeads = true;
		}
	}
}
