using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTarget : MonoBehaviour
{
    public int targetNumber;
    
    void OnMouseDown () {
        if (Input.GetKey ("mouse 0"))
        {
            BattleSystem.BS.target = targetNumber;
        }
    }
}
