using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoDcombat : MonoBehaviour, ICombat
{
    public Rigidbody2D rb;
    public SpriteRenderer sp;
    public Animator animator;
    public GameObject player;

    public bool inRange = false;
    float baseAttackRange;
    float currentAttackRange;

    //Stage
    public int stage = 1;

    //Attack
    public Transform attackPointLeft;
    public Transform attackPointRight;
    public LayerMask playerLayer;

    public int attackDamage = 25;
    public float attackSpeed = 1.75f;
    float timeOfLastAttack;
    public float attackKnockBack = 1f;

    public int attackCount = 0;
    public bool dirSwitched = false;

    public float switchDirTime;

    //Spell
    // - za spel koristi movemnt enabled i disabled skripte iz movemnta
    public GameObject spellPrefab;
    public bool shouldCastSpeel = false;


    //DownTime
    float downTimeStart;
    public float downTimeDuration = 2;
    bool downtimeStarted = false;

    private void Start()
    {
        baseAttackRange = gameObject.GetComponent<BoDMovement>().stopMovementAt;
        currentAttackRange = baseAttackRange;
    }
    private void Update()
    {
        PreformAttack();
        CheckForStage2();
        //SwitchDirection();
        CheckSpellCast();
        CheckDowntime();
    }

    private void FixedUpdate()
    {
        CheckIfOurOfRange();
    }

    void CheckIfOurOfRange()
    {
        if(transform.position.x + currentAttackRange < player.transform.position.x)
        {
            inRange = false;

        }else if(transform.position.x - currentAttackRange > player.transform.position.x)
        {
            inRange = false;
        }
    }

    void PreformAttack()
    {
        if (inRange && Time.time >= timeOfLastAttack + attackSpeed)
        {
            if(stage == 1)
                animator.SetTrigger("NonEffectAttack");
            if (stage == 2)
            {
                if(attackCount < 2)
                {
                    animator.SetTrigger("EffectAttack");

                    attackCount++;
                }
            }

            timeOfLastAttack = Time.time;
        }
    }
    public void DettectPlayerAndDealDamage()
    {
        if(stage == 2)
            attackDamage += 15;

        Collider2D[] hitEnemies = { };
        if (!sp.flipX)
        {
            hitEnemies = Physics2D.OverlapCircleAll(attackPointLeft.position, currentAttackRange + 0.5f, playerLayer);
            attackKnockBack = -attackKnockBack;
        }
        else
        {
            hitEnemies = Physics2D.OverlapCircleAll(attackPointRight.position, currentAttackRange + 0.5f, playerLayer);
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
    }

    public void SwitchDirection()
    {
        if(attackCount == 2)
        {
            GetComponent<BoDMovement>().shouldMoveAway = true;
            dirSwitched = true;
            switchDirTime = 0;

            downtimeStarted = false;
        }
    }

    void CheckSpellCast()
    {
        if (shouldCastSpeel)
        {
            animator.SetTrigger("Spell");
            shouldCastSpeel = false;
            GetComponent<BoDMovement>().DisableMovemetn();
        }
    }

    public void CastSpell()
    {
        float dir = GetComponent<BoDMovement>().movementDir;
        Vector2 spawnPosition = new Vector2(player.transform.position.x + 1.5f * -dir, player.transform.position.y + 1.5f);

        Instantiate(spellPrefab, spawnPosition, Quaternion.identity);
    }

    public void StartDowntime()
    {
        downTimeStart = Time.time;
        downtimeStarted = true;
        //Debug.Log("Start");
    }
    void CheckDowntime()
    {
        if(Time.time >= downTimeStart + downTimeDuration && downtimeStarted)
        {
            GetComponent<BoDMovement>().EnableMovement();
        }
    }
    public void CheckForStage2()
    {
        var currentHealth = gameObject.GetComponent<EnemyHealth>().currentHealth;
        var maxHealth = gameObject.GetComponent<EnemyHealth>().maxHealth;

        if (currentHealth <= maxHealth / 2)
        {
            stage = 2;
            attackSpeed = 1f;
            currentAttackRange = baseAttackRange + 1;

            gameObject.GetComponent<BoDMovement>().maxMovemetnSepeed = 2;
            gameObject.GetComponent<BoDMovement>().movementAcceleration = 100;

            animator.SetTrigger("Stage2");
        }
    }


    public void DisableCombat()
    {
        this.enabled = false;
    }

    public void ResetAttackTimer()
    {
        //throw new System.NotImplementedException();
    }
}
