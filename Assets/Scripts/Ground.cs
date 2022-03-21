using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : LevelObject {
    int maxHealth;
    int currentHealth;
    private void Awake() {
        LevelManager.RegisterObject(gameObject);
    }
    public override bool IsPassable()
    {
        return true;
    }
    public void OnPlayerEnter(GameObject player,ref Command command) {  
        //判断玩家当前举着什么动物,来给command绑定不同的action
        //没有任何动物,不绑定任何action
        //如果一滴血,就是command.executeAction += ()=> currentHealth -= 1;

    }



}
