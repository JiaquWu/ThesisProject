using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonManager<InputManager>
{
    public static event Action<Direction> MoveInputAction;
    public static event Action InteractInputAction;
    public static event Action UndoInputAction;

    private float inputTimer;
    private float inputExecuteGap = 0.18f;

    void DetectInputs() {
        DetectMoveInputs();
        DetectInteractInputs();
        DetectUndoInputs();
    }
    void DetectMoveInputs() {
        inputTimer += Time.deltaTime;
        if(inputTimer < inputExecuteGap) return;      
        
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        if(new Vector2(x,y).magnitude <= float.Epsilon) return;
        Direction dir = Direction.NONE;
        if(Mathf.Abs(x) > Mathf.Abs(y)) {
            dir = x > 0?Direction.RIGHT:Direction.LEFT;
        }else if(Mathf.Abs(x) < Mathf.Abs(y)){
            dir = y > 0?Direction.UP:Direction.DOWN;
        }
        MoveInputAction.Invoke(dir);
        
        inputTimer = 0;
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
