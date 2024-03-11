using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : new ()
{
    private static T instance;
    public static T Instance
    {
        get 
        {
            if (instance == null) 
            {
                instance = new T ();
            }
            return instance;
        }
    }
}
