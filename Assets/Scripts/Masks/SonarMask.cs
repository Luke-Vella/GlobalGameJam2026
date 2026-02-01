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
    
    [Header("Fuel Cell Tracking")]
    public float trackingDuration = 10f;
    public float trackingCooldown = 15f;

    private float lastSonarTime;
    private float lastTrackingTime;
    private bool canUseSonar = true;
    private bool canUseTracking = true;

    public override void Initialize(PlayerController playerController)
    {
        base.Initialize(playerController);
        maskName = "Sonar Mask";
        speedMultiplier = 0.85f;
        oxygenConsumptionRate = 1.3f;
    }

    public override void UsePrimaryAbility()
    {
        if (!canUseTracking || SonarManager.Instance == null)
        {
            return;
        }

        // Activate fuel cell tracking
        SonarManager.Instance.ActivateSonar(player.transform);
        AudioManager.Instance.PlaySFX(AudioDatabase.Instance.SonarOnOffClip);
        
        canUseTracking = false;
        StartCoroutine(TrackingCooldownRoutine());
    }

    public override void UseSecondaryAbility()
    {
        // Your existing sonar wave/beam ability
        if (!canUseSonar)
        {
            return;
        }

        // Emit sonar wave for detection
        if (sonarWavePrefab != null)
        {
            Instantiate(sonarWavePrefab, player.transform.position, Quaternion.identity);
        }

        canUseSonar = false;
        StartCoroutine(SonarCooldownRoutine());
    }

    private IEnumerator TrackingCooldownRoutine()
    {
        yield return new WaitForSeconds(trackingCooldown);
        canUseTracking = true;
    }

    private IEnumerator SonarCooldownRoutine()
    {
        yield return new WaitForSeconds(sonarCooldown);
        canUseSonar = true;
    }

    public override void OnEquip()
    {
        base.OnEquip();
        canUseSonar = true;
        canUseTracking = true;
    }

    public override void OnUnequip()
    {
        base.OnUnequip();
        
        // Deactivate sonar when unequipping
        if (SonarManager.Instance != null && SonarManager.Instance.IsSonarActive())
        {
            SonarManager.Instance.DeactivateSonar();
        }
    }
}