using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonManager<LevelManager>
{
    public CommandHandler commandHandler{get;private set;}
    public static event Action OnlevelFinish;
    public static event Action OnPlayerDead;
    public bool IsPlayerDead{get;private set;}
    public bool IsPlayerEnterBoat{get;private set;}
    public bool IsLevelFinished{get;private set;}
    private void Awake() {
        commandHandler = new CommandHandler();
    }
    List<GameObject> levelObjects = new List<GameObject>();
    List<IInteractable> levelGoals = new List<IInteractable>();

    private void OnDisable() {
        Instance.levelObjects.Clear();
        Instance.levelGoals.Clear();
    }
    public static void RegisterObject(GameObject go) {
        if(!Instance.levelObjects.Contains(go)) Instance.levelObjects.Add(go);
        if(go.TryGetComponent<IInteractable>(out IInteractable interactable)) {
            if(!Instance.levelGoals.Contains(interactable)) Instance.levelGoals.Add(interactable);
        }
    }
    public static void UnRegisterObject(GameObject go) {
        if(Instance.levelObjects.Contains(go)) Instance.levelObjects.Remove(go);
    }
    public static List<GameObject> GetObjectsOn(Vector3 pos) {
        List<GameObject> results = new List<GameObject>();
        foreach (var item in Instance.levelObjects) {
            if(Vector3.Distance(item.transform.position,pos) <= 0.1f) {
                results.Add(item);
            }
        }
        return results;
    }
    public static List<T> GetInterfaceOn<T>(Vector3 pos) { 
        List<GameObject> objects = GetObjectsOn(pos);
        List<T> results = new List<T>();
        foreach (var item in objects) {
           if(item.TryGetComponent(out T t)) {
               results.Add(t);
           }
        }
        return results;
    }
    public void UndoCheckPlayerDead() {
        if(IsPlayerDead) IsPlayerDead = false;
    }
    public void OnPlayerDeadInvoke() {
        OnPlayerDead.Invoke();
        IsPlayerDead = true;
    }
    public void OnPlayerEnterBoat(bool isEntering) {
        if(isEntering) {
            IsPlayerEnterBoat = true;
            if(levelGoals.Count == 0) {
                OnLevelFinish();
            }
        }else {
            IsPlayerEnterBoat = false;
        }
    }
    public void OnInteractableEnterBoat(IInteractable interactable,bool isEntering) {
        if(isEntering) {
            if(levelGoals.Contains(interactable)) {
                levelGoals.Remove(interactable);
                if(levelGoals.Count == 0 && IsPlayerEnterBoat) {
                    OnLevelFinish();
                }
            }
        }else {
            if(!levelGoals.Contains(interactable)) {
                levelGoals.Add(interactable);
            }
        }
    }
    void OnLevelFinish() {
        OnlevelFinish.Invoke();
        IsLevelFinished = true;
        AudioManager.Instance.PlayplayerFinishLevelAudio();
    }
}
