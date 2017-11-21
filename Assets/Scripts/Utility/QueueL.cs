using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

public class QueueL<T> : IBasicQueue<T> {
	List<T> _cont = new List<T>();
	
	public void Enqueue(T elem) {
		_cont.Add(elem);
	}

	public T Dequeue() {
		Debug.Assert(!IsEmpty());		//Precondicion: Que no este vacio
		var temp = _cont[0];
		_cont.RemoveAt(0);
		return temp;
	}

	public T Peek() {
		Debug.Assert(!IsEmpty());		//Precondicion: Que no este vacio
		return _cont[0];
	}
	public T PeekLast() {
		Debug.Assert(!IsEmpty());		//Precondicion: Que no este vacio
		return _cont[_cont.Count-1];
	}

	public bool IsEmpty() {
		return _cont.Count == 0;
	}
	public bool Has2OrMore() {
		return _cont.Count >= 2;
	}
	public int Count()
	{
		return _cont.Count;
	}

	public void RemoveLast()
	{
		Debug.Assert(!IsEmpty());       //Precondicion: Que no este vacio
		_cont.RemoveAt(_cont.Count - 1);
	}
	public IEnumerator<T> GetEnumerator() {
		return _cont.GetEnumerator();
	}

	//Esto es necesario dado que IEnumerable<T> hereda de IEnumerable (version no generica) y nos obliga a implementar la version no generica tambien
	IEnumerator IEnumerable.GetEnumerator() {
		return GetEnumerator();
	}



}
