using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Animal : MonoBehaviour,IInteractable
{
    private bool isBeingHold;
    private void OnEnable() {
        LevelManager.RegisterObject(gameObject);
    }

    public void OnPlayerInteract(InteractionType interaction,IPlaceable placeable,GameObject player,ref Command command) {
        if(!player.TryGetComponent<PlayerController>(out PlayerController playerController)) return;
        Vector3 animalPlacePosition = playerController.CharacterPosition + Utilities.DirectionToVector(playerController.CharacterDirection);
        command.executeAction += ()=>AnimalInteractCommand(interaction,placeable,animalPlacePosition);
        command.undoAction += ()=>AnimalInteractCommand(Utilities.ReverseInteractionType(interaction),placeable,animalPlacePosition);
    }
    public bool IsInteractable(GameObject player) {
        return !isBeingHold;
    }

    void AnimalInteractCommand(InteractionType interaction,IPlaceable placeable,Vector3 animalPlacePosition) {
        //
        Debug.Log("animal要做" + interaction);
        switch (interaction)
        {
            case InteractionType.PICK_UP_ANIMALS:
            //如果是拿起来,首先从地图上消失,然后出现在玩家的手中,  
            if(placeable == null) return;
            if(LevelManager.Instance.IsPlayerEnterBoat) {//但是这里要判断玩家在不在船上,如果玩家在船上,这里要触发levelmanager的动物进入船上的方法
                LevelManager.Instance.OnInteractableEnterBoat(this,true);
            }
            if(placeable.GetType().Name == "Boat") {
                //玩家无法从船上拿东西,因此这里只能是撤回发生的操作
                GetComponent<SpriteRenderer>().enabled = false;
                LevelManager.UnRegisterObject(gameObject);
                isBeingHold = true;
            }else {
                GetComponent<SpriteRenderer>().enabled = false;
                LevelManager.UnRegisterObject(gameObject);
                isBeingHold = true;
            }
            //先这么写着,万一有变化
            break;
            case InteractionType.PUT_DOWN_ANIMALS:
            if(placeable == null) return;//肯定不会null 只是以防万一
            if(LevelManager.Instance.IsPlayerEnterBoat) {//但是这里要判断玩家在不在船上,如果玩家在船上,这里要触发levelmanager的动物进入船上的方法
                LevelManager.Instance.OnInteractableEnterBoat(this,false);
            }
            //说明是放下去
            if(placeable.GetType().Name == "Boat") {
                //放在船上
                GetComponent<SpriteRenderer>().enabled = false;
                LevelManager.UnRegisterObject(gameObject);//因为这个animal完成了它的使命,所以不需要注册了
            }else {
                //放在地面上 出现在玩家的前面一个位置  
                gameObject.transform.position = animalPlacePosition;
                GetComponent<SpriteRenderer>().enabled = true;
                LevelManager.RegisterObject(gameObject);
            }
            isBeingHold = false;                   
            break;
        }
    }
}
