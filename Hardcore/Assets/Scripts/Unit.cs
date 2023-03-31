using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int level;
    public int damage;
    public GameObject enemyCollider;
    public float maxHP;
    public float currentHP;
    public bool isAlive = true;
    

    public void TakeDamage(float dmg)
    {
       
            currentHP -= dmg;
        
        
       
        if (currentHP <= 0)
        {
            isAlive = false;
            enemyCollider.SetActive(false);
            BattleSystem.BS.aliveEnemies--;
            this.gameObject.SetActive(false);
        }
        else
        {
            isAlive = true;
        }

        


    }

}
