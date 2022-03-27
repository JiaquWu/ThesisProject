using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat :MonoBehaviour,IPlaceable
{
    private void OnEnable() {
        LevelManager.RegisterObject(gameObject);
    }
    public void OnPlayerPlace(IInteractable interactable,ref Command command) {
        command.executeAction += ()=>Debug.Log("金毛上船了" + interactable);
        command.undoAction += ()=>Debug.Log("撤回" + interactable);
    }
    public bool IsPassable(Direction dir) {
        return false;
    }
    public bool IsPlaceable() {
        return true;
    }
    
}
