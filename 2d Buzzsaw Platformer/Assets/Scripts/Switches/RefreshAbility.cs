using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshAbility : Switch
{

    public float snapMotionSpeed;
    public float stopMotionLength;

    Vector2 previousVelocity;
    float stopMotionTimer;
    public override void Awake()
    {
        base.Awake();
        switchName = "Refresh Ability";
    }
    public override void DoAction()
    {
        PlaySound("Switch On");
        switchActive = true;
        UpdateSprite();
        SnapMotion();
        player.ResetMovements();
    }
    public void FixedUpdate()
    {
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
}
