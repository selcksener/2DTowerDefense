using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T Instance { get { return _instance; } }

    public virtual void Awake()
    {
        if (_instance == null) _instance = this as T;
        else Destroy(gameObject);
    }
}
