using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat :MonoBehaviour,IInteractable,ILevelObject
{
    private void OnEnable() {
        LevelManager.RegisterObject(gameObject);
    }

    public void OnPlayerEnter(GameObject player,ref Command command) {

    }
    public bool IsPassable(Direction dir) {
        return true;
    }
    public bool IsInteractable(GameObject player) {
        if(!player.TryGetComponent<PlayerController>(out PlayerController controller)) return false;
        if(controller.InteractableCharacterHold == null) return false;
        return true;
    }
    public void OnPlayerInteract(InteractionType interaction,GameObject player,ref Command command) {
        //if(interaction == InteractionType.PUT_DOWN_ANIMALS)
        Debug.Log("送狗上船"); 
    }
    
}
