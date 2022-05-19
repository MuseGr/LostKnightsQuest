using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePlace : MonoBehaviour
{
    public Animator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !collision.isTrigger)
        {
            if(collision.GetComponent<CombatScript>().currentHealth < collision.GetComponent<CombatScript>().maxHealth
               || collision.GetComponent<NewMovement>().rollStamina < collision.GetComponent<NewMovement>().maxRollStamina)
            {
                animator.SetTrigger("TurnOf");
                collision.GetComponent<CombatScript>().Heal();
                collision.GetComponent<NewMovement>().RestoreStamina();
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
}
