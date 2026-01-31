using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LumenMask : Mask
{
    [Header("Torch Settings")]
    public GameObject lightBeamPrefab;
    public float lightRange = 10f;
    public float lightIntensity = 1f;
    public float batteryDrain = 0.5f;

    private GameObject activeLightBeam;
    private Light2D torchLight;
    private bool isLightOn = false;

    public override void Initialize(PlayerController playerController)
    {
        base.Initialize(playerController);
        maskName = "Torch Mask";
        speedMultiplier = 0.9f; // Slightly slower due to weight
        oxygenConsumptionRate = 1.2f; // Uses more oxygen
    }

    public override void OnEquip()
    {
        base.OnEquip();
        CreateLightBeam();
    }

    public override void OnUnequip()
    {
        base.OnUnequip();
        DestroyLightBeam();
    }

    public override void UpdateMask()
    {
        if (isLightOn && activeLightBeam != null)
        {
            // Update light beam position and rotation to match player
            activeLightBeam.transform.position = player.transform.position;
            activeLightBeam.transform.rotation = player.transform.rotation;
        }
    }

    public override void UsePrimaryAbility()
    {
        ToggleTorch();
    }

    private void CreateLightBeam()
    {
        if (lightBeamPrefab != null)
        {
            activeLightBeam = Instantiate(lightBeamPrefab, player.transform.position, player.transform.rotation);
            activeLightBeam.transform.SetParent(player.transform);
            torchLight = activeLightBeam.GetComponent<Light2D>();
        }
        else
        {
            // Create a simple Light2D component if no prefab is assigned
            GameObject lightObj = new GameObject("TorchLight");
            lightObj.transform.SetParent(player.transform);
            lightObj.transform.localPosition = Vector3.zero;
            torchLight = lightObj.AddComponent<Light2D>();
            torchLight.lightType = UnityEngine.Rendering.Universal.Light2D.LightType.Point;
            torchLight.pointLightOuterRadius = lightRange;
            torchLight.intensity = lightIntensity;
            activeLightBeam = lightObj;
        }

        activeLightBeam.SetActive(false);
    }

    private void DestroyLightBeam()
    {
        if (activeLightBeam != null)
        {
            Destroy(activeLightBeam);
        }
    }

    private void ToggleTorch()
    {
        isLightOn = !isLightOn;
        if (activeLightBeam != null)
        {
            activeLightBeam.SetActive(isLightOn);
        }
    }
}