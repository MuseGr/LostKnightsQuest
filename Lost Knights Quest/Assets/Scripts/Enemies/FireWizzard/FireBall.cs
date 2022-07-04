using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    float dir;
    public float speed = 0;
    public int dmg = 17;
    float spawnedAt;

    private void Start()
    {
        spawnedAt = Time.time;
    }

    private void Update()
    {
        transform.position+= new Vector3(-speed, 0, 0) * Time.deltaTime;
        if(Time.time >= spawnedAt + 2.5f)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<CombatScript>().UnblockableAttack(dmg);
            Destroy(gameObject);
        }
    }
}
