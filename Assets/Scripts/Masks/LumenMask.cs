using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LumenMask : Mask
{
    private bool isLightOn = false;

    public override void Initialize(PlayerController playerController)
    {
        base.Initialize(playerController);
        maskName = "Lumen Mask";
        speedMultiplier = 0.9f; // Slightly slower due to weight
        oxygenConsumptionRate = 1.2f; // Uses more oxygen
    }

    public override void OnEquip()
    {
        base.OnEquip();
        // Don't turn on light automatically - wait for player to press E
    }

    public override void OnUnequip()
    {
        base.OnUnequip();
        // Turn off light when unequipping mask
        if (isLightOn)
        {
            TurnOffLight();
        }
    }

    public override void UpdateMask()
    {
        // Light orientation is handled by PlayerController
    }

    public override void UsePrimaryAbility()
    {
        ToggleTorch();
    }

    private void ToggleTorch()
    {
        isLightOn = !isLightOn;

        AudioManager.Instance.PlaySFX(AudioDatabase.Instance.LightOnOffClip);

        if (isLightOn)
        {
            TurnOnLight();
        }
        else
        {
            TurnOffLight();
        }

        Debug.Log($"Lumen Mask: Light {(isLightOn ? "ON" : "OFF")}");
    }

    private void TurnOnLight()
    {
        player.SetLightState(true);
    }

    private void TurnOffLight()
    {
        player.SetLightState(false);
        isLightOn = false;
    }
}