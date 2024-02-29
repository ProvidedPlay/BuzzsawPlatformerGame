using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJump : MonoBehaviour
{

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float longJumpThreshold = 0;
    public float feelMultiplier;
    public float rbVelocityY;

    Rigidbody2D rb;
    Player player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
    }
    private void Start()
    {
        rb.gravityScale = rb.gravityScale * feelMultiplier;
        player.jumpStrength = player.jumpStrength * feelMultiplier;
        player.multiJumpStrength = player.multiJumpStrength * feelMultiplier;
    }

    private void Update()
    {
        //rbVelocityY = rb.velocity.y;
        if (rb.gravityScale != 0)
        {
            rbVelocityY = rb.velocity.y * Mathf.Sign(player.previousGravityScale);
            if (Input.GetKey("g"))
            {
                Debug.Log(rbVelocityY);
            }

            if (rbVelocityY < 0 && !player.isGrounded)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime * Mathf.Sign(player.previousGravityScale);
            }
            else if (rbVelocityY > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime * Mathf.Sign(player.previousGravityScale);
            }
            else if (Input.GetButton("Jump") && rbVelocityY > 0 && rbVelocityY < longJumpThreshold)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime * Mathf.Sign(player.previousGravityScale);
            }
        }
        
    }
}
