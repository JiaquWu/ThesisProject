using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum Direction {
    NONE,
    UP,
    DOWN,
    LEFT,
    RIGHT
}
public enum InteractionType {
    NONE,
    PICK_UP_ANIMALS,
    PUT_DOWN_ANIMALS
}

public class GameManager : SingletonManager<GameManager> {
    [SerializeField]
    private LevelSequence levelSequence;
    // private List<string> allLevels;
    // private string level_01 = "level_01";
    // private string level_02 = "level_02";
    // private string level_03 = "level_03";
    // private string level_04 = "level_04";
    // private string level_05 = "level_05";
    // private string level_06 = "level_06";
    // private string level_07 = "level_07";
    // private string level_08 = "level_08";
    // private string level_09 = "level_09";
    // private string level_10 = "level_10";
    // private string level_11 = "level_11";
    // private string level_12 = "level_12";

    private void Awake() {
        levelSequence = Resources.Load<LevelSequence>("LevelSequence_01");
        if(levelSequence == null) {
            Debug.LogError("no such levelSequence");//这样后面的方法就不用检测了
        }
        DontDestroyOnLoad(gameObject);
        // allLevels = new List<string>() {
        //     level_01,
        //     level_02,
        //     level_03,
        //     level_04,
        //     level_05,
        //     level_06,
        //     level_07,
        //     level_08,
        //     level_09,
        //     level_10,
        //     level_11,
        //     level_12,
        // };
    }
    public void LoadLevel(int levelIndex) {
        if(levelSequence.levels.Count <= levelIndex) {
            Debug.LogError("no such level" + levelIndex);
            return;
        }
        LevelData data = levelSequence.levels[levelIndex];
        SceneManager.LoadScene(data.FileName);
        // if(Instance.allLevels.Count <= levelIndex) return;
        // SceneManager.LoadScene(Instance.allLevels[levelIndex]);
    }
    public void LoadNextOrPrevLevel(bool isPrevLevel) {
        string currentLevel = SceneManager.GetActiveScene().name;
        for (int i = 0; i < levelSequence.levels.Count; i++) {
            if(levelSequence.levels[i].FileName == currentLevel) {
                int targetLevelIndex = isPrevLevel? i-1 : i+1;
                SceneManager.LoadScene(levelSequence.levels[targetLevelIndex].FileName);
            }
        }
        // if(allLevels != null && allLevels.Contains(currentLevel)) {
        //     int targetLevelIndex = isPrevLevel?allLevels.IndexOf(currentLevel) - 1:allLevels.IndexOf(currentLevel) + 1;
        //     SceneManager.LoadScene(allLevels[targetLevelIndex]);//一定不是第一个或者最后一个,没有对应按钮   
        // }
    }

    public bool isFirstLevel() {
        string currentLevel = SceneManager.GetActiveScene().name;
        if(levelSequence.levels[0].FileName == currentLevel) return true;
        //if(allLevels != null && allLevels[0] == currentLevel) return true;
        return false;
    }
    public bool isLastLevel() {
        string currentLevel = SceneManager.GetActiveScene().name;
        if(levelSequence.levels[levelSequence.levels.Count-1].FileName == currentLevel) return true;
        //if(allLevels != null && allLevels[Instance.allLevels.Count-1] == currentLevel) return true;
        return false;
    }
}
