using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPooleable<T>  {

	void InitializePool(T obj);
	
	void DisposePool(T obj);
}
