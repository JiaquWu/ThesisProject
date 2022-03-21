using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonManager<T> : MonoBehaviour where T:Component
{
    private static T instance;
    public static T Instance {
        get{
            if(!instance) {
                instance = FindObjectOfType<T>();
                if(!instance) {
                    Debug.LogError("There is no GameManager object in any of the currently loaded scenes");
                }
            }
            return instance;
        }
    }
}
