using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {
    bool IsInteractable(GameObject player);
    void OnPlayerInteract(InteractionType interaction,IPlaceable placeable, GameObject player,ref Command command);
}
