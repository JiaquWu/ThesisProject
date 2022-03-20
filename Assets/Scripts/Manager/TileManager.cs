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

    //public static event Action UndoAction;
    [SerializeField]
    private GameObject tile_Prefab;
    private LevelConfigSO currentLevelConfig;
    private Tile currentTile;
    private List<GameObject> allTilesInCurrentLevel;
    private Direction currentDirection;
    public Direction CurrentDirection => currentDirection;

    // Start is called before the first frame update
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

    void GenerateAllTiles() {
        if(currentLevelConfig.tileInfos.Count == 0) return;
        allTilesInCurrentLevel.Clear();
        for (int i = 0; i < currentLevelConfig.tileInfos.Count; i++){
            GameObject go = Instantiate(tile_Prefab,gameObject.transform);
            allTilesInCurrentLevel.Add(go);
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
        }
        currentTile.ChangeCurrentTileState(TileState.NORMAL);
        Debug.Log("处理地图逻辑之类的东西");
        GameObject go = GetNextTileByDir(currentTile, dir);
        if(go != null) {
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
        Debug.Log("TileUndo");
    }

    void TileRotate(Direction startDir, Direction targetDir) {
        currentDirection = targetDir;
    }
    void TileMove(bool canMove, Direction dir) {

    }
    void TileInteract(InteractionType interaction) {

    }

}
// public class TileCommand:Command{
//     public TileCommand() {

//     }
//     public override void Execute() {

//     }
//     public override void Undo() {
        
//     }
// }
