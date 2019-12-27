using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TronTailSwitch : Switch
{

    public float deactivationCheckPeriod;
    public float switchPositionOffset;
    public float activationCheckPeriod;
    public string switchDirection;


    public override void Awake()
    {
        base.Awake();
        switchName = "Tron Tail Switch";
    }
    public override void DoAction()
    {
        if (deactivationCheckTimer == 0)
        {
            PlaySound("Switch On");
            switchActive = true;
            player.SetLinkedSwitchesActive(switchName, true);
            if (!player.tronTailActive)
            {
                tronTail.InitiateTronTail(player.transform.position);
            }
        }

    }
    public void FixedUpdate()
    {
        //deactivationCheckPeriod = Time.fixedDeltaTime;
        if (!switchActive && player.inTronZone)
        {
            LinkedActivationCheck();
        }
    }
    public void Update()
    {
        if (switchActive && Time.time > deactivationCheckTimer && deactivationCheckTimer != 0)
        {
            LinkedDeactivationCheck();
        }
        
    }
    public void LinkedDeactivationCheck()
    {
        if (!player.CheckLinkedSwitchesColliderOverlap(switchName))
        {
            if (!CheckPlayerInActiveDirection())
            {
                player.SetLinkedSwitchesActive(switchName, false);
                player.inTronZone = false;
                tronTail.EndTronTail();
            }
            deactivationCheckTimer = 0;
        }
        else
        {
            deactivationCheckTimer = Time.time + deactivationCheckPeriod;
        }
    }
    public void LinkedActivationCheck()
    {
        if (!player.CheckLinkedSwitchesColliderOverlap(switchName))
        {
            ActivateSwitch();
            deactivationCheckTimer = 0;
        }
    }
    public bool CheckPlayerInActiveDirection()
    {
        bool playerInActiveDirection = false;
        switch (switchDirection)
        {
            case "up":
                if (player.rb.position.y > (transform.position.y + switchPositionOffset))
                {
                    playerInActiveDirection = true;
                }
                break;
            case "down":
                if (player.rb.position.y < (transform.position.y-switchPositionOffset))
                {
                    playerInActiveDirection = true;
                }
                break;
            case "left":
                if (player.rb.position.x < (transform.position.x - switchPositionOffset))
                {
                    playerInActiveDirection = true;
                }
                break;
            case "right":
                if (player.rb.position.x > (transform.position.x + switchPositionOffset))
                {
                    playerInActiveDirection = true;
                }
                break;
            default:
                Debug.Log("Switch direction incorrectly specified");
                playerInActiveDirection = false;
                break;
        }
        return playerInActiveDirection;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!CheckPlayerInActiveDirection())
            {
                LinkedDeactivationCheck();
            }
            //else if (CheckPlayerInActiveDirection()) // Legacy
            else if(player.inTronZone)
            {
                LinkedActivationCheck();
            }
        }
    }
}
