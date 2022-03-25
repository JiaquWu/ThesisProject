using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour,ILevelObject,IInteractable
{
    private bool isBeingHold;
    private void OnEnable() {
        LevelManager.RegisterObject(gameObject);
    }
    public void OnPlayerEnter(GameObject player,ref Command command) {
        //
    }
    public void OnPlayerInteract(InteractionType interaction,GameObject player,ref Command command) {
        if(!player.TryGetComponent<PlayerController>(out PlayerController playerController)) return;
        Vector3 animalPlacePosition = playerController.CharacterPosition + Utilities.DirectionToVector(playerController.CharacterDirection);
        command.executeAction += ()=>AnimalInteractCommand(interaction,animalPlacePosition);
        command.undoAction += ()=>AnimalInteractCommand(Utilities.ReverseInteractionType(interaction),animalPlacePosition);
    }
    public bool IsPassable(Direction dir) {
        Debug.Log("这里不能走");
        return false;
    }
    public bool IsInteractable(GameObject player) {
        return !isBeingHold;
    }

    void AnimalInteractCommand(InteractionType interaction, Vector3 animalPlacePosition) {
        Debug.Log("animal要做" + interaction);
        switch (interaction)
        {
            case InteractionType.PICK_UP_ANIMALS:
            //如果是拿起来,首先从地图上消失,然后出现在玩家的手中,
            if(!TryGetComponent<SpriteRenderer>(out SpriteRenderer renderer)) return;
            renderer.enabled = false;
            LevelManager.UnRegisterObject(gameObject);
            isBeingHold = true;
            break;
            case InteractionType.PUT_DOWN_ANIMALS:
            if(!TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer)) return;
            spriteRenderer.enabled = true;
            isBeingHold = false;
            LevelManager.RegisterObject(gameObject);
            //如果是放下去,那么就出现在玩家的前面一个位置
            gameObject.transform.position = animalPlacePosition;
            break;
        }
    }
}
