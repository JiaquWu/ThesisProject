using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlaceable
{
    bool IsPlaceable();
    void OnPlayerPlace(GameObject player,ref Command command); 
}
