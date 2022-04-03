using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Level/Sequence",fileName ="LevelSequence", order = 0)]
public class LevelSequence : ScriptableObject {
    public List<LevelData> levels = new List<LevelData>();
}
