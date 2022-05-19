using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHealthBar
{
    public Rigidbody2D rb;
    public Animator animator;
    public Movement movement;
    public Collider2D col;

    //Health
    public int maxHealth = 100;
    int currentHealth;

    //HealthBar
    public bool isHit;
    public int hitDamage;
    public bool isDead = false;

    public bool isBoss = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isBoss)
        {
            if (Vector3.Distance(transform.position, NewMovement.Instance.transform.position) < 8)
            {
                BossHealthBar.Instance.SetCurrent(this);
            }
            else
            {
                if(BossHealthBar.Instance.currentBoss == this)
                {
                    BossHealthBar.Instance.SetCurrent(null);
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");

        GetComponent<ICombat>().ResetAttackTimer();

        //HealthBar, if enemy has one
        isHit = true;
        hitDamage = damage;


        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        isDead = true;

        animator.SetBool("IsDead", true);

        rb.velocity = Vector2.zero;
        movement.enabled = false;
        GetComponent<ICombat>().DisableCombat();
        rb.gravityScale = 0;
        col.enabled = false;
    }

    public float GetHealthPer()
    {
        return (float) currentHealth / maxHealth;
    }
}
