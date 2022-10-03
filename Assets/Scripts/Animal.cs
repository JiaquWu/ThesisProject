using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Animal : MonoBehaviour,IInteractable
{
    private bool isBeingHold;
    private void OnEnable() {
        LevelManager.RegisterObject(gameObject);
    }

    public void OnPlayerInteract(InteractionType interaction,IPlaceable placeable,GameObject player,ref Command command) {
        if(!player.TryGetComponent<PlayerController>(out PlayerController playerController)) return;
        Vector3 animalPlacePosition = playerController.CharacterPosition + Utilities.DirectionToVector(playerController.CharacterDirection);
        command.executeAction += ()=>AnimalInteractCommand(interaction,placeable,animalPlacePosition);
        command.undoAction += ()=>AnimalInteractCommand(Utilities.ReverseInteractionType(interaction),placeable,animalPlacePosition);
    }
    public bool IsInteractable(GameObject player) {
        return !isBeingHold;
    }

    void AnimalInteractCommand(InteractionType interaction,IPlaceable placeable,Vector3 animalPlacePosition) {

        switch (interaction)
        {
            case InteractionType.PICK_UP_ANIMALS:
            if(placeable == null) return;
            if(LevelManager.Instance.IsPlayerEnterBoat) {
                LevelManager.Instance.OnInteractableEnterBoat(this,true);
            }
            if(placeable.GetType().Name == "Boat") {
                GetComponent<SpriteRenderer>().enabled = false;
                LevelManager.UnRegisterObject(gameObject);
                isBeingHold = true;
            }else {
                GetComponent<SpriteRenderer>().enabled = false;
                LevelManager.UnRegisterObject(gameObject);
                isBeingHold = true;
            }
            AudioManager.Instance.PlayPlayerPickUpAnimalAudio();
            break;
            case InteractionType.PUT_DOWN_ANIMALS:
            if(placeable == null) return;
            if(LevelManager.Instance.IsPlayerEnterBoat) {
                LevelManager.Instance.OnInteractableEnterBoat(this,false);
            }
            if(placeable.GetType().Name == "Boat") {
                GetComponent<SpriteRenderer>().enabled = false;
                LevelManager.UnRegisterObject(gameObject);
                AudioManager.Instance.PlayplayerPutDownAnimalOnBoatAudio();
            }else {
                gameObject.transform.position = animalPlacePosition;
                GetComponent<SpriteRenderer>().enabled = true;
                LevelManager.RegisterObject(gameObject);
            }
            isBeingHold = false;                   
            break;
        }
    }
}
