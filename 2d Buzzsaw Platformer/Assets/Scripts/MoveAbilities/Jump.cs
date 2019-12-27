using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MoveAbility
{
    public override void Awake()
    {
        base.Awake();
        abilityName = "Jump";
        movementIsEnabled = true;
    }

    public override void DoAction(bool input)
    {
        if (input)
        {
            if (canAct)
            {
                InterruptAbilities();
                InterruptSwitches();
                player.PlaySound("Jump Sound");
                //player.rb.velocity = Vector2.up * player.jumpStrength;
                player.rb.velocity = Vector2.up * player.jumpStrength * Mathf.Sign(player.previousGravityScale);

                counter += 1;
                actTimer = Time.time + actDelay;
                canAct = false;
            }
            else if (!canAct)
            {
                if (!player.delayedJumpTimerActive)
                {
                    player.ActivateDelayedJumpTimer();
                }
            }
        }
    }
    public override void ResetAction()
    {

 //       if (player.isGrounded)
 //       {
            base.ResetAction();
            if (!canAct)
            {
                canAct = true;
            }
 //       }
    }
}
