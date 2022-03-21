using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TileState {
    NORMAL,
    OBSTACLE,
    ANIMAL,
    CHARACTER,
    CHARACTER_WITH_ANIMAL,
    DESTINATION,
    BROKEN
}
public class Tile:MonoBehaviour
{
    private int tileHealth;
    public int TileHealth => tileHealth;
    private Vector2 coordinate;
    public Vector2 Coordinate => coordinate;

    private TileState currentTileState;
    public TileState CurrentTileState => currentTileState;
    // Start is called before the first frame update

    public void SetTileHealth(int health) {
        tileHealth = health;
    }
    public bool ReduceTileHealth() {
        return --tileHealth > 0;
    } 
    public void SetCoordinate(Vector2 co) {
        coordinate = co;
    }

    public void ChangeCurrentTileState(TileState state) {
        currentTileState = state;
    }

    
}

