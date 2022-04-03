using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonManager<T> : MonoBehaviour 
                        where T:Component
{
    private static T instance;
    public static T Instance {
        get{
            if(!instance) {
                instance = FindObjectOfType<T>();
                if(!instance) {
                    GameObject go = new GameObject();
                    instance = go.AddComponent<T>();
                    Debug.LogWarning(string.Format("There was no {0} object in any of the currently loaded scenes",instance));
                }
            }
            return instance;
        }
    }
}
