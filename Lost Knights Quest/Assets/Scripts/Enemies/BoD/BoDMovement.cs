using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoDMovement : Movement
{
    public BoDcombat combat;
    bool shouldSwitch = false;
    private void Start()
    {
        combat = GetComponent<BoDcombat>();
    }

    protected override void AttackAfterMovement()
    {
        combat.inRange = true;
    }
}
