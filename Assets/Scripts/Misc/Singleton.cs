using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static event EventHandler OnInstantiate;
    public static bool IsInstanced { get; private set; } = false;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    if (!isDestroyed)
                    {
                        Debug.LogWarningFormat("No existing singleton of type {0}", typeof(T).Name);
                    }
                }
                else
                {
                    InvokeOnInstantiate();
                }
            }
            return instance;
        }

        private set => instance = value;
    }

    private static T instance;

    private static bool isDestroyed = false;

    private void RegisterInstance(T inst)
    {
        if (instance != null && instance != inst)
        {
            Debug.LogErrorFormat("More than one singleton of type {0} registered. First: {1}, second: {2}",
                typeof(T).Name,
                instance.gameObject.name,
                inst.gameObject.name);
        }
        else
        {
            instance = inst;
            InvokeOnInstantiate();
        }
    }

    private static void InvokeOnInstantiate()
    {
        IsInstanced = true;
        OnInstantiate?.Invoke(null, null);
    }

    protected virtual void Awake()
    {
        RegisterInstance(this as T);
    }

    protected virtual void OnDestroy()
    {
        isDestroyed = true;
        instance = null;
        IsInstanced = false;
    }
}
