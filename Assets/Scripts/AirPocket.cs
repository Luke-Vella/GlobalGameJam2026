using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPocket : MonoBehaviour
{
    [Header("Oxygen Settings")]
    public float maxOxygenCapacity = 100f; // Maximum oxygen this air pocket can provide
    public float oxygenPerSecond = 20f; // Oxygen given per second while player is inside
    
    [Header("Timing Settings")]
    public float activeDuration = 5f; // Duration the air pocket dispenses oxygen
    public float cooldownDuration = 10f; // Cooldown after expiration before reuse
    
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private PlayerController _player;
    private bool isPlayerInside = false;
    private bool isActive = false; // Currently dispensing oxygen
    private bool isOnCooldown = false;
    
    private float activeTimer = 0f; // Tracks how long the pocket has been active
    private float cooldownTimer = 0f;
    
    private Color initialColor;

    private void Start()
    {
        if (spriteRenderer != null)
        {
            initialColor = spriteRenderer.color;
        }
    }

    private void Update()
    {
        // Handle active state (dispensing oxygen)
        if (isActive)
        {
            activeTimer += Time.deltaTime;
            
            // Dispense oxygen if player is inside
            if (isPlayerInside && _player != null)
            {
                float oxygenToGive = oxygenPerSecond * Time.deltaTime;
                _player.ReplenishOxygen(oxygenToGive);
            }
            
            // Check if active duration expired
            if (activeTimer >= activeDuration)
            {
                ExpireAirPocket();
            }
        }
        
        // Handle cooldown state
        if (isOnCooldown)
        {
            cooldownTimer += Time.deltaTime;
            
            if (cooldownTimer >= cooldownDuration)
            {
                ResetAirPocket();
            }
        }
        
        UpdateVisuals();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            
            if (player != null && !isOnCooldown)
            {
                _player = player;
                isPlayerInside = true;
                
                // Start active state if not already active
                if (!isActive)
                {
                    isActive = true;
                    activeTimer = 0f;
                    AudioManager.Instance.PlayBackgroundB(AudioDatabase.Instance.AirPocketClip);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;
            _player = null;
            
            if (isActive)
            {
                AudioManager.Instance.StopBackgroundB();
            }
            
            // Note: Active timer continues even if player exits
        }
    }

    private void ExpireAirPocket()
    {
        isActive = false;
        isOnCooldown = true;
        activeTimer = 0f;
        cooldownTimer = 0f;
        
        // Stop audio if still playing
        if (isPlayerInside)
        {
            AudioManager.Instance.StopBackgroundB();
        }
    }

    private void ResetAirPocket()
    {
        isOnCooldown = false;
        cooldownTimer = 0f;
    }

    private void UpdateVisuals()
    {
        if (spriteRenderer == null) return;
        
        Color color = initialColor;
        
        if (isOnCooldown)
        {
            // Show as opaque/faded during cooldown
            color.a = 0.2f;
        }
        else if (isActive)
        {
            // Fade based on remaining time during active state
            float remainingPercent = 1f - (activeTimer / activeDuration);
            color.a = Mathf.Lerp(0.3f, initialColor.a, remainingPercent);
        }
        else
        {
            // Full opacity when ready
            color.a = initialColor.a;
        }
        
        spriteRenderer.color = color;
    }
}
