using UnityEngine;
using System.Collections;
using System;

public abstract class Bullet : MonoBehaviour, IPooleable<Bullet>
{
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
