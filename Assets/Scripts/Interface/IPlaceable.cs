using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlaceable
{
    bool IsPlaceable();
    void OnPlayerPlace(IInteractable interactable,ref Command command); 
}
