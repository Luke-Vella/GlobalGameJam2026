using UnityEngine;
using System.Collections;

public class SonarMask : Mask
{
    [Header("Sonar Settings")]
    public GameObject sonarWavePrefab;
    public float sonarRange = 15f;
    public float sonarCooldown = 2f;
    public LayerMask detectionLayers;

    [Header("Sonar Beam Settings")]
    public float beamWidth = 1f;
    public float beamDuration = 1f;
    public float beamForce = 500f;
    public LayerMask destructibleLayers;

    private float lastSonarTime;
    private bool canUseSonar = true;

    public override void Initialize(PlayerController playerController)
    {
        base.Initialize(playerController);
        maskName = "Sonar Mask";
        speedMultiplier = 0.85f; // Heavier equipment
        oxygenConsumptionRate = 1.3f;
    }

    public override void OnEquip()
    {
        base.OnEquip();
        canUseSonar = true;
    }

    public override void OnUnequip()
    {
        base.OnUnequip();
    }
}