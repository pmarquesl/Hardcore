using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
   public Text nameText;
   public Text leveltext;
   public Slider hpSlider;

   public void SetEnemyHUD(Unit unit)
   {
      nameText.text = unit.unitName;
      leveltext.text = "Lvl " + unit.level;
      hpSlider.maxValue = unit.maxHP;
      hpSlider.value = unit.currentHP;

   }
   public void SetPlayerHUD(PlayerUnit playerUnit)
   {
      nameText.text = playerUnit.unitName;
      leveltext.text = "Lvl " + playerUnit.level;
      hpSlider.maxValue = playerUnit.maxHP;
      hpSlider.value = playerUnit.currentHP;

   }

   public void SetHP(float hp)
   {
      hpSlider.value = hp;
   }
}
