using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterController : MonoBehaviour
{
    public Direction CharacterDirection{get;private set;}
    private Stack<Command> characterCommands = new Stack<Command>();

    private void Awake(){
        CharacterDirection = Direction.UP;
    }
    private void OnEnable() {

        InputManager.MoveInputAction += OnCharacterMoveInput;
        // TileManager.MoveAction += CharacterMove;
        // TileManager.InteractAction += CharacterInteract;
        // TileManager.RotateAction += CharacterRotate;
        InputManager.UndoInputAction += CharacterUndoCommand;
    }
    private void OnDisable() {
        // TileManager.MoveAction -= CharacterMove;
        // TileManager.InteractAction -= CharacterInteract;
        // TileManager.RotateAction -= CharacterRotate;
        InputManager.UndoInputAction -= CharacterUndoCommand;
    }

    void CharacterRotate(Direction startDir, Direction targetDir) {
        
    }
    void OnCharacterMoveInput(Direction dir) {
        //
        if(dir == CharacterDirection) {
            //移动
            // Command command = new Command(()=>CharacterMoveCommand(dir),()=>CharacterMoveCommand(Utilitys.ReverseDirection(dir)));
            // characterCommands.Push(command);
            // command.Execute();


            Command command = new Command();
            command.executeAction += ()=>CharacterMoveCommand(dir);
            command.undoAction += ()=>CharacterMoveCommand(Utilitys.ReverseDirection(dir));
            //获得目的地位置上的东西的引用,绑定execute和undo
            // Ground ground;
            // ground.OnPlayerEnter(gameObject,ref command);
            LevelManager.Instance.commandHandler.AddCommand(command); 
        }else {
            Command command = new Command(()=>CharacterRotateCommand(dir),()=>CharacterRotateCommand(CharacterDirection));
            LevelManager.Instance.commandHandler.AddCommand(command);  
            //转向
        }
        
    }
    void CharacterInteract(InteractionType interaction) { 
        Command command = new Command(()=>CharacterInteractCommand(interaction),
                                      ()=>CharacterInteractCommand(Utilitys.ReverseInteractionType(interaction)));
        characterCommands.Push(command);
        command.Execute();
    }
    void CharacterRotateCommand(Direction targetDir) {
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

    bool IsPassable(Direction dir) {
        Vector3 vec = transform.position + Utilitys.DirectionToVector(dir);
        List<GameObject> objects = LevelManager.GetObjectsOn(vec);
        if(objects.Count > 0) {
            for (int i = 0; i < objects.Count; i++) {
                if(!objects[i].GetComponent<LevelObject>().IsPassable()) return false;
            }
        }
        return true;
    }

}

