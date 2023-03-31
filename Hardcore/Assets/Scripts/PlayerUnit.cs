using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    public string unitName;
    public int level;
    public int damageAttack1;
    public int damageAttack2;
    
    public float maxHP;
    public float currentHP;
    public float shield =0;//porcentagem
    

    public bool TakeDamage(float dmg)
    {
        if (shield > 0)
        {
            dmg -= dmg*shield;
            currentHP -= dmg;
        }
        else
        {
            currentHP -= dmg;
        }
        
        
        if (currentHP <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }

        


    }
}
