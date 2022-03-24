using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour,ILevelObject
{
    public void OnPlayerEnter(GameObject player,ref Command command) {
        //
    }
    public void OnPlayerInteract(GameObject player,ref Command command) {

    }
    public bool IsPassable(Direction dir) {
        return false;
    }
    public bool IsInteractable(GameObject player) {
        return true;
    }
}
