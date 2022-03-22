using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Utilities {
    public static Direction ReverseDirection(Direction dir) {
        switch (dir)
        {
            case Direction.UP:
            return Direction.DOWN;
            case Direction.DOWN:
            return Direction.UP;
            case Direction.LEFT:
            return Direction.RIGHT;
            case Direction.RIGHT:
            return Direction.LEFT;
        }
        return Direction.NONE;
    }

    public static Vector3 DirectionToVector(Direction dir) {
        switch (dir) {
            case Direction.UP:
            return Vector3.up;
            case Direction.DOWN:
            return Vector3.down;
            case Direction.LEFT:
            return Vector3.left;
            case Direction.RIGHT:
            return Vector3.right;
        }
        return Vector3.zero;
    }

    public static InteractionType ReverseInteractionType(InteractionType type) {
        switch(type) {
            case InteractionType.PICK_UP_ANIMALS:
            return InteractionType.PUT_DOWN_ANIMALS;
            case InteractionType.PUT_DOWN_ANIMALS:
            return InteractionType.PICK_UP_ANIMALS;
        }
        return InteractionType.NONE;
    }

    public static Vector3Int Vector3ToVector3Int(Vector3 vec) {
        return new Vector3Int(Mathf.RoundToInt(vec.x),Mathf.RoundToInt(vec.y),Mathf.RoundToInt(vec.z));
    }
    


}
