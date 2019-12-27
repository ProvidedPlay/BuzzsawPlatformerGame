using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseGravity : Switch
{

    public float snapMotionSpeed;
    public float stopMotionLength;
    public float deactivationCheckPeriod;

    Vector2 previousVelocity;
    float stopMotionTimer;
    public override void Awake()
    {
        base.Awake();
        switchName = "Refresh Ability";
    }
    public override void DoAction()
    {
        if (deactivationCheckTimer == 0)
        {
            PlaySound("Switch On");
            switchActive = true;
            player.SetLinkedSwitchesActive(switchName, true);
            ReversePreviousGravity();
            SnapMotion();
            player.FlipPlayer();
        }
        
    }
    public void FixedUpdate()
    {
        //deactivationCheckPeriod = Time.fixedDeltaTime;
        if (stoppingMotion)
        {
            if (Time.time < stopMotionTimer)
            {
                FreezePlayer();
            }
            if (Time.time >= stopMotionTimer && stoppingMotion == true)
            {
                CancelAction(false);
            }
        }
    }
    public void Update()
    {
        if (switchActive && Time.time > deactivationCheckTimer && deactivationCheckTimer !=0)
        {
            LinkedDeactivationCheck();
        }
    }
    void ReversePreviousGravity()
    {
        player.previousGravityScale = -player.previousGravityScale;
        player.rb.gravityScale = -player.rb.gravityScale;
        player.gravityReversed = !player.gravityReversed;
    }
    void SnapMotion()
    {
        InterruptAbilities();

        player.canWalk = false;
        previousVelocity = player.rb.velocity;
        if (player.rb.gravityScale != 0)
        {
            player.previousGravityScale = player.rb.gravityScale;
        }
        player.rb.gravityScale = 0;
        player.rb.velocity = Vector2.zero;

        stopMotionTimer = Time.time + stopMotionLength;
        stoppingMotion = true;

    }
    void FreezePlayer()
    {
        Vector2 targetTransform = transform.position;
        if (player.rb.position != targetTransform)
        {
            float step = snapMotionSpeed * Time.fixedDeltaTime;
            player.rb.MovePosition(Vector2.MoveTowards(player.rb.position, targetTransform, step));
        }
        if (player.rb.position == targetTransform)
        {
            player.rb.velocity = Vector2.zero;

        }
    }
    public override void CancelAction(bool interrupted)
    {
        player.canWalk = true;
        stoppingMotion = false;
        player.rb.gravityScale = player.previousGravityScale;
        player.rb.velocity = Vector2.zero;

    }
    public void LinkedDeactivationCheck()
    {
        if (!player.CheckLinkedSwitchesColliderOverlap(switchName))
        {
            player.SetLinkedSwitchesActive(switchName, false);
            deactivationCheckTimer = 0;
            //player.ResetLinkedTimers(switchName);
        }
        else
        {
            deactivationCheckTimer = Time.time + deactivationCheckPeriod;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LinkedDeactivationCheck();
        }
    }
}
