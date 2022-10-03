using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Ground :MonoBehaviour, IPassable,IPlaceable {
    int maxHealth = 2;
    [SerializeField]
    int currentHealth = 2;
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
        return true;
    }
    public bool IsPlaceable() {
        return currentHealth != 0;
    }
    public void OnPlayerEnter(GameObject player,ref Command command) {  
        if(!player.TryGetComponent<PlayerController>(out PlayerController controller)) return;
        IInteractable temp = controller.InteractableCharacterHold;
        if(LevelManager.Instance.IsPlayerEnterBoat) {
            command.executeAction += ()=>LevelManager.Instance.OnPlayerEnterBoat(false);
            command.undoAction += ()=>LevelManager.Instance.OnPlayerEnterBoat(true);
        }
        if(temp == null) {
            if(currentHealth == 2) {
                AudioManager.Instance.PlayPlayerMoveAudio();
            }else if(currentHealth == 1) {
                AudioManager.Instance.PlayIceBreakAudio();
            }else {
                command.executeAction += ()=>OnPlayerEnterBrokenGround(true);
                command.undoAction += ()=>OnPlayerEnterBrokenGround(false);
            }
        }else {
            command.executeAction += ()=>OnBreakingGround(true);
            command.undoAction += ()=>OnBreakingGround(false);           
        }
    }
    public void OnPlayerPlace(IInteractable player,ref Command command) {

    }
    private void OnPlayerEnterBrokenGround(bool b) {
        if(b) {
            LevelManager.Instance.OnPlayerDeadInvoke();
            UIManager.Instance.SetDrownObjectActive(true);
            AudioManager.Instance.PlayPlayerDrownAudio();
        }else {
            UIManager.Instance.SetDrownObjectActive(false);
        }
    }

    public void OnBreakingGround(bool b) {
        if(b) {
            currentHealth = Mathf.Clamp(currentHealth-1,0,maxHealth);
            ChangeGroundSprite(currentHealth);
            if(currentHealth <= 0) {
                OnPlayerEnterBrokenGround(b);
            }else {
                AudioManager.Instance.PlayIceBreakAudio();
            }
        }else {
            OnPlayerEnterBrokenGround(b);
            currentHealth = Mathf.Clamp(currentHealth+1,0,maxHealth);
            ChangeGroundSprite(currentHealth);
        }
    }
    void ChangeGroundSprite(int health) {
        if(health == 0) {
            GetComponent<SpriteRenderer>().sprite = groundZeroHealthSprite;
        }else if(health == 1) {
            GetComponent<SpriteRenderer>().sprite = groundOneHealthSprite;
        }else if(health == 2) {
            GetComponent<SpriteRenderer>().sprite = groundTwoHealthSprite;
        }
    }

}
