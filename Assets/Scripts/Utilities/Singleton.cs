using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//继承自MonoBehaviour的单例类
public class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }
            else
            {
                Debug.LogWarning("Instance is null");
                return null;
            }
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

}
//未继承自MonoBehaviour的单例类
public class Singleton<T> where T : Singleton<T>,new()
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
            }
            return _instance;
        }
    }


}
