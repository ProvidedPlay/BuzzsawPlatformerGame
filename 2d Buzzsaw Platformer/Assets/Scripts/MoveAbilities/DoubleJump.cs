using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MoveAbility
{
    public float doubleJumpSpeedThreshold;
    public override void Awake()
    {
        base.Awake();
        abilityName = "Double Jump";
        movementIsEnabled = false;
    }

    public override void Update()
    {
        if (!canAct)
        {
            if (counter == 0 && Time.time > actTimer)
            {
                canAct = true;
            }
            //if(counter >0 && counter < maxCounter && player.rb.velocity.y < doubleJumpSpeedThreshold*-1)
            if(counter >0 && counter < maxCounter)
            {
                canAct = true;
            }
        }
    }
    public override void DoAction(bool input)
    {
        if (input)
        {
            if (!canAct)
            {
                if (!player.delayedJumpTimerActive)
                {
                    player.ActivateDelayedJumpTimer();
                }
            }
            else if (canAct)
            {
                InterruptAbilities();
                InterruptSwitches();

                PlaySound(counter < 1 ? "Jump Sound" : "Double Jump Sound");
                player.SetPlayerAnimationJump();

                float jumpStrength = player.jumpStrength;
                if (counter > 0)
                {
                    jumpStrength = player.multiJumpStrength;
                    //animation code goes here
                }

                //player.rb.velocity = Vector2.up * jumpStrength;
                player.rb.velocity = Vector2.up * jumpStrength * Mathf.Sign(player.previousGravityScale);

                counter += 1;
                actTimer = Time.time + actDelay;
                canAct = false;
            }
        }
    }
    public override void ResetAction()
    {

//        if (player.isGrounded)
//        {
            base.ResetAction();
            if (!canAct)
            {
                canAct = true;
            }
//        }
    }
    public override void EnableMovement()
    {
        base.EnableMovement();
        if (player.GetComponent<Jump>().movementIsEnabled)
        {
            counter = player.GetComponent<Jump>().counter;
            player.GetComponent<Jump>().movementIsEnabled = false;
        }
    }
}
