using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sopell : MonoBehaviour
{
    GameObject target;
    public int dmg = 45;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            Debug.Log(collision.name);
            target = collision.gameObject;
        }
    }

    public void DealDamage()
    {
        Debug.Log(target.name);
        target.GetComponent<CombatScript>().TakeDamage(dmg);
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
