using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class LightSwitch : Switch
{

    public float snapMotionSpeed;
    public Light2D switchLight;
    public Sprite inActiveSpriteLightOn;
    public Sprite activeSpriteLightOff;

    Vector2 previousVelocity;
    public override void Awake()
    {
        base.Awake();
        switchName = "Light Switch";
    }
    public override void DoAction()
    {
        PlaySound("Switch On");
        switchActive = true;
        UpdateGlobalLight();
        player.UpdateLinkedLightSwitches("Light Switch");
        SnapMotion();
        player.ResetMovements();
    }
    public void FixedUpdate()
    {
        if (stoppingMotion)
        {
            FreezePlayer();
        }
    }
    public override void UpdateSprite()
    {
        if (switchActive)
        {
            sp.sprite = player.globalLightOn ? activeSprite : activeSpriteLightOff;
        }
        if (!switchActive)
        {
            sp.sprite = player.globalLightOn ? inActiveSpriteLightOn : inActiveSprite;
        }
        
    }
    public override void ResetSwitch()
    {
        if (switchActive)
        {
            switchActive = false;
        }
        UpdateGlobalLight();
        player.UpdateLinkedLightSwitches("Light Switch");

        
    }
    public void LeaveSwitch()
    {
        player.ToggleLevelLight(false);
        player.UpdateLinkedLightSwitches("Light Switch");
    }
    public override void UpdateSwitchLight()
    {
        switchLight.enabled = player.globalLightOn ? false : !switchActive; 
    }
    void UpdateGlobalLight()
    {
        player.ToggleLevelLight(switchActive);
    }
    void UpdateGlobalLight(bool forceOn)
    {
        player.ToggleLevelLight(forceOn);
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
        LeaveSwitch();//Test this feature: has light turn off when player jumps away
    }
}
