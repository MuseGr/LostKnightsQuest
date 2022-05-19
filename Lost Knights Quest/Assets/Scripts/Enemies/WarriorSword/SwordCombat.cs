using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCombat : MonoBehaviour , ICombat
{
    public Rigidbody2D rb;
    public SpriteRenderer sp;
    public Animator animator;
    public GameObject player;

    //Attack
    public Transform attackPointRight;
    public Transform attackPointLeft;
    public float attackRange = 1f;
    public LayerMask playerLayer;

    public int attackDamage = 20;

    public bool canAttack = false;
    float timeOfGettingInRange;
    public float timeBeforeFirstAttack = 1f;
    float timeOfLastAttack;
    float attackAnimationDuration;
    public float attackSpeed = 1.5f;
    float attackPushStrenght = 5;
    float attackKnockBack = 0.7f;

    void Start()
    {
        
    }
    private void Update()
    {
        CheckForAttack();
    }
    void CheckForAttack()
    {
        if (GetComponent<Movement>().isAlert && isInRange())
        {
            if(Time.time >= timeOfGettingInRange + timeBeforeFirstAttack && Time.time >= timeOfLastAttack + attackSpeed)
            {
                Attack();
            }
        }
        else
        {
            timeOfGettingInRange = Time.time;
        }
    }
    bool isInRange()
    {
        if (player.GetComponent<CombatScript>().enabled)
        {
            if (rb.transform.position.x > player.transform.position.x) //on desno
            {
                if (rb.transform.position.x - player.transform.position.x <= GetComponent<Movement>().stopMovementAt)
                {
                    return true;
                }
            }
            else if (rb.transform.position.x < player.transform.position.x) //on levo
            {
                if (player.transform.position.x - rb.transform.position.x <= GetComponent<Movement>().stopMovementAt)
                {
                    return true;
                }
            }
            return false;
        }
        return false;
    }
    void Attack()
    {
        //Stop me form moving
        rb.velocity = Vector3.zero;

        //Attack animation
        animator.SetTrigger("Attack");

        addAttackPush(attackPushStrenght);

        timeOfLastAttack = Time.time;
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
            Debug.Log("Enemy dmg setn: " + attackDamage);

            if (!enemy.isTrigger)
            {
                enemy.GetComponent<CombatScript>().TakeDamage(attackDamage);
                enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(attackKnockBack, 0), ForceMode2D.Impulse);
            }
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

    public void ResetAttackTimer()
    {
        timeOfLastAttack = Time.time - 0.5f;
    }

    public void DisableCombat()
    {
        this.enabled = false;
    }
}

//Ukoliko je alertovan ide ka igracu, kada se zaustavi pokreni timer i nako Xs pokreni napad u odredjenom attack speedu