using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameMgr : MonoBehaviour {

    //게임 시작 여부
    [HideInInspector]
    public bool gameStart = false;

    private void Update()
    {
        if(gameStart != true)
        {
            
            return;
        }
        else
        {
            
        }
    }
}
