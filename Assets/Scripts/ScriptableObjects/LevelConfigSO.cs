using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Obsolete]
[CreateAssetMenu(menuName ="Levels",fileName ="Level", order = 0)]
public class LevelConfigSO : ScriptableObject {
    public Vector2 offset;
    public float scaleFactor;
    public List<TileInfo> tileInfos;
    
}

[Serializable]
public struct TileInfo {
    public Vector2 coordinate;
    public int tileHealth;
    public TileState tileState;
}