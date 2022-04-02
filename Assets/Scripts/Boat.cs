using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat :MonoBehaviour,IPlaceable,IPassable {

    private List<SpriteRenderer> animalsAttachedList = new List<SpriteRenderer>();
    private void Update() {
        Debug.Log(transform.position);
    }
    private void OnEnable() {
        LevelManager.RegisterObject(gameObject);
        LevelManager.OnlevelFinish += OnLevelFinish;

        SpriteRenderer[] temp = GetComponentsInChildren<SpriteRenderer>(true);
        foreach (var item in temp) {
            if(item.gameObject.GetComponent<Boat>() == null) {//说明不是自己
                animalsAttachedList.Add(item);
                item.enabled = false;
            }               
        }
    }
    private void OnDisable() {
        LevelManager.OnlevelFinish -= OnLevelFinish;
    }
    public void OnPlayerPlace(IInteractable interactable,ref Command command) {
        command.executeAction += ()=>LevelManager.Instance.OnInteractableEnterBoat(interactable,true);
        command.executeAction += ()=>DisPlayAnimalIcon(true);
        command.undoAction += ()=>LevelManager.Instance.OnInteractableEnterBoat(interactable,false);
        command.undoAction += ()=>DisPlayAnimalIcon(false);
    }
    public void OnPlayerEnter(GameObject player,ref Command command) {
        //判断玩家有没有拿着动物
        if(!player.TryGetComponent<PlayerController>(out PlayerController controller)) return;
        IInteractable temp = controller.InteractableCharacterHold;
        if(temp != null) {
            //说明玩家拿着东西在,玩家和动物都要上去
            command.executeAction += ()=>LevelManager.Instance.OnInteractableEnterBoat(temp,true);
            command.executeAction += ()=>LevelManager.Instance.OnPlayerEnterBoat(true);
            
            command.undoAction += ()=>LevelManager.Instance.OnInteractableEnterBoat(temp,false);
            command.undoAction += ()=>LevelManager.Instance.OnPlayerEnterBoat(false);     
        }else {
            //只有玩家上去
            command.executeAction += ()=>LevelManager.Instance.OnPlayerEnterBoat(true);
            command.undoAction += ()=>LevelManager.Instance.OnPlayerEnterBoat(false);
        }
    }
    private void DisPlayAnimalIcon(bool b) {
        if(b) {
            for (int i = 0; i < animalsAttachedList.Count; i++) {
                if(animalsAttachedList[i].enabled == false) {
                    animalsAttachedList[i].enabled = true;//找到第一个没有enable的
                    break;
                }
            }
        }else {
            for (int i = animalsAttachedList.Count-1; i >= 0; i--) {
                if(animalsAttachedList[i].enabled == true) {
                    animalsAttachedList[i].enabled = false;//从后往前找到第一个enable了的
                    break;
                }
            }
        }
    }
    public bool IsPassable(Direction dir) {
        return true;
    }
    public bool IsPlaceable() {
        return true;
    }
    private void OnLevelFinish() {
        //这里应该让船带着玩家开走 用一个偷懒的方法好了
        GameObject playerObj = FindObjectOfType<PlayerController>().gameObject;
        if(playerObj != null) {
            playerObj.transform.SetParent(transform);
        }
        //船自己怎么开走呢
        StartCoroutine(BoatLeave());
        
    }
    IEnumerator BoatLeave() {
        int i = 0;
        while(i< 5f/Time.fixedDeltaTime) {//不能直接deltatime,因为帧率不固定
            transform.position += Vector3.right * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
            i++;
        }
        
    }
}
