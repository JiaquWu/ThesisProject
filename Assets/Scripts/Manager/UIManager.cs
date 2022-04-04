using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIManager : SingletonManager<UIManager> {
    [SerializeField]
    private GameObject drownAnimObject;
    [SerializeField]
    private GameObject leftLevelButton;
    [SerializeField]
    private GameObject rightLevelButton;
    [SerializeField]
    private GameObject selectLevelMenuPanel;
    [SerializeField]
    private GameObject selectLevelLeftButton;
    [SerializeField]
    private GameObject selectLevelRightButton;
    [SerializeField]
    private GameObject levelImage_01;
    [SerializeField]
    private GameObject levelImage_02;
    [SerializeField]
    private GameObject levelImage_03;
    [SerializeField]
    private GameObject levelImage_04;
    [SerializeField]
    private GameObject levelImage_05;
    [SerializeField]
    private GameObject levelImage_06;
    private List<GameObject> levelButtonList;
    private int levelSelectPage;//选关界面页数,默认是第0页

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
        if(drownAnimObject != null) {
            Vector3 temp = Camera.main.ViewportToWorldPoint(new Vector2(0.5f,1)) + Vector3.down;
            drownAnimObject.transform.position = new Vector3(temp.x,temp.y,0);
            drownAnimObject.SetActive(false);
        }

        if(levelImage_01 == null) return;//如果这个都没有,其他应该也不会有的
        levelButtonList = new List<GameObject>();
        levelButtonList.Add(levelImage_01.transform.GetChild(0).gameObject);
        levelButtonList.Add(levelImage_02.transform.GetChild(0).gameObject);
        levelButtonList.Add(levelImage_03.transform.GetChild(0).gameObject);
        levelButtonList.Add(levelImage_04.transform.GetChild(0).gameObject);
        levelButtonList.Add(levelImage_05.transform.GetChild(0).gameObject);
        levelButtonList.Add(levelImage_06.transform.GetChild(0).gameObject);

    }
    public void OnStartFirstLevelButtonClicked() {
        AudioManager.Instance.PlayUIClickAudio();
        GameManager.Instance.LoadLevel(0);
    }
    public void OnLeftLevelButtonClicked() {
        AudioManager.Instance.PlayUIClickAudio();
        GameManager.Instance.LoadNextOrPrevLevel(true);
    }
    public void OnRightLevelButtonClicked() {
        AudioManager.Instance.PlayUIClickAudio();
        GameManager.Instance.LoadNextOrPrevLevel(false);
    }
    public void OnEscButtonPerformed(InputAction.CallbackContext context) {
        OnSelectLevelMenuButtonClicked();
    }
    public void OnSelectLevelMenuButtonClicked() {
        AudioManager.Instance.PlayUIClickAudio();
        selectLevelMenuPanel.SetActive(!selectLevelMenuPanel.activeSelf);
        if(selectLevelMenuPanel.activeSelf) {      
            levelSelectPage = 0;
            RefreshCurrentLevelPage();
        }
    }
    public void OnSelectLevelLeftAndRightButtonClicked(bool isRight) {
        AudioManager.Instance.PlayUIClickAudio();
        levelSelectPage = isRight? levelSelectPage + 1 : levelSelectPage - 1;
        RefreshCurrentLevelPage();
    }
    private void RefreshCurrentLevelPage() {
       if(GameManager.Instance.LevelSequence == null || GameManager.Instance.LevelSequence.levels.Count == 0) Debug.LogError("no levelSequce or level");
       int maxPages = Mathf.CeilToInt(GameManager.Instance.LevelSequence.levels.Count / 6f) - 1;//因为0是第一页
       Debug.Log(GameManager.Instance.LevelSequence.levels.Count/6f);
       selectLevelLeftButton.SetActive(levelSelectPage != 0);
       selectLevelRightButton.SetActive(levelSelectPage != maxPages);
       //知道了当前第几页,那么那六个图片分别要显示指定的关卡缩略图,六个按钮要绑定选择加载对应关卡的方法
       //首先要显示几个呢?因为不知道上次开关了几个,所以每次刷新都要重新加载每一个的状态
       int activeObjectCount = 0;
       if(levelSelectPage == maxPages && GameManager.Instance.LevelSequence.levels.Count % 6 != 0) {//只有在最后一页并且不是6的整数才是别的数
          activeObjectCount = GameManager.Instance.LevelSequence.levels.Count % 6;
       }else {
           activeObjectCount = 6;
       }
       for (int i = 0; i < levelButtonList.Count; i++) {
           if(i<activeObjectCount) {
               levelButtonList[i].SetActive(true);
               //然后是换图片和挂事件
               LevelData data = GameManager.Instance.LevelSequence.levels[levelSelectPage * 6 + i];//比如说第一页第二个就是0+1 
               levelButtonList[i].GetComponent<Image>().sprite = data.LevelThumbnail;
               levelButtonList[i].GetComponent<Button>().onClick.RemoveAllListeners();
               levelButtonList[i].GetComponent<Button>().onClick.AddListener(()=> {
                   AudioManager.Instance.PlayUIClickAudio();
                   GameManager.Instance.LoadLevel(data.FileName);});
           }else {
               levelButtonList[i].SetActive(false);
           }
       }
    }

    public void SetDrownObjectActive(bool b) {
        drownAnimObject.SetActive(b);
    }
}
