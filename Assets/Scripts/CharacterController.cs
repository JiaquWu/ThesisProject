using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterController : MonoBehaviour
{
    private Stack<Command> characterCommands = new Stack<Command>();
    private void OnEnable() {
        TileManager.MoveAction += CharacterMove;
        TileManager.InteractAction += CharacterInteract;
        TileManager.RotateAction += CharacterRotate;
        InputManager.UndoInputAction += CharacterUndoCommand;
    }
    private void OnDisable() {
        TileManager.MoveAction -= CharacterMove;
        TileManager.InteractAction -= CharacterInteract;
        TileManager.RotateAction -= CharacterRotate;
        InputManager.UndoInputAction -= CharacterUndoCommand;
    }

    void CharacterRotate(Direction startDir, Direction targetDir) {
        Command command = new Command(()=>CharacterRotateCommand(startDir,targetDir),()=>CharacterRotateCommand(targetDir,startDir));
        characterCommands.Push(command);
        command.Execute();
    }
    void CharacterMove(bool canMove, Direction dir) {
        if(canMove) {
            Command command = new Command(()=>CharacterMoveCommand(dir),()=>CharacterMoveCommand(Extensions.ReverseDirection(dir)));
            characterCommands.Push(command);
            command.Execute();
        }else {
            //如果不能移动，说明前面是障碍物或者没有路之类的，可能后期会添加小提示之类的？
        }
    }
    void CharacterInteract(InteractionType interaction) { 
        Command command = new Command(()=>CharacterInteractCommand(interaction),
                                      ()=>CharacterInteractCommand(Extensions.ReverseInteractionType(interaction)));
        characterCommands.Push(command);
        command.Execute();
    }
    void CharacterRotateCommand(Direction startDir, Direction targetDir) {
        //玩家从之前的朝向转向新的朝向
    }
    void CharacterMoveCommand(Direction dir) {
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

}

