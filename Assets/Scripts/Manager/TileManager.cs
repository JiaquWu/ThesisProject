using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : SingletonManager<TileManager>
{

    public static event Action<Direction,Direction> RotateAction;
    public static event Action<bool,Direction> MoveAction;
    public static event Action<InteractionType> InteractAction;
    private Stack<Command> tileCommands = new Stack<Command>();

    [SerializeField]
    private GameObject tile_Prefab;
    private LevelData levelData;
    [SerializeField]
    private LevelConfigSO currentLevelConfig;
    private Tile currentTile;
    private List<GameObject> allTilesInCurrentLevel = new List<GameObject>();
    private Direction currentDirection;
    public Direction CurrentDirection => currentDirection;

        private void OnEnable() {
        InputManager.MoveInputAction += TryToMove;
        InputManager.InteractInputAction += TryToInteract;
        InputManager.UndoInputAction += TileUndo;

        RotateAction += TileRotate;
        MoveAction += TileMove;
        InteractAction += TileInteract;
    }
    private void OnDisable() {
        InputManager.MoveInputAction -= TryToMove;
        InputManager.InteractInputAction -= TryToInteract;
        InputManager.UndoInputAction -= TileUndo;

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
                currentTile = tile;
                break;
                case TileState.ANIMAL:
                break;
                case TileState.OBSTACLE:
                break;
                case TileState.DESTINATION:
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

        if(currentDirection != dir) {
            RotateAction.Invoke(currentDirection,dir);                
            return;
        }

        if(currentTile.CurrentTileState == TileState.CHARACTER_WITH_ANIMAL) {
            if(currentTile.ReduceTileHealth()) {
                currentTile.ChangeCurrentTileState(TileState.NORMAL);
            }else {
                currentTile.ChangeCurrentTileState(TileState.BROKEN);
            }
        }
        GameObject go = GetNextTileByDir(currentTile, dir);
        if(go != null) {
            Tile tile = go.GetComponent<Tile>();
            if(!tile) return;
            if(tile.CurrentTileState == TileState.NORMAL) {
                MoveAction.Invoke(true,dir);
                
            }
            //go.GetComponent<Tile>().CurrentTileState
        }
    }
    void TryToInteract() {
        Debug.Log("tilemanager interact");
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
            Command command = new Command(()=>TileMoveCommand(dir),()=>TileMoveCommand(Utilities.ReverseDirection(dir)));
            tileCommands.Push(command);
            command.Execute();
        }else {
            //
        }
    }
    void TileInteract(InteractionType interaction) {
        Command command = new Command(()=>TileInteractCommand(interaction),()=>TileInteractCommand(Utilities.ReverseInteractionType(interaction)));
        tileCommands.Push(command);
        command.Execute();
    }
    void TileUndoCommand() {
    }

    void TileRotateCommand(Direction startDir, Direction targetDir) {
        currentDirection = targetDir;
    }
    void TileMoveCommand(Direction dir) {
    }
    void TileInteractCommand(InteractionType interaction) {
    }
}
