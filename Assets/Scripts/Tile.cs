using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Vector2 coordinate;
    public Vector2 Coordinate => coordinate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetCoordinate(Vector2 co) {
        coordinate = co;
    }

    public Vector2 GetNeighborTileCoByDir(Direction dir) {
        switch (dir) {
            case Direction.UP:
            return new Vector2(coordinate.x,coordinate.y+1);
            case Direction.DOWN:
            return new Vector2(coordinate.x,coordinate.y-1);
            case Direction.LEFT:
            return new Vector2(coordinate.x-1,coordinate.y);
            case Direction.RIGHT:
            return new Vector2(coordinate.x+1,coordinate.y);          
        }
        return Vector2.zero;

    }
}
