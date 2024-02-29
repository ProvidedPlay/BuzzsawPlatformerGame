using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerColour : MonoBehaviour
{
    public Vector4 playerDefaultColourVector;
    public Vector4 playerDarkenedColourVector;
    [HideInInspector] public bool canDarkenPlayer;
    [HideInInspector] public bool playerDarkened;

    Player player;
    GameController controller;
    SpriteRenderer sp;
    Color playerDefaultColour;
    Color playerDarkenedColour;

    private void Awake()
    {
        player = GetComponent<Player>();
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>(); ;
        sp = player.spriteHandler.GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        if (controller != null)
        {
            if (player.globalLightPresent)
            {
                canDarkenPlayer = true;
                playerDefaultColour = playerDefaultColourVector;
                playerDarkenedColour = playerDarkenedColourVector;
            }
        }
    }
    private void FixedUpdate()
    {
        if (canDarkenPlayer)
        {
            UpdateTronTailColour();
        }
    }
    void UpdateTronTailColour()
    {
        if (!player.globalLightOn && !playerDarkened)
        {
            playerDarkened = true;
            sp.color = playerDarkenedColour;
            
        }
        if (player.globalLightOn && playerDarkened)
        {
            playerDarkened = false;
            sp.color = playerDefaultColour;
        }
    }
}
