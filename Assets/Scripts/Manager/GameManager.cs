using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Direction {
    NONE,
    UP,
    DOWN,
    LEFT,
    RIGHT
}
public enum InteractionType {
    PICK_UP_ANIMALS,
    PUT_DOWN_ANIMALS
}
public enum ActionType {
    ROTATE,
    MOVE,
    INTERACT
}
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance {
        get{
            if(!instance) {
                instance = FindObjectOfType<GameManager>();
                if(!instance) {
                    Debug.LogError("There is no GameManager object in any of the currently loaded scenes");
                }
            }
            return instance;
        }
    }

    public static event Action<Direction> MoveInputAction;
    public static event Action InteractInputAction;
    public static event Action UndoInputAction;

    private float inputTimer;
    private float inputExecuteGap = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        DetectInputs();
    }

    void DetectInputs() {
        DetectMoveInputs();
        DetectInteractInputs();
        DetectUndoInputs();
    }

    void DetectMoveInputs() {
        inputTimer += Time.deltaTime;
        if(inputTimer < inputExecuteGap) return;
        inputTimer = 0;

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        if(new Vector2(x,y).magnitude <= float.Epsilon) return;
        Direction dir = Direction.NONE;
        if(Mathf.Abs(x) > Mathf.Abs(y)) {
            dir = x > 0?Direction.RIGHT:Direction.LEFT;
        }else if(Mathf.Abs(x) < Mathf.Abs(y)){
            dir = y > 0?Direction.UP:Direction.DOWN;
        }
        Debug.Log(dir);
        MoveInputAction.Invoke(dir);
    }
    void DetectInteractInputs() {
        if(Input.GetButtonDown("Jump")) {
            InteractInputAction.Invoke();
        }
    }
    void DetectUndoInputs() {
        if(Input.GetButtonDown("Undo")) {
            UndoInputAction.Invoke();
        }
    }
}
