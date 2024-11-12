using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct IntStack
{
    private int[] stack;
    private int head;
    private int size;
    public bool IsFull { get => !(head < size) ; }
    public bool IsEmty { get => head < 1; }

    public IntStack(int size)
    {
        this.stack = new int[size];
        this.head = 0;
        this.size = size;
    }

    public int Pop()
    {
        if (IsEmty)
        {
            Debug.LogWarning("intstack is emty cant pop, returning 0");
            return 0;
        }
        head--;
        int num = stack[head];
        return num;
    }
    public void Push(int num)
    {
        if (IsFull)
        {
            Debug.LogWarning("intstack if full cant push");
            return;
        }
        stack[head] = num;
        head++;
    }
}
