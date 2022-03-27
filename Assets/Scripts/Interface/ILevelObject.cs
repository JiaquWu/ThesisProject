using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelObject 
{
    void OnPlayerEnter(GameObject player,ref Command command);
    bool IsPassable(Direction dir);
    bool IsPlaceable();
}
