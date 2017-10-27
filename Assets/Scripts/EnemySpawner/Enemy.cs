using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IPooleable<Enemy> {

	public float life;
	
	public void DisposePool(Enemy enemyObj)
	{
		enemyObj.gameObject.SetActive(false);
	}

	public virtual void Initialize() { }

	public void InitializePool(Enemy enemyObj)
	{
		enemyObj.gameObject.SetActive(true);
		enemyObj.Initialize();
	}
}
