using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer sp;
    public GameObject player;
    public Animator animator;

    //Alert
    public bool isAlert = false;
    public float alertRange;

    public bool triggerAttack = false;

    //Run
    public float movementDir;
    public float movementAcceleration;   //40
    public float maxMovemetnSepeed;      //2.5
    public float movementDecceleration;  //10
    public float stopMovementAt;   //2.5
    public bool changingDirection => (rb.velocity.x > 0f && movementDir < 0f) || (rb.velocity.x < 0f && movementDir > 0f);

    public bool shouldMoveAway = false;

    //IsAbble
    bool isAbleToDash = false;
    bool isAbbleToRun = false;

    public bool facingLeft = false;

    bool movementEnabled = true;

    void FixedUpdate()
    {
        if(movementEnabled)
            checkForAlert();
        CheckIfShouldRun();
        CheckForMovementDir();
        FlippSpriteWhenChangingDir();
        FlippSpriteBasedOnPlayerPossiton();
        ApplyLinearDrag();
        counterPushing();
        MoveAwayFromPlayer();
    }

    private void checkForAlert()
    {
        if (shouldMoveAway)
            return;

        if (transform.position.x - player.transform.position.x <= alertRange)
        {
            isAlert = true;
            isAbbleToRun = true;
        }
        else
        {
            isAbbleToRun = false;
            isAlert = false;
        }
    }
    private void CheckForMovementDir()
    {
        if (shouldMoveAway)
            return;

        if (player.transform.position.x + stopMovementAt < transform.position.x)
        {
            movementDir = -1f;
            triggerAttack = false;
        }
        else if (player.transform.position.x - stopMovementAt > transform.position.x)
        {
            movementDir = 1f;
            triggerAttack = false;
        }
        else
        {
            movementDir = 0f;
            if (!triggerAttack)
            {
                triggerAttack = true;
                AttackAfterMovement();
            }
        }
    }
    protected virtual void AttackAfterMovement()
    {
        Debug.Log("Needs to be implemented");
    }

    private void FlippSpriteWhenChangingDir()
    {
        switch (movementDir * ((facingLeft)?-1:1))
        {
            case 1f:
                sp.flipX = false;
                break;
            case -1f:
                sp.flipX = true;
                break;
        }
    }
    private void FlippSpriteBasedOnPlayerPossiton()
    {
        if((player.transform.position.x - transform.position.x) * ((facingLeft)?-1:1) > 0)
        {
            sp.flipX = false;
        }
        else
        {
            sp.flipX = true;
        }
    }

    private void CheckIfShouldRun()
    {
        if (shouldMoveAway)
            return;

        if (isAlert && isAbbleToRun)
        {
            Run();
        }
        else
        {
            animator.SetFloat("Speed", movementDir);
        }
    }
    private void Run()
    {
        animator.SetFloat("Speed", Mathf.Abs(movementDir));
        if (rb.velocity.x < maxMovemetnSepeed && rb.velocity.x > -maxMovemetnSepeed)
        {
            float speedDif = movementAcceleration - rb.velocity.x;
            rb.AddForce(new Vector2(movementDir * speedDif, rb.position.y), ForceMode2D.Force);
        }
    }
    private void ApplyLinearDrag()
    {
        if (Mathf.Abs(movementDir) < 0.4f || changingDirection)
        {
            rb.drag = movementDecceleration;
        }
        else
        {
            rb.drag = 0f;
        }

    }

    private void counterPushing()
    {
        if (player.transform.position.x < transform.position.x)
        {
            if (transform.position.x - player.transform.position.x < 1)
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
        else
        {
            if (player.transform.position.x - transform.position.x < 1)
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }

    public void MoveAwayFromPlayer()
    {
        if (shouldMoveAway)
        {
            if(player.transform.position.x > transform.position.x)
            {
                movementDir = -1;
            }
            else
            {
                movementDir = 1;
                sp.flipX = true;
            }

            animator.SetFloat("Speed", Mathf.Abs(movementDir));
            if (rb.velocity.x < maxMovemetnSepeed && rb.velocity.x > -maxMovemetnSepeed)
            {
                float speedDif = movementAcceleration - rb.velocity.x;
                rb.AddForce(new Vector2(movementDir * speedDif, rb.position.y), ForceMode2D.Force);
            }
            var combat = GetComponent<BoDcombat>();

            combat.switchDirTime += Time.deltaTime;

            if(combat.switchDirTime >= 1.5)
            {
                shouldMoveAway = false;
                combat.dirSwitched = false;
                combat.shouldCastSpeel = true;
            }
        }
    }

    public void DisableMovemetn()
    {
        movementEnabled = false;
        isAbbleToRun = false;
    }
    public void EnableMovement()
    {
        movementEnabled = true;
        isAbbleToRun = true;
    }
}
