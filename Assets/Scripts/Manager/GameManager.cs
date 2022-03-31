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
    private List<string> allLevels;
    private string level_01 = "level_01";
    private string level_02 = "level_02";
    private string level_03 = "level_03";
    private string level_04 = "level_04";
    private string level_05 = "level_05";
    private string level_06 = "level_06";
    private string level_07 = "level_07";
    private string level_08 = "level_08";
    private string level_09 = "level_09";
    private string level_10 = "level_10";
    private string level_11 = "level_11";
    private string level_12 = "level_12";

    private void Awake() {
        allLevels = new List<string>() {
            level_01,
            level_02,
            level_03,
            level_04,
            level_05,
            level_06,
            level_07,
            level_08,
            level_09,
            level_10,
            level_11,
            level_12,
        };
    }

    public static void LoadLevel(bool isPrevLevel) {
        string currentLevel = SceneManager.GetActiveScene().name;
        if(Instance.allLevels != null && Instance.allLevels.Contains(currentLevel)) {
            int targetLevelIndex = isPrevLevel?Instance.allLevels.IndexOf(currentLevel) - 1:Instance.allLevels.IndexOf(currentLevel) + 1;
            SceneManager.LoadScene(targetLevelIndex);//一定不是第一个或者最后一个,没有对应按钮   
        }
    }

    public static bool isFirstLevel() {
        string currentLevel = SceneManager.GetActiveScene().name;
        if(Instance.allLevels != null && Instance.allLevels[0] == currentLevel) return true;
        return false;
    }
    public static bool isLastLevel() {
        string currentLevel = SceneManager.GetActiveScene().name;
        if(Instance.allLevels != null && Instance.allLevels[Instance.allLevels.Count-1] == currentLevel) return true;
        return false;
    }
}
