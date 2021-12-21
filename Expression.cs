using System;
using System.Collections.Generic;

namespace FunctionGraph
{
    class Expression
    {
        public static int priority(char ch)
	{
	    switch(ch)
	    {
	        case '+':
		case '-':
		     return 1;
		case '*':
		case '/':
		     return 2;
		case '^':
		     return 3;
	    }
	    return -1;
	}

	public static string infixToPostfix(string func)
	{
	    string result = "";
	    char[] s = func.ToCharArray();
	    Stack<char> stack = new Stack<char>();
	    for(int i = 0; i < s.Length; ++i)
	    {
	        char c = s[i];
		if(Char.IsDigit(c) || c == 'x')
		{
		    result += c;
		}
		else if(c == '(')
		{
		    stack.push(c);
		}
		else if(c == ')')
		{
		    while (!stack.isEmpty() && stack.peek() != '(')
		    {
		        result += stack.pop();
		    }
		    if (!stack.isEmpty() && stack.peek() != '(')
		    {
		        return "Invalid expression";
		    }
		    else
			stack.pop();
		}
		else
		{
		    result += " ";
		    while(!stack.isEmpty() && priority(c) <= priority(stack.peek()))
		    {
		        result += stack.pop();
		    }
		    stack.push(c);
		}
	    }
	    while(!stack.isEmpty())
	    {
	        result += stack.pop();
	    }
	    return result;
	}

	public static float evaluatePostfix(string func, float x)
	{
	    Stack<float> stack = new Stack<float>();
	    char[] s = func.ToCharArray();
	    for(int i = 0; i < s.Length; i++)
	    {
	        char c = s[i];
		if( c == ' ')
		{
		    continue;
		}
		else if(Char.IsDigit(c))
		{
		    float n =0;
		    while(Char.IsDigit(c))
		    {
		        n = n * 10 + (int)(c - '0');
			i++;
			c = s[i];
		    }
		    i--;
		    stack.push(n);
		}
		else if( c == 'x')
		{
		    stack.push(x);
		}
		else
		{
		    float val1 = stack.pop();
		    float val2 = stack.pop();
		    switch(c)
		    {
		        case '-':
			     stack.push(val2-val1);
			     break;
			case '+':
			     stack.push(val2 + val1);
			     break;
			case '*':
			     stack.push(val2 * val1);
			     break;
			case '/':
			     stack.push(val2 / val1);
			     break;
			case '^':
			     float temp = val2;
			     for(int k = 1; k < val1; ++k)
			     {
			         val2 *= temp;
			     }
			     stack.push(val2);
			     break;
		    }
		}
	    }
	    return stack.pop();
	}
    }
}