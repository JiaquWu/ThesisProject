using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Ground :MonoBehaviour, ILevelObject {
    int maxHealth = 2;
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
        if(temp == null) {
            //说明玩家没有拿东西,因此没啥反应,走进来不会有,撤销也不会有
        }else {
            //玩家拿了东西,所以会掉血,图片会变,撤销的话就把血加回来,图片变回来
            command.executeAction += ()=>{currentHealth = Mathf.Clamp(currentHealth-1,0,maxHealth);
                                          ChangeGroundSprite(currentHealth);};
            command.undoAction += ()=>{currentHealth = Mathf.Clamp(currentHealth+1,0,maxHealth);
                                       ChangeGroundSprite(currentHealth);};           
        }
        //这里不是简单地添加离开和进入两个方法,因为比如说玩家拿着动物走进了一个格子,它应该掉血,如果是离开血还是会掉的,应该是"撤销"
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
