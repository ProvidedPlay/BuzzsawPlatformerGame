using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashImproved : MoveAbility
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
        abilityName = "Dash Improved";
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
                float verticalDirection = Input.GetAxisRaw("Vertical");
                if (horizontalDirection!=0 || verticalDirection!=0)
                {
                    PlaySound("Dash");
                    player.dashEcho.ActivateDashEcho();
                    DashInDirection(horizontalDirection, verticalDirection);

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
    void DashInDirection(float horizontalDirection, float verticalDirection)
    {
        player.canWalk = false;
        dashTimer = Time.time + dashPeriod;

        moveActive = true;
        if (player.rb.gravityScale != 0)
        {
            player.previousGravityScale = player.rb.gravityScale;
        }

        player.rb.gravityScale = 0;
        player.rb.velocity = Mathf.Abs(horizontalDirection) >= Mathf.Abs(verticalDirection) ? (Vector2.right * dashSpeed * Mathf.Sign(horizontalDirection)) : (Vector2.up * dashSpeed * verticalDashMultiplier * Mathf.Sign(verticalDirection));

    }
    public override void CancelAction(bool movementInterrupted)
    {
        player.canWalk = true;
        moveActive = false;
        player.dashEcho.DeactivateDashEcho();

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
