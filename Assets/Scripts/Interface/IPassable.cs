using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPassable 
{
    bool IsPassable(Direction dir);
    void OnPlayerEnter(GameObject player,ref Command command);
    
}
