using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearMovement : Movement
{
    public SpearCombat spCombat;

    protected override void AttackAfterMovement()
    {
        spCombat.TriggerAttackWithDash();
    }
}
