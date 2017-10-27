using UnityEngine;
using System.Collections;
using System;

public abstract class Bullet : MonoBehaviour, IPooleable<Bullet>
{
	public float speed;
	public float lifeSpan;
	public float damage;

	public void DisposePool(Bullet obj)
	{
		obj.gameObject.SetActive(false);
	}

	public void InitializePool(Bullet obj)
	{
		obj.gameObject.SetActive(true);
		obj.Initialize();
	}
	public virtual void Initialize() { }
}
