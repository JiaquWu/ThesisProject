using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    
    private void OnEnable() {
        TileManager.MoveAction += CharacterMove;
        TileManager.InteractAction += CharacterInteract;
        TileManager.RotateAction += CharacterRotate;
    }
    private void OnDisable() {
        TileManager.MoveAction -= CharacterMove;
        TileManager.InteractAction -= CharacterInteract;
        TileManager.RotateAction -= CharacterRotate;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CharacterRotate(Direction startDir, Direction targetDir) {
        //玩家从之前的朝向转向新的朝向
    }
    void CharacterMove(bool canMove, Direction dir) {
        //处理角色动画之类的东西
        Debug.Log("处理角色动画之类的东西");

    }
    void CharacterInteract(InteractionType interaction) {
        Debug.Log("cc interact");

    }
}
