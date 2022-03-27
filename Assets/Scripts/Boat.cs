using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat :MonoBehaviour,IPlaceable
{
    private void OnEnable() {
        LevelManager.RegisterObject(gameObject);
    }
    public void OnPlayerPlace(GameObject player,ref Command command) {
        
    }
    public bool IsPassable(Direction dir) {
        return false;
    }
    public bool IsPlaceable() {
        return true;
    }
    
}
