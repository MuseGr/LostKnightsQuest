using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearCombat : MonoBehaviour , ICombat
{
    public Rigidbody2D rb;
    public SpriteRenderer sp;
    public Animator animator;
    public GameObject player;

    //Health
    public int maxHealth = 100;
    int currentHealth;

    //Dash
    public bool isDashing;
    public float dashSpeed;
    float timeOfDash;


    //Attack
    public Transform attackPointRight;
    public Transform attackPointLeft;
    public float attackRange = 1.57f;
    public LayerMask playerLayer;

    public bool canAttackNoDash = false;

    public int attackDamage = 30;
    public float attackSpeed = 1;

    public float attackPushStrenght = 5;
    public float attackKnockBack = 1;

    public float timeOfLastAttack;

    void Start()
    {
        currentHealth = maxHealth;
    }
    private void FixedUpdate()
    {
        StopDash();
        AttackIfInRange();
    }

    public void TriggerAttackWithDash()
    {
        Dash();
    }

    float CheckDashDir()
    {
        if (sp.flipX)
            return -1;

        return 1;
    }
    private void Dash()
    {
        isDashing = true;
        rb.AddForce(new Vector2(dashSpeed * CheckDashDir(), rb.velocity.y), ForceMode2D.Impulse);
        timeOfDash = Time.time;
    }
    private void StopDash()
    {
        if (isDashing && Time.time >= timeOfDash + 0.2)
        {
            rb.velocity = Vector2.zero;
            isDashing = false;

            Attack();
        }
    }

    public void AttackIfInRange()
    {
        if (GetComponent<SpearMovement>().triggerAttack && Time.time >= timeOfLastAttack + attackSpeed && canAttackNoDash)
        {
            Attack();
        }
    }
    public void Attack()
    {
        canAttackNoDash = false;
        Debug.Log("Attack");
        animator.SetTrigger("Attack");
        addAttackPush(attackPushStrenght);
    }
    void detectEnemiesAndApplyDamage()
    {
        Collider2D[] hitEnemies = { };
        if (sp.flipX)
        {
            hitEnemies = Physics2D.OverlapCircleAll(attackPointLeft.position, attackRange, playerLayer);
            attackKnockBack = -attackKnockBack;
        }
        else
        {
            hitEnemies = Physics2D.OverlapCircleAll(attackPointRight.position, attackRange, playerLayer);
            attackKnockBack = Mathf.Abs(attackKnockBack);
        }

        foreach (Collider2D enemy in hitEnemies)
        {
            if (!enemy.isTrigger)
            {
                enemy.GetComponent<CombatScript>().TakeDamage(attackDamage);
                enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(attackKnockBack, 0), ForceMode2D.Impulse);
            }
        }

        timeOfLastAttack = Time.time;
        canAttackNoDash = true;
    }
    //private void OnDrawGizmosSelected()
    //{
    //    if (attackPointLeft != null)
    //    {
    //        Gizmos.DrawSphere(attackPointLeft.position, attackRange);
    //    }
    //    else
    //        return;
    //}
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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");

        //timeOfLastAttack = Time.time - 0.5f;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        animator.SetBool("isDead", true);

        rb.velocity = Vector2.zero;
        GetComponent<Movement>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    public void ResetAttackTimer()
    {
        timeOfLastAttack = Time.time - 0.5f;
    }

    public void DisableCombat()
    {
        this.enabled = false;
    }
}