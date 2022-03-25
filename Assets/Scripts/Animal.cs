using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour,ILevelObject,IInteractable
{
    private void OnEnable() {
        LevelManager.RegisterObject(gameObject);
    }
    public void OnPlayerEnter(GameObject player,ref Command command) {
        //
    }
    public void OnPlayerInteract(InteractionType interaction,GameObject player,ref Command command) {
        command.executeAction += ()=>AnimalInteractCommand(interaction);
        command.undoAction += ()=>AnimalInteractCommand(Utilities.ReverseInteractionType(interaction));
    }
    public bool IsPassable(Direction dir) {
        Debug.Log("这里不能走");
        return false;
    }
    public bool IsInteractable(GameObject player) {
        return true;
    }

    void AnimalInteractCommand(InteractionType interaction) {
        Debug.Log("animal要做" + interaction);
        switch (interaction)
        {
            case InteractionType.PICK_UP_ANIMALS:

            break;
            case InteractionType.PUT_DOWN_ANIMALS:

            break;
        }
    }
}
