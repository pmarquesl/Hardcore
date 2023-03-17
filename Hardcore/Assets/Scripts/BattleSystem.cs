using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{
    public enum BattleState{ START,PLAYERTURN,ENEMYTURN,WON,LOST}

    public Animator playerAnimAttack;
    public Animator enemyAnimAttack;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform EnemyBattleStation;

    private Unit playerUnit;
    private Unit enemyUnit;

    public Text startText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;

    public GameObject attackBT;
    public GameObject skipBT;
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());



    }

    IEnumerator SetupBattle()
    {
       GameObject playerGO = Instantiate(playerPrefab, playerBattleStation.transform);
       playerUnit = playerGO.GetComponent<Unit>();
       playerAnimAttack = playerGO.GetComponent<Animator>();
       GameObject enemyGO =  Instantiate(enemyPrefab, EnemyBattleStation.transform);
       enemyUnit = enemyGO.GetComponent<Unit>();
       enemyAnimAttack = enemyGO.GetComponent<Animator>();
       

       startText.text = "A Wild " + enemyUnit.unitName + " appeared";
       
       playerHUD.SetHUD(playerUnit);
       enemyHUD.SetHUD(enemyUnit);
       yield return new WaitForSeconds(2);
       
       state = BattleState.PLAYERTURN;
       PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
       bool isDead =  enemyUnit.TakeDamage(playerUnit.damage);
       startText.text = playerUnit.unitName + " is Attacking";
       
      
       playerAnimAttack.SetBool("Attacking",true);
       attackBT.SetActive(false);
       skipBT.SetActive(false);
       yield return new WaitForSeconds(1f);

       startText.text = "The attack is Successful";

        yield return new WaitForSeconds(1);
        playerAnimAttack.SetBool("Attacking",false);
        enemyHUD.SetHP(enemyUnit.currentHP);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        startText.text = enemyUnit.unitName + " attacks";
        enemyAnimAttack.SetBool("Attacking",true);
        yield return new WaitForSeconds(1f);
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        
        
        
        yield return new WaitForSeconds(1f);
        playerHUD.SetHP(playerUnit.currentHP);
        enemyAnimAttack.SetBool("Attacking",false);
        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }
    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            startText.text = "You won the Battle";
            playerAnimAttack.SetTrigger("BackFlip");
        }else if(state == BattleState.LOST)
        {
            startText.text = "You lost the Battle";
            enemyAnimAttack.SetTrigger("EBackFlip");
        }
    }
    void PlayerTurn()
    {
        startText.text = "Choose an action: ";
        attackBT.SetActive(true);
        skipBT.SetActive(true);
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;

        }
        StartCoroutine(PlayerAttack());

    }

    public void SkipButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;

        }
        StartCoroutine(EnemyTurn());
    }
    
}
