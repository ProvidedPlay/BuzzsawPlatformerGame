using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MoveAbility
{

    public float dashSpeed;
    public float dashPeriod;
    public float verticalDashMultiplier;
    public float afterDashFreezePercentage;

    float dashTimer;
    float afterDashFreezeTime;
    public override void Awake()
    {
        base.Awake();
        abilityName = "Dash";
        movementIsEnabled = false;
    }
    private void Start()
    {
        SetUpDashFreeze();
        actDelay += dashPeriod;
    }
    void SetUpDashFreeze()
    {
        afterDashFreezeTime = dashPeriod * afterDashFreezePercentage;
        dashPeriod += afterDashFreezeTime;
    }
    public override void DoAction(bool input)
    {
        if (input)
        {
            if (canAct)
            {
                InterruptSwitches();
                float horizontalDirection = Input.GetAxisRaw("Horizontal");
                if (horizontalDirection!=0 )
                {
                    PlaySound("Dash");
                    DashInDirection(horizontalDirection);

                    counter += 1;
                    actTimer = Time.time + actDelay;
                    canAct = false;
                }
            }
        }
    }

    public override void Update()
    {
        base.Update();
        if (moveActive)
        {
            if (Time.time > dashTimer)
            {
                CancelAction(false);
            }
            if (Time.time > (dashTimer - afterDashFreezeTime) && Time.time < dashTimer)
            {
                player.rb.velocity = Vector2.zero;
            }
        }
    }
    void DashInDirection(float horizontalDirection)
    {
        player.canWalk = false;
        dashTimer = Time.time + dashPeriod;

        moveActive = true;
        if (player.rb.gravityScale != 0)
        {
            player.previousGravityScale = player.rb.gravityScale;
        }

        player.rb.gravityScale = 0;

        player.rb.velocity = Vector2.right * dashSpeed * Mathf.Sign(horizontalDirection);

    }
    public override void CancelAction(bool movementInterrupted)
    {
        player.canWalk = true;
        moveActive = false;

        player.rb.gravityScale = player.previousGravityScale;
        if (!movementInterrupted)
        {
            player.rb.velocity = Vector2.zero;
        }
        
    }
    public override void ResetAction()
    {
        base.ResetAction();
        //CancelAction(false);
    }

}
