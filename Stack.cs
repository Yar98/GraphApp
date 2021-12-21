using System;
using System.Collections.Generic;

namespace FunctionGraph
{
    class Stack <T>
    {
        public int n;
	public Node first;

	public class Node
	{
	   public T value;
	   public Node next;

	   public Node(T value)
	   {
	       this.value = value;
	   }
	}


	public Stack()
	{
	    first = null;
	    n = 0;
	}

	public bool isEmpty()
	{
	    return first == null;
	}

	public int Size()
	{
	    return n;
	}

	public void push(T t)
	{
	    Node oldFirst = first;
	    first = new Node(t);
	    first.next = oldFirst;
	    n++;
	}

	public T pop()
	{
	    if(isEmpty())
	    {
	        throw new Exception("Stack Overflow");
	    }
	    T t = first.value;
	    first = first.next;
	    n--;
	    return t;
	}

	public T peek()
	{
	    if(isEmpty())
	    {
	        throw new Exception("Stack Overflow");
	    }
	    return first.value;
	}
    }
}