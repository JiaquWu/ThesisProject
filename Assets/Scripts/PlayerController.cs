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
    public Vector3 CharacterPosition{get;private set;}
    public IInteractable InteractableCharacterHold{get;private set;}
    private Stack<Command> characterCommands = new Stack<Command>();

    private List<Direction> moveInputPool;
    private Coroutine processMoveInputCoroutine;

    [SerializeField]
    private Sprite characterUpSprite;
    [SerializeField]
    private Sprite characterDownSprite;
    [SerializeField]
    private Sprite characterLeftSprite;
    [SerializeField]
    private Sprite characterRightSprite;
    private Dictionary<Direction,Sprite> characterSpritesDic;
    private void Awake(){
        characterSpritesDic = new Dictionary<Direction, Sprite>() {
                               {Direction.UP,characterUpSprite},
                               {Direction.DOWN,characterDownSprite},
                               {Direction.LEFT,characterLeftSprite},
                               {Direction.RIGHT,characterRightSprite}
        };
        CharacterRotateCommand(Direction.UP);
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
        LevelManager.OnPlayerDead += ()=>OnPlayerDead();
    }
    private void OnDisable() {
        
        
        mainInputAction.Gameplay.MoveUp.performed -= OnMoveUpPerformed;
        mainInputAction.Gameplay.MoveLeft.performed -= OnMoveLeftPerformed;
        mainInputAction.Gameplay.MoveDown.performed -= OnMoveDownPerformed;
        mainInputAction.Gameplay.MoveRight.performed -= OnMoveRightPerformed;

        mainInputAction.Gameplay.Interact.performed -= OnInteractPerformed;
        mainInputAction.Gameplay.Undo.performed -= OnUndoPerformed;
        mainInputAction.Disable();
        LevelManager.OnPlayerDead -= ()=>OnPlayerDead();
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
        if(LevelManager.Instance.IsPlayerDead) return;
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
        if(LevelManager.Instance.IsPlayerDead) return; 
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
        if(LevelManager.Instance.IsPlayerDead) return;    
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
        if(LevelManager.Instance.IsPlayerDead) return;         
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
        if(LevelManager.Instance.IsPlayerDead) return;
        OnCharacterInteractInput();
    }
    void OnUndoPerformed(InputAction.CallbackContext context) {
        LevelManager.Instance.UndoCheckPlayerDead();
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
            List<IPassable> objects = LevelManager.GetInterfaceOn<IPassable>(Utilities.DirectionToVector(dir) + transform.position);
            Command command = new Command();
            command.executeAction += ()=>CharacterMoveCommand(dir);
            command.undoAction += ()=>CharacterMoveCommand(Utilities.ReverseDirection(dir));
            foreach (var item in objects) {
               item.OnPlayerEnter(gameObject,ref command);
            }
            LevelManager.Instance.commandHandler.AddCommand(command); 
        }else {
            Direction temp = CharacterDirection;
            Command command = new Command(()=>CharacterRotateCommand(dir),()=>CharacterRotateCommand(temp));
            LevelManager.Instance.commandHandler.AddCommand(command);  
            //转向        
        }  
    }
    void OnCharacterInteractInput() { 
        InteractionType interaction = InteractionType.NONE;
        Command command = new Command();
        Debug.Log("当前动物是"+InteractableCharacterHold);
        if(InteractableCharacterHold != null) {
            if(IsPlaceable(out IPlaceable placeable)) {  
                //手中的动物放在当前的格子
                IInteractable temp = InteractableCharacterHold;
                interaction = InteractionType.PUT_DOWN_ANIMALS;
                command.executeAction += ()=>CharacterInteractCommand(interaction,temp);
                command.undoAction += ()=>CharacterInteractCommand(Utilities.ReverseInteractionType(interaction),temp);
                InteractableCharacterHold.OnPlayerInteract(interaction,placeable,gameObject,ref command);
                List<IPlaceable> placeables = LevelManager.GetInterfaceOn<IPlaceable>((Utilities.DirectionToVector(CharacterDirection)) + transform.position);
                foreach (var item in placeables) {
                    item.OnPlayerPlace(temp,ref command);
                }
                LevelManager.Instance.commandHandler.AddCommand(command);     
            }
        }else {
            if(!IsInteractable()) return;
            interaction = InteractionType.PICK_UP_ANIMALS;
            List<IPlaceable> placeables = LevelManager.GetInterfaceOn<IPlaceable>((Utilities.DirectionToVector(CharacterDirection)) + transform.position);
            IPlaceable temp = placeables[0];//这里是要获取主角要交互物体前面的地面是什么 普通地面还是船
            List<IInteractable> objects = LevelManager.GetInterfaceOn<IInteractable>((Utilities.DirectionToVector(CharacterDirection)) + transform.position);
            foreach (var item in objects) {
                item.OnPlayerInteract(interaction,temp,gameObject,ref command);             
            }
            command.executeAction += ()=>CharacterInteractCommand(interaction,objects[0]);
            command.undoAction += ()=>CharacterInteractCommand(Utilities.ReverseInteractionType(interaction),objects[0]);
            //前面已经检验过了,objects如果一个元素都没有就return了,但是如果有两个可以interact的物体还是会出问题
            //这里还有一件事要做,就是玩家拿起来的时候实际上会把地板压一下
            List<Ground> grounds = LevelManager.GetInterfaceOn<Ground>(transform.position);
            foreach (var item in grounds) {
               command.executeAction += ()=>item.OnBreakingGround(true);
               command.undoAction += ()=>item.OnBreakingGround(false);//这里的true可以看objects[0]具体是什么动物
            }
            LevelManager.Instance.commandHandler.AddCommand(command);
        }  
    }
    void CharacterRotateCommand(Direction targetDir) {
        //玩家从之前的朝向转向新的朝向
        CharacterDirection = targetDir;
        if(!TryGetComponent<SpriteRenderer>(out SpriteRenderer renderer) || !characterSpritesDic.TryGetValue(targetDir, out Sprite sprite))  return;
        renderer.sprite = sprite;
        Debug.Log(sprite);
        Debug.Log("玩家新的朝向是" + CharacterDirection);
        //改变方向
    }
    void CharacterMoveCommand(Direction dir) {
        //玩家说了算
        Debug.Log("处理角色动画之类的东西");
        transform.position = Utilities.Vector3ToVector3Int(Utilities.DirectionToVector(dir) + transform.position);
        CharacterPosition = transform.position;
    }
    void CharacterInteractCommand(InteractionType interaction,IInteractable interactableItem) {
        Debug.Log("interactableItem"+interactableItem);
        switch (interaction) {
            case InteractionType.PICK_UP_ANIMALS:
            InteractableCharacterHold = interactableItem;
            break;
            case InteractionType.PUT_DOWN_ANIMALS:
            InteractableCharacterHold = null;
            break;
        }
    }
    void CharacterUndoCommand() {
       if(!LevelManager.Instance.commandHandler.Undo()) {
           Debug.Log("无销可撤");
       }
       Debug.Log("当前动物是"+InteractableCharacterHold);
    }

    void OnPlayerDead() {
        moveInputPool.Clear();
    }
    bool IsPassable(Direction dir) {
        List<GameObject> objects = LevelManager.GetObjectsOn(Utilities.DirectionToVector(dir) + transform.position);
        if(objects.Count == 0) return false;
        foreach (var item in objects) {
           if(!item.TryGetComponent<IPassable>(out IPassable passable)) return false;//如果有一个没有这个接口那就不能通过
           if(!passable.IsPassable(dir)) return false;//
        }
        return true;
    }
    bool IsPlaceable(out IPlaceable placeable) {//放置应该不需要判断方向,一个地方能不能放东西,还要判断这个地方没有其他东西
        IPlaceable temp = null;
        List<GameObject> objects = LevelManager.GetObjectsOn(Utilities.DirectionToVector(CharacterDirection) + transform.position);
        if(objects.Count == 0) {
            placeable = null;
            return false;
        }
        foreach (var item in objects) {
            if(!item.TryGetComponent<IPlaceable>(out IPlaceable place)) {
                placeable = null;
                return false;
            }else {
                temp = place;//一个地方最多有一个iplaceable,所以不会替换
            }
        }
        placeable = temp;
        return true;
    }

    bool IsInteractable() {
        //有一个能交互的就行   
        List<IInteractable> objects = LevelManager.GetInterfaceOn<IInteractable>(Utilities.DirectionToVector(CharacterDirection) + transform.position);
        if(objects.Count == 0) return false;
        return true;
    }

}

