using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat :MonoBehaviour,IPlaceable,IPassable {
    private void OnEnable() {
        LevelManager.RegisterObject(gameObject);
    }
    public void OnPlayerPlace(IInteractable interactable,ref Command command) {
        command.executeAction += ()=>LevelManager.Instance.OnInteractableEnterBoat(interactable,true);
        command.undoAction += ()=>LevelManager.Instance.OnInteractableEnterBoat(interactable,false);
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
 
    public bool IsPassable(Direction dir) {
        return true;
    }
    public bool IsPlaceable() {
        return true;
    }
    
}
