using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class SingletonMonoBehavior<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Type t = typeof(T);
                instance = (T)FindObjectOfType(t);

                if (instance == null)
                {
                    Debug.LogError("Missing GameObject " + t + "!!");
                }
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        // 他のオブジェクトにアタッチされているかどうかをチェック。
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }
}

public class DebugSystem : SingletonMonoBehavior<DebugSystem>
{

}
