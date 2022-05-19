using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkCombat : MonoBehaviour, ICombat
{
    public Rigidbody2D rb;
    public SpriteRenderer sp;
    public Animator animator;
    public Transform attackPointLeft;
    public Transform attackPointRight;
    public LayerMask playerLayer;
    public LayerMask playerDodgeLayer;

    public bool isInRange = false;

    //Attack
    public int attackDamage;
    public float attackRange;
    public float attackKnockBack;

    //Combos
    bool inCombo = false;
    bool combo1 = false;
    bool combo2 = false;
    bool combo3 = false;
    int comboAction = 0;

    //Times
    float timeOfPreformingAttack = 0;
    float timeOfComboStart = -5;

    private void Update()
    {
        Combo1();
        Combo2();
        Combo3();
    }
    private void FixedUpdate()
    {
        CheckForAttack();

        //Prekidanje komboa ukoliko nije u range
        if (!isInRange)
        {
            combo1 = false;
            combo2 = false;
            combo3 = false;
            inCombo = false;
        }
    }

    public void CheckForAttack()
    {
        if (isInRange && !inCombo)
        {
            AttackCombinations();
        }
    }
    public void AttackCombinations()
    { 
        int randNum = Random.Range(0, 3); // 0 1 2
        
        if(Time.time >= timeOfPreformingAttack + 1.5)
        {
            Debug.Log("choosing combination");
            switch (randNum)
            {
                case 0:
                    Debug.Log("calling combo 1");
                    timeOfComboStart = Time.time;
                    combo1 = true;
                    break;
                case 1:
                    Debug.Log("calling combo 2");
                    timeOfComboStart = Time.time;
                    combo2 = true;
                    break;
                case 2:
                    Debug.Log("calling combo 3");
                    timeOfComboStart = Time.time;
                    combo3 = true;
                    break;
            }

            inCombo = true;
        }
    }
    
    public void Combo1()
    {
        if (combo1 == true && comboAction == 0 && Time.time >= timeOfPreformingAttack + 0.6)
        {
            Punch();
            comboAction = 1;
        }
        else if (combo1 == true && comboAction == 1 && Time.time >= timeOfPreformingAttack + 0.6)
        {
            Kick();
            comboAction = 2;
        }
        else if (combo1 == true && comboAction == 2 && Time.time >= timeOfPreformingAttack + 0.6)
        {
            LowKick();
            comboAction = 3;
        }
        else if (combo1 == true && comboAction == 3 && Time.time >= timeOfPreformingAttack + 0.6)
        {
            Debug.Log("Prekini Kombo");
            inCombo = false;
            combo1 = false;
            comboAction = 0;
        }
    }
    public void Combo2()
    {
        if (combo2 == true && comboAction == 0 && Time.time >= timeOfPreformingAttack + 0.6)
        {
            Punch();
            comboAction = 1;
        }
        else if (combo2 == true && comboAction == 1 && Time.time >= timeOfPreformingAttack + 0.6)
        {
            LowKick();
            comboAction = 2;
        }
        else if (combo2 == true && comboAction == 2 && Time.time >= timeOfPreformingAttack + 0.6)
        {
            Kick();
            comboAction = 3;
        }
        else if (combo2 == true && comboAction == 3 && Time.time >= timeOfPreformingAttack + 0.6)
        {
            Debug.Log("Prekini Kombo");
            inCombo = false;
            combo2 = false;
            comboAction = 0;
        }
    }
    public void Combo3()
    {
        if (combo3 == true && comboAction == 0 && Time.time >= timeOfPreformingAttack + 0.6)
        {
            LowKick();
            comboAction = 1;
        }
        else if (combo3 == true && comboAction == 1 && Time.time >= timeOfPreformingAttack + 0.6)
        {
            Punch();
            comboAction = 2;
        }
        else if (combo3 == true && comboAction == 2 && Time.time >= timeOfPreformingAttack + 0.6)
        {
            Kick();
            comboAction = 3;
        }
        else if (combo3 == true && comboAction == 3 && Time.time >= timeOfPreformingAttack + 0.6)
        {
            Debug.Log("Prekini Kombo");
            inCombo = false;
            combo3 = false;
            comboAction = 0;
        }
    }

    public void Punch()
    {
        timeOfPreformingAttack = Time.time;
        Debug.Log("punch");
        animator.SetTrigger("Punch");
    }
    public void Kick()
    {
        timeOfPreformingAttack = Time.time;
        Debug.Log("Kick");
        animator.SetTrigger("Kick");
    }
    public void LowKick()
    {
        timeOfPreformingAttack = Time.time;
        Debug.Log("LowKick");
        animator.SetTrigger("LowKick");
    }

    public void detectEnemiesAndApplyDamage(int dmgMultiplyer)
    {
        Collider2D[] hitEnemies = { };

        if (sp.flipX)
        {
            
            hitEnemies = Physics2D.OverlapCircleAll(attackPointLeft.position, attackRange, playerDodgeLayer);
            
            hitEnemies = Physics2D.OverlapCircleAll(attackPointLeft.position, attackRange, playerLayer);
  
            attackKnockBack = -attackKnockBack;
        }
        else
        {
            
            hitEnemies = Physics2D.OverlapCircleAll(attackPointLeft.position, attackRange, playerDodgeLayer);

            hitEnemies = Physics2D.OverlapCircleAll(attackPointLeft.position, attackRange, playerLayer);

            attackKnockBack = Mathf.Abs(attackKnockBack);
        }

        foreach (Collider2D enemy in hitEnemies)
        {
            if (!enemy.isTrigger)
            {
                enemy.GetComponent<CombatScript>().TakeDamage(attackDamage + dmgMultiplyer);
                enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(attackKnockBack, 0), ForceMode2D.Impulse);
            }
        }
    }

    public void ResetAttackTimer()
    {
        if (inCombo)
        {
            inCombo = false;
            timeOfPreformingAttack = Time.time + 0.5f;
        }
    }

    public void DisableCombat()
    {
        this.enabled = false;
    }
}
