using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;
    private void Awake() {
        instance = this;
    }
    public event Action<Direction> OnMove;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        DetectInputs();
    }

    void DetectInputs() {  
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        if(new Vector2(x,y).magnitude <= float.Epsilon) return;
        Direction dir;
        if(Mathf.Abs(x) > Mathf.Abs(y)) {
            dir = x > 0?Direction.RIGHT:Direction.LEFT;
        }else {
            dir = y > 0?Direction.UP:Direction.DOWN;
        }
        Debug.Log(dir);

    }
}
