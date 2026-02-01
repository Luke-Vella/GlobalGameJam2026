using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class lightpulsescript : MonoBehaviour
{
    [Header("Light Settings")]
    [SerializeField] private Light2D lightSource;
    
    [Header("Pulse Settings")]
    [SerializeField] private float minIntensity = 0.7f;
    [SerializeField] private float maxIntensity = 0.9f;
    [SerializeField] private float pulseSpeed = 2f;
    
    private float pulseTimer = 0f;

    private void Start()
    {
        // Auto-assign Light2D if not set
        if (lightSource == null)
        {
            lightSource = GetComponent<Light2D>();
        }
    }

    private void Update()
    {
        if (lightSource == null) return;
        
        // Increment timer
        pulseTimer += Time.deltaTime * pulseSpeed;
        
        // Calculate intensity using sine wave for smooth pulsing
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, (Mathf.Sin(pulseTimer) + 1f) / 2f);
        
        // Apply intensity to light
        lightSource.intensity = intensity;
    }
}
