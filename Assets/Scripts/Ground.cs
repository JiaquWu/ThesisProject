using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground :MonoBehaviour, ILevelObject {
    int maxHealth;
    int currentHealth;
    private void Awake() {
        LevelManager.RegisterObject(gameObject);
    }
    public bool IsPassable(Direction dir) {
        return true;
        //可能会存在单行道
    }
    public bool IsInteractable(GameObject player) {
       if(!player.TryGetComponent<PlayerController>(out PlayerController controller)) return false;
        //这取决于玩家身上有没有动物
       if(controller.AnimalCharacterHold != null) {
           return true;
       }else {
           return false;
       }
    }
    public void OnPlayerEnter(GameObject player,ref Command command) {  
        //判断玩家当前举着什么动物,来给command绑定不同的action
        //没有任何动物,不绑定任何action
        //如果一滴血,就是command.executeAction += ()=> currentHealth -= 1;
        command.executeAction += ()=>Debug.Log("走到这块砖上面"+gameObject);
        command.undoAction += ()=>Debug.Log("离开这块砖"+gameObject);
    }
    public void OnPlayerInteract(GameObject player,ref Command command) {

    }


}
