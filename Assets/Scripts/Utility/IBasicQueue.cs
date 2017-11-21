using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IBasicQueue<T> : IEnumerable<T> {
	void Enqueue(T elem);
	T Dequeue();
	T Peek();
	bool IsEmpty();
}
