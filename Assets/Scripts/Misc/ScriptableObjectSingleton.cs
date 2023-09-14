using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObjectSingleton<T>
{
    public static T Instance 
    { 
        get
        {
            if (instance == null)
            {
                T[] assets = Resources.LoadAll<T>("");
                if (assets == null || assets.Length < 1)
                {
                    Debug.LogError("Couldn't find ScriptableObjectSingleton in Resources folder");
                }
                else if (assets.Length > 1)
                {
                    Debug.LogWarning("Multiple ScriptableObjectSingletons of the same type in Resources folder");
                }
                instance = assets[0];
            }
            return instance;
        } 
    }

    private static T instance = null;
}
