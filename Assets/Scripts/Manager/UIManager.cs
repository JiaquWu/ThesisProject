using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIManager : SingletonManager<UIManager> {
    [SerializeField]
    private GameObject leftLevelButton;
    [SerializeField]
    private GameObject rightLevelButton;
    [SerializeField]
    private GameObject selectLevelMenuPanel;
    private IA_Main uiInputAction;
    private void OnEnable() {
        uiInputAction = new IA_Main();
        uiInputAction.Enable();
        uiInputAction.UI.Esc.performed += OnEscButtonPerformed;//这么写显然是不对的,但是目前只有这一个UI
    }
    private void OnDisable() {

        uiInputAction.UI.Esc.performed -= OnEscButtonPerformed;
        uiInputAction.Disable();
    }
    private void Start() {
        if(leftLevelButton != null) {
            leftLevelButton.SetActive(!GameManager.Instance.isFirstLevel());
        }
        if(rightLevelButton != null) {
            rightLevelButton.SetActive(!GameManager.Instance.isLastLevel());
        }
        if(selectLevelMenuPanel != null) {
            selectLevelMenuPanel.SetActive(false);
        }
    }
    public void OnStartFirstLevelButtonClicked() {
        GameManager.Instance.LoadLevel(0);
    }
    public void OnLeftLevelButtonClicked() {
        GameManager.Instance.LoadNextOrPrevLevel(true);
    }
    public void OnRightLevelButtonClicked() {
        GameManager.Instance.LoadNextOrPrevLevel(false);
    }
    public void OnEscButtonPerformed(InputAction.CallbackContext context) {
        OnSelectLevelMenuButtonClicked();
    }
    public void OnSelectLevelMenuButtonClicked() {
        selectLevelMenuPanel.SetActive(!selectLevelMenuPanel.activeSelf);
    }
}
