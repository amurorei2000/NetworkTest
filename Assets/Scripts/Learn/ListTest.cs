using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListTest : MonoBehaviour
{
    public MyList<int> mlist;

    void Start()
    {
        mlist = new MyList<int>();

        mlist.Add(3);
        mlist.Add(12);
        mlist.Add(1);
        
        for(int i = 0; i < mlist.Count; i++)
        {
            print(mlist[i]);
        }

        print("==============================");
        mlist.Remove(12);

        for (int i = 0; i < mlist.Count; i++)
        {
            print(mlist[i]);
        }

        print("==============================");
        //mlist.Clear();

        //for (int i = 0; i < mlist.Count; i++)
        //{
        //    print(mlist[i]);
        //}
    }
}
