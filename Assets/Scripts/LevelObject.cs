using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObject : MonoBehaviour
{
    public virtual bool IsPassable() {
        return false;
    }
}
