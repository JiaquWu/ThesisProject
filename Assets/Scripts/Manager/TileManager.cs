using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private static TileManager instance;
    public static TileManager Instance {
        get{
            if(!instance) {
                instance = FindObjectOfType<TileManager>();
                if(!instance) {
                    Debug.LogError("There is no GameManager object in any of the currently loaded scenes");
                }
            }
            return instance;
        }
    }
    public static event Action<Direction,Direction> RotateAction;
    public static event Action<bool,Direction> MoveAction;
    public static event Action<InteractionType> InteractAction;
    private Stack<Command> tileCommands = new Stack<Command>();

    [SerializeField]
    private GameObject tile_Prefab;
    [SerializeField]
    private LevelConfigSO currentLevelConfig;
    private Tile currentTile;
    private List<GameObject> allTilesInCurrentLevel = new List<GameObject>();
    private Direction currentDirection;
    public Direction CurrentDirection => currentDirection;

        private void OnEnable() {
        GameManager.MoveInputAction += TryToMove;
        GameManager.InteractInputAction += TryToInteract;
        GameManager.UndoInputAction += TileUndo;

        RotateAction += TileRotate;
        MoveAction += TileMove;
        InteractAction += TileInteract;
    }
    private void OnDisable() {
        GameManager.MoveInputAction -= TryToMove;
        GameManager.InteractInputAction -= TryToInteract;
        GameManager.UndoInputAction -= TileUndo;

        RotateAction -= TileRotate;
        MoveAction -= TileMove;
        InteractAction -= TileInteract;
    }

    private void Start()
    {
        GenerateAllTiles();
        GenerateTileConfigs();
    }

    void GenerateAllTiles() {
        if(currentLevelConfig?.tileInfos.Count == 0) return;
        allTilesInCurrentLevel.Clear();
        for (int i = 0; i < currentLevelConfig.tileInfos.Count; i++){
            GameObject go = Instantiate(tile_Prefab,gameObject.transform);
            allTilesInCurrentLevel.Add(go);
            go.GetComponent<Tile>()?.ChangeCurrentTileState(currentLevelConfig.tileInfos[i].tileState);
            go.GetComponent<Tile>()?.SetCoordinate(currentLevelConfig.tileInfos[i].coordinate);
            go.GetComponent<Tile>()?.SetTileHealth(currentLevelConfig.tileInfos[i].tileHealth);
            go.transform.position = (Vector2)(go.GetComponent<Tile>()?.Coordinate + currentLevelConfig.offset);
            go.transform.localScale *= currentLevelConfig.scaleFactor; 
        }

    }

    void GenerateTileConfigs() {

        for (int i = 0; i < allTilesInCurrentLevel.Count; i++) {
            Tile tile = allTilesInCurrentLevel[i].GetComponent<Tile>();
            if(!tile) return;
            switch (tile.CurrentTileState)
            {
                case TileState.CHARACTER:
                Debug.Log("让主角在这里");
                currentTile = tile;
                break;
                case TileState.ANIMAL:
                Debug.Log("让动物在这里");
                break;
                case TileState.OBSTACLE:
                Debug.Log("让石头在这里");
                break;
                case TileState.DESTINATION:
                Debug.Log("让终点在这里");
                break;
                case TileState.NORMAL:
                Debug.Log(allTilesInCurrentLevel[i].GetComponent<Tile>()?.TileHealth);
                break;
            }
        }
    }
    public GameObject GetNextTileByDir(Tile tile, Direction dir) {
        Vector2 vec = Vector2.zero;
        switch (dir) {
            case Direction.UP:
            vec = Vector2.up;
            break;
            case Direction.DOWN:
            vec = Vector2.down;
            break;
            case Direction.LEFT:
            vec = Vector2.left;
            break;
            case Direction.RIGHT:
            vec = Vector2.right;
            break;
        }
        vec += tile.Coordinate;
        for (int i = 0; i < allTilesInCurrentLevel.Count; i++) {
            if(allTilesInCurrentLevel[i].GetComponent<Tile>().Coordinate == vec) {
                return allTilesInCurrentLevel[i];
            }            
        }
        return null;
    }
    void TryToMove(Direction dir) {
        
        //处理地图逻辑之类的东西
        if(currentDirection != dir) {
            RotateAction.Invoke(currentDirection,dir);//玩家转向目标方向                         
            return;
        }

        if(currentTile.CurrentTileState == TileState.CHARACTER_WITH_ANIMAL) {
            //格子融化一些
            if(currentTile.ReduceTileHealth()) {
                currentTile.ChangeCurrentTileState(TileState.NORMAL);
            }else {
                currentTile.ChangeCurrentTileState(TileState.BROKEN);
            }
        }
        
        Debug.Log("处理地图逻辑之类的东西");
        GameObject go = GetNextTileByDir(currentTile, dir);
        if(go != null) {
            Tile tile = go.GetComponent<Tile>();
            if(!tile) return;
            if(tile.CurrentTileState == TileState.NORMAL) {
                MoveAction.Invoke(true,dir);
            }else {
                MoveAction.Invoke(false,dir);
            }
            //go.GetComponent<Tile>().CurrentTileState
        }
        //MoveAction.Invoke(); 前提是有路可走
    }
    void TryToInteract() {
        Debug.Log("tilemanager interact");
        //如果前面能走 判断何种交互类型
        InteractAction.Invoke(InteractionType.PICK_UP_ANIMALS);

    }

    void TileUndo() {
        if(tileCommands.Count > 0) {
            Command command = tileCommands.Pop();
            command.Undo();
        }
    }

    void TileRotate(Direction startDir, Direction targetDir) {
        Command command = new Command(()=>TileRotateCommand(startDir,targetDir),()=>TileRotateCommand(targetDir,startDir));
        tileCommands.Push(command);
        command.Execute();
        
    }
    void TileMove(bool canMove, Direction dir) {
        if(canMove) {
            Command command = new Command(()=>TileMoveCommand(dir),()=>TileMoveCommand(Extensions.ReverseDirection(dir)));
            tileCommands.Push(command);
            command.Execute();
        }else {
            //
        }
    }
    void TileInteract(InteractionType interaction) {
        Command command = new Command(()=>TileInteractCommand(interaction),()=>TileInteractCommand(Extensions.ReverseInteractionType(interaction)));
        tileCommands.Push(command);
        command.Execute();
    }
    void TileUndoCommand() {
        Debug.Log("TileUndo");
    }

    void TileRotateCommand(Direction startDir, Direction targetDir) {
        currentDirection = targetDir;
        Debug.Log("转向" + targetDir);
    }
    void TileMoveCommand(Direction dir) {
        Debug.Log("走了一部" + currentTile.Coordinate);
    }
    void TileInteractCommand(InteractionType interaction) {
        Debug.Log("交互了" + interaction);
    }
}
