using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private void OnEnable() {
        GameManager.Instance.OnMove += Move;
    }
    private void OnDisable() {
        GameManager.Instance.OnMove -= Move;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Move(Direction dir) {
        //处理角色动画之类的东西

    }
}
