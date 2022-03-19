using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private Tile currentTile;
    private List<Tile> allTilesInCurrentLevel;

    // Start is called before the first frame update
    private void OnEnable() {
        GameManager.Instance.OnMove += Move;
    }
    private void OnDisable() {
        GameManager.Instance.OnMove -= Move;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Tile GetNextTileByDir(Tile tile, Direction dir) {
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
            if(allTilesInCurrentLevel[i].Coordinate == vec) {
                return allTilesInCurrentLevel[i];
            }            
        }
        return null;

    }
    void Move(Direction dir) {

    }
}
