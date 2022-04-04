using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Ground :MonoBehaviour, IPassable,IPlaceable {
    int maxHealth = 2;
    [SerializeField]
    int currentHealth = 2;//但是是通过什么来设定呢?如果要手动设置,就公开出来,如果程序赋值,就变成属性
    [SerializeField]
    private Sprite groundZeroHealthSprite;
    [SerializeField]
    private Sprite groundOneHealthSprite;
    [SerializeField]
    private Sprite groundTwoHealthSprite;
    
    private void OnEnable() {
        LevelManager.RegisterObject(gameObject);
        ChangeGroundSprite(currentHealth);
    }
    public bool IsPassable(Direction dir) {
        return currentHealth != 0;
        //可能会存在单行道
    }
    public bool IsPlaceable() {
        return currentHealth != 0;
    }
    public void OnPlayerEnter(GameObject player,ref Command command) {  
        //判断玩家当前举着什么动物,来给command绑定不同的action
        //没有任何动物,不绑定任何action
        //如果一滴血,就是command.executeAction += ()=> currentHealth -= 1;
        if(!player.TryGetComponent<PlayerController>(out PlayerController controller)) return;
        IInteractable temp = controller.InteractableCharacterHold;
        Debug.Log("这里会进来" + temp);
        //要考虑到玩家从船上走到地面
        if(LevelManager.Instance.IsPlayerEnterBoat) {
            command.executeAction += ()=>LevelManager.Instance.OnPlayerEnterBoat(false);
            command.undoAction += ()=>LevelManager.Instance.OnPlayerEnterBoat(true);
        }
        if(temp == null) {
            //说明玩家没有拿东西,因此没啥反应,走进来不会有,撤销也不会有,除了播放声音
            if(currentHealth == 1) {//因为能走就肯定不是碎的
                AudioManager.Instance.PlayIceBreakAudio();
            }else {
                AudioManager.Instance.PlayPlayerMoveAudio();
            }
        }else {
            //玩家拿了东西,所以会掉血,图片会变,撤销的话就把血加回来,图片变回来
            command.executeAction += ()=>OnBreakingGround(true);
            command.undoAction += ()=>OnBreakingGround(false);           
        }
        //这里不是简单地添加离开和进入两个方法,因为比如说玩家拿着动物走进了一个格子,它应该掉血,如果是离开血还是会掉的,应该是"撤销"
    }
    public void OnPlayerPlace(IInteractable player,ref Command command) {

    }

    public void OnBreakingGround(bool b) {//true就是说是掉血
        if(b) {
            currentHealth = Mathf.Clamp(currentHealth-1,0,maxHealth);
            ChangeGroundSprite(currentHealth);
            if(currentHealth <= 0) {
                //说明玩家死了
                LevelManager.Instance.OnPlayerDeadInvoke();
                UIManager.Instance.SetDrownObjectActive(true);
                AudioManager.Instance.PlayPlayerDrownAudio();
            }else {
                //踩了之后玩家没死,图片已经变了,要播放声音
                AudioManager.Instance.PlayIceBreakAudio();
            }
        }else {
            UIManager.Instance.SetDrownObjectActive(false);
            currentHealth = Mathf.Clamp(currentHealth+1,0,maxHealth);
            ChangeGroundSprite(currentHealth);
        }
    }
    void ChangeGroundSprite(int health) {
        if(health == 0) {
            //让图片消失
            GetComponent<SpriteRenderer>().sprite = groundZeroHealthSprite;
        }else if(health == 1) {
            GetComponent<SpriteRenderer>().sprite = groundOneHealthSprite;
        }else if(health == 2) {
            GetComponent<SpriteRenderer>().sprite = groundTwoHealthSprite;
        }
    }

}
