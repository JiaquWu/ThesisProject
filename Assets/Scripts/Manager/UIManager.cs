using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonManager<UIManager> {
    [SerializeField]
    private GameObject leftLevelButton;
    [SerializeField]
    private GameObject rightLevelButton;

    private void Start() {
        if(leftLevelButton != null) {
            leftLevelButton.SetActive(!GameManager.isFirstLevel());
        }
        if(rightLevelButton != null) {
            rightLevelButton.SetActive(!GameManager.isLastLevel());
        }
    }
    public void OnLeftLevelButtonClicked() {
        GameManager.LoadLevel(true);
    }
    public void OnRightLevelButtonClicked() {
        GameManager.LoadLevel(false);
    }
}
