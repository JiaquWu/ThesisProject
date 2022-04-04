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
    List<GameObject> levelObjects = new List<GameObject>();//当前level中占位置的所有object,被玩家拿着的不算
    List<IInteractable> levelGoals = new List<IInteractable>();//这关需要搬的东西

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
        //玩家死了之后会发生的事情,比如说显示UI,播放声音
        OnPlayerDead.Invoke();
        IsPlayerDead = true;
        Debug.Log("玩家死辣");
    }
    public void OnPlayerEnterBoat(bool isEntering) {
        if(isEntering) {
            //说明玩家进来了,要判断还没有搬的东西,如果没有说明过关了
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
            //说明动物进来了
            if(levelGoals.Contains(interactable)) {
                levelGoals.Remove(interactable);
                if(levelGoals.Count == 0 && IsPlayerEnterBoat) {//这里理论上不用判断两次,但是不确定哪个方法先执行,所以这样保险,而且由于IsPlayerEnterBoat变了
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
        Debug.Log("关卡通过了");
    }
}
