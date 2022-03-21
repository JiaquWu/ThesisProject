using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
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
        InputManager.MoveInputAction -= OnCharacterMoveInput;
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
            if(!IsPassable(dir)) return;
            List<ILevelObject> objects = GetObjectsOn(Utilities.DirectionToVector(dir) + transform.position);
            Command command = new Command();
            command.executeAction += ()=>CharacterMoveCommand(dir);
            command.undoAction += ()=>CharacterMoveCommand(Utilities.ReverseDirection(dir));
            foreach (var item in objects) {
               item.OnPlayerEnter(gameObject,ref command);
            }
            LevelManager.Instance.commandHandler.AddCommand(command); 
        }else {
            Command command = new Command(()=>CharacterRotateCommand(dir),()=>CharacterRotateCommand(CharacterDirection));
            LevelManager.Instance.commandHandler.AddCommand(command);  
            //转向
            
        }
        
    }
    void CharacterInteract(InteractionType interaction) { 
        Command command = new Command(()=>CharacterInteractCommand(interaction),
                                      ()=>CharacterInteractCommand(Utilities.ReverseInteractionType(interaction)));
        characterCommands.Push(command);
        command.Execute();
    }
    void CharacterRotateCommand(Direction targetDir) {
        //玩家从之前的朝向转向新的朝向
        CharacterDirection = targetDir;
        Debug.Log("玩家新的朝向是" + CharacterDirection);
    }
    void CharacterMoveCommand(Direction dir) {
        //玩家说了算
        Debug.Log("处理角色动画之类的东西");
        transform.position = Utilities.Vector3ToVector3Int(Utilities.DirectionToVector(dir) + transform.position);

    }
    void CharacterInteractCommand(InteractionType interaction) {
        Debug.Log("cc interact"+interaction);
        
    }
    void CharacterUndoCommand() {
       if(!LevelManager.Instance.commandHandler.Undo()) {
           Debug.Log("无销可撤");
       }
    }

    List<ILevelObject> GetObjectsOn(Vector3 pos) {
        List<GameObject> objects = LevelManager.GetObjectsOn(pos);
        List<ILevelObject> results = new List<ILevelObject>();
        foreach (var item in objects) {
           if(item.TryGetComponent(out ILevelObject levelObject)) {
               results.Add(levelObject);
           }
        }
        return results;
    }
    bool IsPassable(Direction dir) {
        List<ILevelObject> objects = GetObjectsOn(Utilities.DirectionToVector(dir) + transform.position);
        if(objects.Count == 0) return false;
        foreach (var item in objects) {
           if(!item.IsPassable(dir)) return false;
        }
        Debug.Log(objects.Count);
        return true;
    }

}

