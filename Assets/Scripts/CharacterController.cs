using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterActionType {
    CHARACTER_ROTATE,
    CHARACTER_MOVE,
    CHARACTER_INTERACT
}
public class CharacterController : MonoBehaviour
{
    public Stack<Command> characterCommands = new Stack<Command>();
    private void OnEnable() {
        TileManager.MoveAction += CharacterMove;
        TileManager.InteractAction += CharacterInteract;
        TileManager.RotateAction += CharacterRotate;
        //TileManager.UndoAction += Undo;
    }
    private void OnDisable() {
        TileManager.MoveAction -= CharacterMove;
        TileManager.InteractAction -= CharacterInteract;
        TileManager.RotateAction -= CharacterRotate;
        //TileManager.UndoAction -= Undo;
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
        Debug.Log("cc interact"+interaction);
        
    }
    void AddCharacterCommand(CharacterActionType type) {
        //new Command(()=>CharacterInteract(InteractionType.PICK_UP_ANIMALS));
    }

}
// public class CharacterCommand:Command{
//     private Action executeAction;
//     private Action undoAction;
//     public CharacterCommand(Action executeAction,Action undoAction) {
//         this.executeAction = executeAction;
//         this.undoAction = undoAction;
//     }
//     public override void Execute() {
//         executeAction?.Invoke();
//     }
//     public override void Undo() {
//         undoAction?.Invoke();
//     }
// }
