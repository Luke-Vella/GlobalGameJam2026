using UnityEngine;

public class DefaultMask : Mask
{
    [Header("Default Mask Settings")]
    public float extraOxygenCapacity = 100f;
    public float bonusSwimSpeed = 1.2f;

    public override void Initialize(PlayerController playerController)
    {
        base.Initialize(playerController);
        maskName = "Default Mask";
        speedMultiplier = bonusSwimSpeed;
        oxygenConsumptionRate = 0.8f; // Uses less oxygen
    }

    public override void OnEquip()
    {
        base.OnEquip();
        // Apply default mask benefits
    }

    public override void OnUnequip()
    {
        base.OnUnequip();
        // Remove default mask benefits
    }

    public override void UpdateMask()
    {
        // Default mask has no active abilities
    }
}