using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Level/Data",fileName ="LevelData", order = 0)]
public class LevelData : ScriptableObject {
    [SerializeField]
    private string fileName;//关卡文件的名字,也就是场景的名字
    public string FileName => fileName;

    [SerializeField]
    private string levelName;//自己的名字
    public string LevelName => LevelName;

    [SerializeField]
    private Sprite levelThumbnail;
    public Sprite LevelThumbnail => levelThumbnail;
    [SerializeField]
    private Sprite levelIndicateImage;
    public Sprite LevelIndicateImage => levelIndicateImage;
    
}
