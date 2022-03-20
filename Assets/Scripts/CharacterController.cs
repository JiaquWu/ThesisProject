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
    private Stack<Command> characterCommands = new Stack<Command>();
    private void OnEnable() {
        TileManager.MoveAction += CharacterMove;
        TileManager.InteractAction += CharacterInteract;
        TileManager.RotateAction += CharacterRotate;
        GameManager.UndoInputAction += CharacterUndoCommand;
    }
    private void OnDisable() {
        TileManager.MoveAction -= CharacterMove;
        TileManager.InteractAction -= CharacterInteract;
        TileManager.RotateAction -= CharacterRotate;
        GameManager.UndoInputAction -= CharacterUndoCommand;
    }

    void CharacterRotate(Direction startDir, Direction targetDir) {
        Command command = new Command(()=>CharacterRotateCommand(startDir,targetDir),()=>CharacterRotateCommand(targetDir,startDir));
        characterCommands.Push(command);
        command.Execute();
    }
    void CharacterMove(bool canMove, Direction dir) {
        

    }
    void CharacterInteract(InteractionType interaction) {
        InteractionType type = interaction == InteractionType.PICK_UP_ANIMALS ? InteractionType.PUT_DOWN_ANIMALS : InteractionType.PICK_UP_ANIMALS;
        Command command = new Command(()=>CharacterInteractCommand(interaction),()=>CharacterInteractCommand(type));
        characterCommands.Push(command);
        command.Execute();
        
    }
    void CharacterRotateCommand(Direction startDir, Direction targetDir) {
        //玩家从之前的朝向转向新的朝向
    }
    void CharacterMoveCommand(bool canMove, Direction dir) {
        //处理角色动画之类的东西
        Debug.Log("处理角色动画之类的东西");

    }
    void CharacterInteractCommand(InteractionType interaction) {
        Debug.Log("cc interact"+interaction);
        
    }
    void CharacterUndoCommand() {
        if(characterCommands.Count > 0) {
           Command command = characterCommands.Pop();
           command.Undo();
        }
    }
    // void AddCharacterCommand(CharacterActionType type) {
    //     switch (type)
    //     {
    //         case CharacterActionType.CHARACTER_MOVE:
    //         new Command(()=>CharacterMove());
    //         break;

    //     }
    //     //new Command(()=>CharacterInteract(InteractionType.PICK_UP_ANIMALS));
    // }

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
