using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkMovement : Movement
{
    public MonkCombat combatScript;

    private void Update()
    {
        if (player.transform.position.x + stopMovementAt < transform.position.x)
        {
            combatScript.isInRange = false;

        }
        else if (player.transform.position.x - stopMovementAt > transform.position.x)
        {
            combatScript.isInRange = false;
        }
    }

    protected override void AttackAfterMovement()
    {
        combatScript.isInRange = true;
    }
}
