using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Extensions {
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

    public static InteractionType ReverseInteractionType(InteractionType type) {
        switch(type) {
            case InteractionType.PICK_UP_ANIMALS:
            return InteractionType.PUT_DOWN_ANIMALS;
            case InteractionType.PUT_DOWN_ANIMALS:
            return InteractionType.PICK_UP_ANIMALS;
        }
        return InteractionType.NONE;
    }
    

}
