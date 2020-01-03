using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTronTailParticleColour : MonoBehaviour
{
    [HideInInspector] public bool canDarkenTronTailParticles;
    [HideInInspector] public bool tronTailParticlesDarkened;

    public Player player;
    GameController gameController;
    ParticleSystem.MainModule tronTailParticleColour;
    //ParticleSystem.ColorOverLifetimeModule tronTailParticleColourOverLifetime;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController") != null ? GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>() : null;
        tronTailParticleColour = GetComponent<ParticleSystem>().main;
        //tronTailParticleColourOverLifetime = GetComponent<ParticleSystem>().colorOverLifetime;
    }
    private void Start()
    {
        if (gameController != null)
        {
            if (player.globalLightPresent)
            {
                canDarkenTronTailParticles = true;
            }
        }
    }
    private void FixedUpdate()
    {
        if (canDarkenTronTailParticles)
        {
            UpdateTronTailColour();
        }
    }
    void UpdateTronTailColour()
    {
        if (!player.globalLightOn && !tronTailParticlesDarkened)
        {
            tronTailParticlesDarkened = true;
            tronTailParticleColour.startColor = new ParticleSystem.MinMaxGradient(Color.black);
            //tronTailParticleColourOverLifetime.color = Color.black;
        }
        if (player.globalLightOn && tronTailParticlesDarkened)
        {
            tronTailParticlesDarkened = false;
            tronTailParticleColour.startColor = new ParticleSystem.MinMaxGradient(Color.red);
            //tronTailParticleColourOverLifetime.color = Color.red;
        }
    }
}
