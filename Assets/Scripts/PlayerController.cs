using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    private IA_Main mainInputAction;
    public Direction CharacterDirection{get;private set;}
    public Animal AnimalCharacterHold{get;private set;}
    private Stack<Command> characterCommands = new Stack<Command>();

    private List<Direction> moveInputPool;
    private Coroutine processMoveInputCoroutine;

    private void Awake(){
        CharacterDirection = Direction.UP;
        moveInputPool=new List<Direction>();
    }
    private void OnEnable() {
        mainInputAction = new IA_Main();
        mainInputAction.Enable();
        
        
        mainInputAction.Gameplay.MoveUp.performed += OnMoveUpPerformed;
        mainInputAction.Gameplay.MoveLeft.performed += OnMoveLeftPerformed;
        mainInputAction.Gameplay.MoveDown.performed += OnMoveDownPerformed;
        mainInputAction.Gameplay.MoveRight.performed += OnMoveRightPerformed;

        mainInputAction.Gameplay.Interact.performed += OnInteractPerformed;
        mainInputAction.Gameplay.Undo.performed += OnUndoPerformed;
        // InputManager.MoveInputAction += OnCharacterMoveInput;
        // TileManager.MoveAction += CharacterMove;
        // TileManager.InteractAction += CharacterInteract;
        // TileManager.RotateAction += CharacterRotate;
        // InputManager.UndoInputAction += CharacterUndoCommand;
    }
    private void OnDisable() {
        
        
        mainInputAction.Gameplay.MoveUp.performed -= OnMoveUpPerformed;
        mainInputAction.Gameplay.MoveLeft.performed -= OnMoveLeftPerformed;
        mainInputAction.Gameplay.MoveDown.performed -= OnMoveDownPerformed;
        mainInputAction.Gameplay.MoveRight.performed -= OnMoveRightPerformed;

        mainInputAction.Gameplay.Interact.performed -= OnInteractPerformed;
        mainInputAction.Gameplay.Undo.performed -= OnUndoPerformed;
        mainInputAction.Disable();

        //InputManager.MoveInputAction -= OnCharacterMoveInput;
        // TileManager.MoveAction -= CharacterMove;
        // TileManager.InteractAction -= CharacterInteract;
        // TileManager.RotateAction -= CharacterRotate;
        //InputManager.UndoInputAction -= CharacterUndoCommand;
    }
    
    
    IEnumerator ProcessMoveInput() {
        OnCharacterMoveInput(moveInputPool[moveInputPool.Count - 1]);
        yield return new WaitForSeconds(moveInputPool.Count <= 1?0.5f:0.1f);

        while (moveInputPool.Count > 0) {
            OnCharacterMoveInput(moveInputPool[moveInputPool.Count - 1]);
            yield return new WaitForSeconds(0.1f);
        }
    }
    void OnMoveUpPerformed(InputAction.CallbackContext context) {         
        if (context.ReadValueAsButton()) {
            // 按下
            if (!moveInputPool.Contains(Direction.UP)) {
                moveInputPool.Add(Direction.UP);
                //瞬间执行一次
                if(processMoveInputCoroutine != null) StopCoroutine(processMoveInputCoroutine);
                processMoveInputCoroutine = StartCoroutine(ProcessMoveInput());
            }
        }else {
            // 抬起
            moveInputPool.Remove(Direction.UP);
        }
    }
    void OnMoveLeftPerformed(InputAction.CallbackContext context) {     
        if (context.ReadValueAsButton()) {
            // 按下
            if (!moveInputPool.Contains(Direction.LEFT)) {
                moveInputPool.Add(Direction.LEFT);
                //瞬间执行一次
                if(processMoveInputCoroutine != null) StopCoroutine(processMoveInputCoroutine);
                processMoveInputCoroutine = StartCoroutine(ProcessMoveInput());
            }
        }else {
            // 抬起
            moveInputPool.Remove(Direction.LEFT);
        }    
    }
    void OnMoveDownPerformed(InputAction.CallbackContext context) {        
        if (context.ReadValueAsButton()) {
            // 按下
            if (!moveInputPool.Contains(Direction.DOWN)) {
                moveInputPool.Add(Direction.DOWN);
                //瞬间执行一次
                if(processMoveInputCoroutine != null) StopCoroutine(processMoveInputCoroutine);
                processMoveInputCoroutine = StartCoroutine(ProcessMoveInput());
            }
        } 
        else {
            // 抬起
            moveInputPool.Remove(Direction.DOWN);
        }    
    }
    void OnMoveRightPerformed(InputAction.CallbackContext context) {         
        if (context.ReadValueAsButton()) {
            // 按下
            if (!moveInputPool.Contains(Direction.RIGHT)) {
                moveInputPool.Add(Direction.RIGHT);
                //瞬间执行一次
                if(processMoveInputCoroutine != null) StopCoroutine(processMoveInputCoroutine);
                processMoveInputCoroutine = StartCoroutine(ProcessMoveInput());
            }
        }else {
            // 抬起
            moveInputPool.Remove(Direction.RIGHT);
        }  
    }
    void OnInteractPerformed(InputAction.CallbackContext context) {
        OnCharacterInteractInput();
    }
    void OnUndoPerformed(InputAction.CallbackContext context) {
        Debug.Log(context.ReadValueAsButton());
        CharacterUndoCommand();          
    }
    void OnCharacterMoveInput(Direction dir) {
        //
        if(dir == CharacterDirection) {
            //移动
            // Command command = new Command(()=>CharacterMoveCommand(dir),()=>CharacterMoveCommand(Utilitys.ReverseDirection(dir)));
            // characterCommands.Push(command);
            // command.Execute();
            if(!IsPassable(dir)) return;
            List<ILevelObject> objects = LevelManager.GetInterfaceOn<ILevelObject>(Utilities.DirectionToVector(dir) + transform.position);
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
    void OnCharacterInteractInput() { 
        InteractionType interaction = InteractionType.NONE;
        Command command = new Command();
        if(AnimalCharacterHold != null) {
            if(IsPassable(CharacterDirection)) {
                //手中的动物放在当前的格子
                interaction = InteractionType.PUT_DOWN_ANIMALS;
                command.executeAction += ()=>CharacterInteractCommand(interaction);
                command.undoAction += ()=>CharacterInteractCommand(Utilities.ReverseInteractionType(interaction));
                AnimalCharacterHold.OnPlayerInteract(interaction,gameObject,ref command);
            }
        }else {
            if(!IsInteractable()) return;
            interaction = InteractionType.PICK_UP_ANIMALS;
            List<IInteractable> objects = LevelManager.GetInterfaceOn<IInteractable>((Utilities.DirectionToVector(CharacterDirection)) + transform.position);
            foreach (var item in objects) {
                item.OnPlayerInteract(interaction,gameObject,ref command);
            }
            command.executeAction += ()=>CharacterInteractCommand(interaction);
            command.undoAction += ()=>CharacterInteractCommand(Utilities.ReverseInteractionType(interaction));
        }
        
        //()=>CharacterInteractCommand(interaction),()=>CharacterInteractCommand(Utilities.ReverseInteractionType(interaction))
        
        LevelManager.Instance.commandHandler.AddCommand(command);
    }
    void CharacterRotateCommand(Direction targetDir) {
        //玩家从之前的朝向转向新的朝向
        CharacterDirection = targetDir;
        Debug.Log("玩家新的朝向是" + CharacterDirection);
        //改变方向
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
    bool IsPassable(Direction dir) {
        List<ILevelObject> objects = LevelManager.GetInterfaceOn<ILevelObject>(Utilities.DirectionToVector(dir) + transform.position);
        if(objects.Count == 0) return false;
        foreach (var item in objects) {//如果有一个不能通关那就不能通过
           if(!item.IsPassable(dir)) return false;
        }
        Debug.Log(objects.Count);
        return true;
    }

    bool IsInteractable() {   
        List<IInteractable> objects = LevelManager.GetInterfaceOn<IInteractable>(Utilities.DirectionToVector(CharacterDirection) + transform.position);
        if(objects.Count == 0) return false;
        foreach (var item in objects) {//如果有一个不能通关那就不能通过
           if(!item.IsInteractable(gameObject)) return false;
        }
        Debug.Log(objects.Count);
        return true;
    }

}

