using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTronTailMaterial : MonoBehaviour
{

    [HideInInspector]public bool canDarkenTronTail;
    [HideInInspector]public bool tronTailDarkened;

    Player player;
    GameController gameController;
    LineRenderer tronTail;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player") != null? GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(): null;
        gameController = GameObject.FindGameObjectWithTag("GameController") != null ? GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>() : null;
        tronTail = GetComponent<LineRenderer>();
    }
    private void Start()
    {
        if (player != null && gameController != null)
        {
            if (player.globalLightPresent)
            {
                canDarkenTronTail = true;
            }
        }
    }
    private void FixedUpdate()
    {
        if (canDarkenTronTail)
        {
            UpdateTronTailColour();
        }
    }
    void UpdateTronTailColour()
    {
        if (!player.globalLightOn && !tronTailDarkened)
        {
            tronTailDarkened = true;
            tronTail.startColor = Color.black;
            tronTail.endColor = Color.black;
        }
        if (player.globalLightOn && tronTailDarkened)
        {
            tronTailDarkened = false;
            tronTail.startColor = Color.red;
            tronTail.endColor = Color.red;
        }
    }
}
