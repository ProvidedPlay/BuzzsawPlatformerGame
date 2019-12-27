using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWall : MonoBehaviour
{
    public bool facingLeft;
    public bool facingRight;

    Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            if (facingLeft)
            {
                player.wallLeft = true;
            }
            if (facingRight)
            {
                player.wallRight = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (facingLeft && player.wallLeft)
        {
            player.wallLeft = false;
        }
        if (facingRight && player.wallRight)
        {
            player.wallRight = false;
        }
    }
}
