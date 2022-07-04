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
    public int currentHealth;

    //HealthBar
    public bool isHit;
    public int hitDamage;
    public bool isDead = false;

    public bool isBoss = false;

    public bool shouldSelfDestroy;

    private ICombat combat;

    void Start()
    {
        currentHealth = maxHealth;
        combat = GetComponent<ICombat>();
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

        if(combat != null)
            combat.ResetAttackTimer();

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
        if(shouldSelfDestroy)
            Destroy(gameObject);

        isDead = true;

        animator.SetBool("IsDead", true);

        if(gameObject.name == "Monk")
        {
            GameObject.Find("Portal").GetComponent<Portal>().canWork = true;
        }

        rb.velocity = Vector2.zero;
        
        if(movement != null)
            movement.enabled = false;

        if (combat != null)
            combat.ResetAttackTimer();

        rb.gravityScale = 0;
        rb.simulated = false;
        col.enabled = false;
    }

    public float GetHealthPer()
    {
        return (float) currentHealth / maxHealth;
    }
    public void DestroObject()
    {
        Destroy(gameObject);
    }
}
