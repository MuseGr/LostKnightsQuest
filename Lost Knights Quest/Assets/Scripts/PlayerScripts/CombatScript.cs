using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatScript : MonoBehaviour
{
    public SpriteRenderer sp;
    public Rigidbody2D rb;
    public Animator animator;
    public HealthBar healthBar;

    //Health
    public int maxHealth = 100;
    public int currentHealth;

    //Attack
    public Transform attackPointRight;
    public Transform attackPointLeft;
    public float attackRange = 1f;
    public LayerMask enemyLayers;

    public int attackDamage = 30;

    public bool canAttack = true;
    int currentAttack = 0;
    public float timeOfLastAttack;
    float currentAttackAnimationDuration;
    public float timeOfAttackCycleReset = 1.5f;
    public float attackPushStrenght = 5;
    public float attackKnockBack;

    //Block
    public bool canBlock = true;
    public bool isBlocking = false;
    float timeOfBlock;
    public float blockDuration;
    public float blockCooldown;
    float timeOfLastBlock;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        checkForAttack();
        CheckForBlock();
    }
    
    void checkForAttack()
    {
        resetAttackCycle();
        if(Input.GetMouseButtonDown(0) && Time.time >= timeOfLastAttack + currentAttackAnimationDuration && canAttack)
        {
            Attack();
        }
    }
    void Attack()
    {
        //Stio player form moving
        rb.velocity = Vector3.zero;

        //Play attack animation
        switch (currentAttack)
        {
            case 0:
                animator.SetTrigger("Attack1");
                addAttackPush(attackPushStrenght);
                timeOfLastAttack = Time.time;
                currentAttackAnimationDuration = 0.5f;
                currentAttack = 1;
                break;
            case 1:
                animator.SetTrigger("Attack2");
                addAttackPush(attackPushStrenght);
                timeOfLastAttack = Time.time;
                currentAttackAnimationDuration = 0.5f;
                currentAttack = 2;
                break;
            case 2:
                animator.SetTrigger("Attack3");
                addAttackPush(attackPushStrenght + (attackPushStrenght / 3));
                timeOfLastAttack = Time.time;
                currentAttackAnimationDuration = 1f;
                currentAttack = 0;
                break;
        }
    }
    public void Attack01()
    {
        detectEnemiesAndApplyDamage();
    }
    public void Attack02()
    {
        detectEnemiesAndApplyDamage();
    }
    public void Attack03()
    {
        detectEnemiesAndApplyDamage();
    }
    void resetAttackCycle()
    {
        if(Time.time >= timeOfLastAttack + timeOfAttackCycleReset)
        {
            currentAttack = 0;
        }
    }
    void addAttackPush(float aps)
    {
        if (sp.flipX)
        {
            rb.AddForce(new Vector2(-aps, 0), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(new Vector2(aps, 0), ForceMode2D.Impulse);
        }
        
    }
    void detectEnemiesAndApplyDamage()
    {
        Collider2D[] hitEnemies = {};
        if (sp.flipX)
        {
            hitEnemies = Physics2D.OverlapCircleAll(attackPointLeft.position, attackRange, enemyLayers);
            attackKnockBack = -attackKnockBack;
        }
        else
        {
            hitEnemies  = Physics2D.OverlapCircleAll(attackPointRight.position, attackRange, enemyLayers);
            attackKnockBack = Mathf.Abs(attackKnockBack);
        }

        foreach(Collider2D enemy in hitEnemies)
        {
            if(currentAttack == 0)
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage + (attackDamage/3));
                enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(attackKnockBack * Mathf.Abs(attackKnockBack), 0), ForceMode2D.Impulse);
            }
            else
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
                enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(attackKnockBack, 0), ForceMode2D.Impulse);
            }
        }
    }
    //private void OnDrawGizmosSelected()
    //{
    //    if (attackPoint != null)
    //    {
    //        Gizmos.DrawSphere(attackPoint.position, attackRange);
    //    }
    //    else
    //        return;
    //}
    public void ToggleCanAttack()
    {
        switch (canAttack)
        {
            case true:
                canAttack = false;
                break;
            case false:
                canAttack = true;
                break;
        }
    }

    void CheckForBlock()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (canBlock && Time.time >= timeOfLastBlock + blockCooldown)
            {
                timeOfBlock = Time.time;
                Block();
            }
        }
        if (Time.time > timeOfBlock + blockDuration && isBlocking)
        {
            isBlocking = false;
            canBlock = true;

            animator.SetBool("isBlocking", false);

            timeOfLastBlock = Time.time;

            canAttack = true;
        }
    }
    void Block()
    {
        isBlocking = true;
        canBlock = false;

        animator.SetBool("isBlocking", true);

        canAttack = false;
    }
    public void TriggerCanBlock()
    {
        switch (canBlock)
        {
            case true:
                canBlock = false;
                break;
            case false:
                canBlock = true;
                break;
        }
    }
   
    public void enabelAllAfterHit()
    {
        canAttack = true;
        canBlock = true;
        GetComponent<NewMovement>().ableToRun = true;
        GetComponent<NewMovement>().ableToJump = true;
        GetComponent<NewMovement>().ableToRoll = true;
        GetComponent<NewMovement>().canRoll = true;
    }

    public void UnblockableAttack(int dmg)
    {
        currentHealth -= dmg;
        animator.SetTrigger("Hurt");

        healthBar.SetHealth(currentHealth);
    }
    public void TakeDamage(int dmg)
    {
        if (!isBlocking)
        {
            currentHealth -= dmg;
            animator.SetTrigger("Hurt");

            healthBar.SetHealth(currentHealth);
        }
        else
        { 
            animator.SetTrigger("Block");

            GameObject.Find("Monk").GetComponent<MonkCombat>().ResetAttackTimer();
        }
       

        if (currentHealth <= 0)
        {
            Die();
        }
        
    }
    public void Heal()
    {
        currentHealth += (maxHealth - currentHealth)/2;

        healthBar.SetHealth(currentHealth);
    }
    public void Die()
    {
        animator.SetBool("isDead", true);

        DeathScreen.instance.Show();

        rb.velocity = Vector2.zero;
        GetComponent<NewMovement>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
