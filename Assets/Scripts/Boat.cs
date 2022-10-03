using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat :MonoBehaviour,IPlaceable,IPassable {

    private List<SpriteRenderer> animalsAttachedList = new List<SpriteRenderer>();
    private void OnEnable() {
        LevelManager.RegisterObject(gameObject);
        LevelManager.OnlevelFinish += OnLevelFinish;

        SpriteRenderer[] temp = GetComponentsInChildren<SpriteRenderer>(true);
        foreach (var item in temp) {
            if(item.gameObject.GetComponent<Boat>() == null) {
                animalsAttachedList.Add(item);
                item.enabled = false;
            }               
        }
    }
    private void OnDisable() {
        LevelManager.OnlevelFinish -= OnLevelFinish;
    }
    public void OnPlayerPlace(IInteractable interactable,ref Command command) {
        command.executeAction += ()=>LevelManager.Instance.OnInteractableEnterBoat(interactable,true);
        command.executeAction += ()=>DisPlayAnimalIcon(true);
        command.undoAction += ()=>LevelManager.Instance.OnInteractableEnterBoat(interactable,false);
        command.undoAction += ()=>DisPlayAnimalIcon(false);
    }
    public void OnPlayerEnter(GameObject player,ref Command command) {
        if(!player.TryGetComponent<PlayerController>(out PlayerController controller)) return;
        IInteractable temp = controller.InteractableCharacterHold;
        if(temp != null) {
            command.executeAction += ()=>LevelManager.Instance.OnInteractableEnterBoat(temp,true);
            command.executeAction += ()=>LevelManager.Instance.OnPlayerEnterBoat(true);
            
            command.undoAction += ()=>LevelManager.Instance.OnInteractableEnterBoat(temp,false);
            command.undoAction += ()=>LevelManager.Instance.OnPlayerEnterBoat(false);     
        }else {
            command.executeAction += ()=>LevelManager.Instance.OnPlayerEnterBoat(true);
            command.undoAction += ()=>LevelManager.Instance.OnPlayerEnterBoat(false);
        }
    }
    private void DisPlayAnimalIcon(bool b) {
        if(b) {
            for (int i = 0; i < animalsAttachedList.Count; i++) {
                if(animalsAttachedList[i].enabled == false) {
                    animalsAttachedList[i].enabled = true;
                    break;
                }
            }
        }else {
            for (int i = animalsAttachedList.Count-1; i >= 0; i--) {
                if(animalsAttachedList[i].enabled == true) {
                    animalsAttachedList[i].enabled = false;
                    break;
                }
            }
        }
    }
    public bool IsPassable(Direction dir) {
        return true;
    }
    public bool IsPlaceable() {
        return true;
    }
    private void OnLevelFinish() {
        GameObject playerObj = FindObjectOfType<PlayerController>().gameObject;
        if(playerObj != null) {
            playerObj.transform.SetParent(transform);
        }
        StartCoroutine(BoatLeave());
        
    }
    IEnumerator BoatLeave() {
        int i = 0;
        while(i< 8f/Time.fixedDeltaTime) {
            transform.position += Vector3.right * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
            i++;
        }
        
    }
}
