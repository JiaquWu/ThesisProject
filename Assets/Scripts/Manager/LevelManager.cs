using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonManager<LevelManager>
{
    public CommandHandler commandHandler{get;private set;}
    private void Awake() {
        commandHandler = new CommandHandler();
    }
    List<GameObject> objects = new List<GameObject>();

    public static void RegisterObject(GameObject go) {
        if(!Instance.objects.Contains(go)) Instance.objects.Add(go);
    }
    public static List<GameObject> GetObjectsOn(Vector3 pos) {
        List<GameObject> results = new List<GameObject>();
        foreach (var item in Instance.objects) {
            if(Vector3.Distance(item.transform.position,pos) <= 0.1f) {
                results.Add(item);
            }
        }
        return results;
        
    }

}
