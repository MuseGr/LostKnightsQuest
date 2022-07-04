using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizzCombat : MonoBehaviour
{
    public GameObject fireBallPrefab;
    public Transform fireBallSpawn;
    public Animator animator;

    float timeOfLastFireBall;
    public float fireRate = 1.75f;

    private void Update()
    {
        float playerX = GameObject.Find("Player").transform.position.x;
        if (Mathf.Abs(playerX - transform.position.x) <= 10)
        {
            if (Time.time >= timeOfLastFireBall + fireRate)
            {
                animator.SetTrigger("Fire");
                timeOfLastFireBall = Time.time;
            }
        }
    }

    public void FireBall()
    {
        Instantiate(fireBallPrefab, fireBallSpawn.position, Quaternion.identity);
    }


    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
