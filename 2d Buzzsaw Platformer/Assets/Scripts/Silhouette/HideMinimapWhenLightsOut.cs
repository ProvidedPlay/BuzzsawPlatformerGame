using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideMinimapWhenLightsOut : MonoBehaviour
{
    [HideInInspector]public bool canHideMinimap;
    [HideInInspector]public bool minimapHid;

    Player player;
    GameController gameController;
    RawImage minimap;

    private void Awake()
    {
        player = GetComponent<Player>();
        gameController = GameObject.FindGameObjectWithTag("GameController") != null ? GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>() : null;
        minimap = gameController != null ? gameController.miniMap : null;
    }
    private void Start()
    {
        if (player != null && gameController != null && minimap != null)
        {
            if (player.globalLightPresent)
            {
                canHideMinimap = true;
            }
            if (!player.globalLightPresent)
            {
                canHideMinimap = false;
                minimapHid = false;
                minimap.color = Color.white;
            }
        }
    }
    private void FixedUpdate()
    {
        if (canHideMinimap)
        {
            UpdateMinimapColourAlpha();
        }
    }
    void UpdateMinimapColourAlpha()
    {
        if (!player.globalLightOn && !minimapHid)
        {
            minimapHid = true;
            minimap.color = Color.black;
        }
        if (player.globalLightOn && minimapHid)
        {
            minimapHid = false;
            minimap.color = Color.white;
        }
    }

}
