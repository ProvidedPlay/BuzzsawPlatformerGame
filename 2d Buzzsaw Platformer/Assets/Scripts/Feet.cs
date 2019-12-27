using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour
{
    public Player player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") && !player.isGrounded && !player.playerIsDead)
        {
                player.audioManager.PlaySoundEffect("Landing Sound");
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("MovingPlatform"))
        {
            player.isGrounded = true;
            player.ResetMovements();
            player.GroundResetSwitches();
            if (collision.CompareTag("MovingPlatform"))
            {
                player.isGroundedMovingPlatform = true;
                //player.transform.SetParent(collision.transform);
            }
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("MovingPlatform"))
        {
            player.isGrounded = false;
            if (collision.CompareTag("MovingPlatform"))
            {
                player.isGroundedMovingPlatform = false;
                //player.transform.SetParent(null);
            }
        }
    }
}
