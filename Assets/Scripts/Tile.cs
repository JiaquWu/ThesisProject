using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public enum TileState {
    NORMAL,
    OBSTACLE,
    ANIMAL,
    CHARACTER,
    CHARACTER_WITH_ANIMAL,
    DESTINATION
}


public class Tile
{
    private int tileHealth;
    public int TileHealth => tileHealth;
    private Vector2 coordinate;
    public Vector2 Coordinate => coordinate;

    private TileState currentTileState;
    public TileState CurrentTileState => currentTileState;
    // Start is called before the first frame update

    public void SetCoordinate(Vector2 co) {
        coordinate = co;
    }

    
}
