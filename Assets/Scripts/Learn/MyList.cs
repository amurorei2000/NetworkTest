using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MyList<T>
{
    T[] arr;
    public int Count = 0;
    

    public MyList()
    {
        arr = new T[0];
    }

    public T this[int index]
    {
        get { return arr[index]; }
    }

    public void Add(T item)
    {
        Count++;
        Array.Resize<T>(ref arr, Count);
        arr[Count - 1] = item;
    }

    public bool Remove(T item)
    {
        bool result = false;
        int idx = -1;

        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i].Equals(item))
            {
                idx = i;
                result = true;
                
                for (int j = idx + 1; j < arr.Length; j++)
                {
                    arr[j - 1] = arr[j];
                }
                Count--;
                Array.Resize<T>(ref arr, Count);
                break;
            }
        }
        return result;
    }

    public void Clear()
    {
        Array.Resize<T>(ref arr, 0);
        Count = 0;
    }
}
