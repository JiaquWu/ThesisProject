using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Levels",fileName ="Level", order = 0)]
public class LevelConfigSO : ScriptableObject {
    public List<TileInfo> tileInfos;

}

[Serializable]
public struct TileInfo {
    public Vector2 coordinate;
    public int tileHealth;
    public TileState tileState;
}