using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem BS { get; private set;}
    private void Awake()
    {   if (BS != null && BS!= this)
        {
            Destroy(gameObject);
        }
        else
        {
            BS = this;
        }
        DontDestroyOnLoad(this.gameObject);
      
    }
    
    public enum BattleState
    {
        START,
        PLAYERTURN,
        ENEMYTURN,
        SELECTATTACK,
        WON,
        LOST
    }

    public int aliveEnemies =2;
    public int target=0;



    public Animator playerAnimAttack;
    public Animator enemyAnimAttack1;
    public Animator enemyAnimAttack2;

    public GameObject playerPrefab;
    public GameObject[] enemyPrefabs;

    public Transform playerBattleStation;
    public Transform EnemyBattleStation1;
    public Transform EnemyBattleStation2;

    

    private PlayerUnit playerUnit;
    private Unit enemyUnit1;
    private Unit enemyUnit2;

    public Text startText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD1;
    public BattleHUD enemyHUD2;
    

    public BattleState state;

    public GameObject attack1BT;
    public GameObject attack2BT;
    public GameObject shieldBT;
    
    

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

   
    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation.transform);
        playerUnit = playerGO.GetComponent<PlayerUnit>();
        playerAnimAttack = playerGO.GetComponent<Animator>();
        GameObject enemyGo1 = Instantiate(enemyPrefabs[TipoInimigo.TI.tipoInimigo1], EnemyBattleStation1.transform);
        enemyUnit1 = enemyGo1.GetComponent<Unit>();
        enemyAnimAttack1 = enemyGo1.GetComponent<Animator>();
        GameObject enemyGo2 = Instantiate(enemyPrefabs[TipoInimigo.TI.tipoInimigo2], EnemyBattleStation2.transform);
        enemyUnit2 = enemyGo2.GetComponent<Unit>();
        enemyAnimAttack2 = enemyGo2.GetComponent<Animator>();


        startText.text = "A Wild " + enemyUnit1.unitName +" and "+ enemyUnit2.unitName +" appeared";

        playerHUD.SetPlayerHUD(playerUnit);
        enemyHUD1.SetEnemyHUD(enemyUnit1);
        enemyHUD2.SetEnemyHUD(enemyUnit2);
        yield return new WaitForSeconds(2);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }
 void PlayerTurn()
    {playerUnit.shield = 0;
        startText.text = "Choose an action: ";
        attack1BT.SetActive(true);
        attack2BT.SetActive(true);
        shieldBT.SetActive(true);
    }
    IEnumerator PlayerAttack1()
    {   state = BattleState.PLAYERTURN;
        enemyUnit1.enemyCollider.SetActive(false);
        enemyUnit2.enemyCollider.SetActive(false);
        if (target == 1)
        {
                    enemyUnit1.TakeDamage(playerUnit.damageAttack1);
                    startText.text = playerUnit.unitName + " is Attacking";
                    playerAnimAttack.SetBool("Attacking", true);
                    attack1BT.SetActive(false);
                    attack2BT.SetActive(false);
                    shieldBT.SetActive(false);
                    yield return new WaitForSeconds(1f);

                    startText.text = "The attack is Successful";

                    yield return new WaitForSeconds(1);
                    playerAnimAttack.SetBool("Attacking", false);
                    enemyHUD1.SetHP(enemyUnit1.currentHP);

                    if (aliveEnemies<=0)
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
        else if (target == 2)
        {
            enemyUnit2.TakeDamage(playerUnit.damageAttack1);
            startText.text = playerUnit.unitName + " is Attacking";
            playerAnimAttack.SetBool("Attacking", true);
            attack1BT.SetActive(false);
            attack2BT.SetActive(false);
            shieldBT.SetActive(false);
            yield return new WaitForSeconds(1f);

            startText.text = "The attack is Successful";

            yield return new WaitForSeconds(1);
            playerAnimAttack.SetBool("Attacking", false);
            enemyHUD2.SetHP(enemyUnit2.currentHP);

            if (aliveEnemies<=0)
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

        target = 0;


    }

    IEnumerator PlayerAttack2()
    {
        state = BattleState.PLAYERTURN;
        enemyUnit1.TakeDamage(playerUnit.damageAttack2);
        enemyUnit2 .TakeDamage(playerUnit.damageAttack2);
        startText.text = playerUnit.unitName + " is Attacking " + enemyUnit1.unitName + " and " + enemyUnit2.unitName;
        playerAnimAttack.SetBool("Attacking", true);
        attack1BT.SetActive(false);
        attack2BT.SetActive(false);
        shieldBT.SetActive(false);
        yield return new WaitForSeconds(1f);

        startText.text = "The attack is Successful";

        yield return new WaitForSeconds(1);
        playerAnimAttack.SetBool("Attacking", false);
        enemyHUD1.SetHP(enemyUnit1.currentHP);
        enemyHUD2.SetHP(enemyUnit2.currentHP);

        if (aliveEnemies<=0)
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

    IEnumerator SelectTarget() //Chamar no BT attack1
    {
        attack1BT.SetActive(false);
        attack2BT.SetActive(false);
        shieldBT.SetActive(false);
        startText.text = "Choose a target";
        yield return new WaitForSeconds(1f);
        enemyUnit1.enemyCollider.SetActive(true);
        enemyUnit2.enemyCollider.SetActive(true);

        if (target ==0)
        {
            
            StartCoroutine(SelectTarget());
        }
        else if(target ==1||target ==2)
        {
            
            StartCoroutine(PlayerAttack1());
        }
    }

    IEnumerator EnemyTurn()
    { //turno inimigo 1
        if (enemyUnit1.isAlive == true)
        {
            startText.text = enemyUnit1.unitName + " attacks";
                    enemyAnimAttack1.SetBool("Attacking", true);
                    yield return new WaitForSeconds(1f);
                     playerUnit.TakeDamage(enemyUnit1.damage);
            
            
            
                    yield return new WaitForSeconds(1f);
                    playerHUD.SetHP(playerUnit.currentHP);
                    enemyAnimAttack1.SetBool("Attacking", false);
                    if (playerUnit.currentHP<=0)
                    {
                        state = BattleState.LOST;
                        EndBattle();
                    }
                   
        }

        yield return new WaitForSeconds(.5f);
        
        // turno inimigo 2
        if (enemyUnit2.isAlive == true)
        {
            startText.text = enemyUnit2.unitName + " attacks";
                    enemyAnimAttack2.SetBool("Attacking", true);
                    yield return new WaitForSeconds(1f);
                     playerUnit.TakeDamage(enemyUnit2.damage);
            
            
            
                    yield return new WaitForSeconds(1f);
                    playerHUD.SetHP(playerUnit.currentHP);
                    enemyAnimAttack2.SetBool("Attacking", false);
                    if (playerUnit.currentHP<=0)
                    {
                        state = BattleState.LOST;
                        EndBattle();
                    }
                   
        }
        state = BattleState.PLAYERTURN;
        PlayerTurn();
        
    }

    void EndBattle()
    {
        attack1BT.SetActive(false);
        attack2BT.SetActive(false);
        shieldBT.SetActive(false);
        if (state == BattleState.WON)
        {
            startText.text = "You won the Battle";
            playerAnimAttack.SetTrigger("BackFlip");
        }
        else if (state == BattleState.LOST)
        {
            startText.text = "You lost the Battle";
            enemyAnimAttack1.SetTrigger("EBackFlip");
            enemyAnimAttack2.SetTrigger("EBackFlip");
        }
    }

   

    public void Attack1Button()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;

        }

        state = BattleState.SELECTATTACK;
        StartCoroutine(SelectTarget());

    }

    public void Attack2Button()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;

        }
        StartCoroutine(PlayerAttack2());
    }
    

    public void Shield()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;

            
        }
        attack1BT.SetActive(false);
        attack2BT.SetActive(false);
        shieldBT.SetActive(false);
        playerUnit.shield = 0.25f;
        StartCoroutine(EnemyTurn());

    }
}
