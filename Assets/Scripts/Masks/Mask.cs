using UnityEngine;

public abstract class Mask : MonoBehaviour
{
    [Header("Mask Properties")]
    public string maskName;
    public int maskID;
    public Sprite maskIcon;

    [Header("Movement Modifiers")]
    public float speedMultiplier = 1f;
    public float oxygenConsumptionRate = 1f;

    protected PlayerController player;

    public virtual void Initialize(PlayerController playerController)
    {
        player = playerController;
    }

    private void Update()
    {
        player.Damage(oxygenConsumptionRate * Time.deltaTime);

        Debug.Log($"Mask: {maskName}, Current Oxygen level : {player.CurrentOxygen}");  
    }

    public virtual void OnEquip()
    {
        Debug.Log($"Equipped {maskName}");
    }

    public virtual void OnUnequip()
    {
        Debug.Log($"Unequipped {maskName}");
    }

    public virtual void UpdateMask()
    {
        // Called every frame while mask is equipped
    }

    public virtual void UsePrimaryAbility()
    {
        // Override in derived classes for specific abilities
    }

    public virtual void UseSecondaryAbility()
    {
        // Override in derived classes for specific abilities
    }
}