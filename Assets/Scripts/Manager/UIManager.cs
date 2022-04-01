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
    public void OnStartFirstLevelButtonClicked() {
        GameManager.LoadLevel(0);
    }
    public void OnLeftLevelButtonClicked() {
        GameManager.LoadNextOrPrevLevel(true);
    }
    public void OnRightLevelButtonClicked() {
        GameManager.LoadNextOrPrevLevel(false);
    }
}
