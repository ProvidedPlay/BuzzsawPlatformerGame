using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEcho : MonoBehaviour
{
    public Material litMaterial;
    public Material unLitMaterial;

    bool globalLightPresent;
    Player player;
    GameController controller;
    TrailRenderer trailRenderer;


    private void Awake()
    {
        player = gameObject.GetComponentInParent<Player>();
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        trailRenderer = GetComponent<TrailRenderer>();
    }
    private void Start()
    {
        DetectGlobalLight();
        SetUpDashWaveProperties();
    }
    void DetectGlobalLight()
    {
        if (controller != null)
        {
            if (player.globalLightPresent)
            {
                globalLightPresent = true;
            }
        }
    }
    void SetUpDashWaveProperties()
    {
        if (globalLightPresent)
        {
            trailRenderer.material = litMaterial;
            trailRenderer.sortingLayerName = "Silhouetted";
            return;
        }
        if (!globalLightPresent)
        {
            trailRenderer.material = unLitMaterial;
            trailRenderer.sortingLayerName = "Default";
            return;
        }
    }
    public void ActivateDashEcho()
    {
        if (!trailRenderer.emitting)
        {
            trailRenderer.emitting = true;
        }
    }
    public void DeactivateDashEcho()
    {
        if (trailRenderer.emitting)
        {
            trailRenderer.emitting = false;
        }
    }
}
