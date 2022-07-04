using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMechanics : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer sp;
    GameObject player;
    public Animator animator;

    bool canMove = false;
    int moveDir;
    public float moveSpeed = 3;

    public string state = "notSpawned";

    public float explodeDistance = 1.25f;

    private void Start()
    {
        player = GameObject.Find("Player");
    }
    private void Update()
    {
        CheckExplode();

        switch (state)
        {
            case "spawned":
                canMove = true;
                break;
            case "dead":
                canMove = false;
                break;
        }
    }
    private void FixedUpdate()
    {
        MoveDir();
        Move(moveDir);
    }
    void MoveDir()
    {
        if (transform.position.x > player.transform.position.x)
        {
            moveDir = -1;
            sp.flipX = false;
        }
        else if (transform.position.x < player.transform.position.x)
        {
            moveDir = 1;
            sp.flipX = true;
        }
        else
            moveDir = 0;
    }
    void Move(int dir)
    {
        if (canMove)
        {
            rb.velocity = new Vector2(dir * moveSpeed, transform.position.y);
        }
    }
    void CheckExplode()
    {
        if(Vector2.Distance(player.transform.position, transform.position) <= explodeDistance && state != "dead")
        {
            state = "dead";
            animator.SetTrigger("Explode");
            player.GetComponent<CombatScript>().TakeDamage(10);
        }
    }
    //states
    public void Spawned()
    {
        state = "spawned";
    }

    public void Die()
    {
        animator.SetTrigger("Die");
        state = "dead";
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
    public void DisableColider()
    {
        rb.isKinematic = true;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        GetComponent<CapsuleCollider2D>().enabled = false;
    }
}
