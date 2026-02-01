using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPocket : MonoBehaviour
{
    public float boostAmount = 100f; // Total oxygen to give
    public float oxygenPerSecond = 20f; // Oxygen given per second while player is inside
    public float decayDuration = 4.5f; // Duration over which the air pocket decays
    [SerializeField] private SpriteRenderer spriteRenderer;
    private PlayerController _player;

    public bool isGivingOxygen = false;
    private float remainingCapacity = 100f; // Current oxygen capacity
    private Coroutine decayCoroutine;

    private void Start()
    {
        remainingCapacity = boostAmount;
    }

    private void Update()
    {
        // Give oxygen while player is inside and capacity remains
        if (isGivingOxygen && _player != null && remainingCapacity > 0f)
        {
            float oxygenToGive = oxygenPerSecond * Time.deltaTime;
            oxygenToGive = Mathf.Min(oxygenToGive, remainingCapacity);
            
            _player.ReplenishOxygen(oxygenToGive);
            remainingCapacity -= oxygenToGive;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            
            if (player && remainingCapacity > 0f)
            {
                _player = player;
                isGivingOxygen = true;
                
                // Start decay if not already decaying
                if (decayCoroutine == null)
                {
                    decayCoroutine = StartCoroutine(DecayAirPocket());
                    AudioManager.Instance.PlayBackgroundB(AudioDatabase.Instance.AirPocketClip);
                }
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(isGivingOxygen)
            {
                isGivingOxygen = false;
                _player = null;
            }

            AudioManager.Instance.StopBackgroundB();
            // Decay continues even after player exits
        }
    }

    private IEnumerator DecayAirPocket()
    {
        float elapsed = 0f;
        float startCapacity = remainingCapacity;
        
        Color color = spriteRenderer.color;
        float startAlpha = color.a;

        while (elapsed < decayDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / decayDuration;
            
            // Decay capacity over time
            remainingCapacity = Mathf.Lerp(startCapacity, 0f, progress);
            
            // Gradually reduce opacity as air pocket decays
            color.a = Mathf.Lerp(startAlpha, 0.1f, progress);
            spriteRenderer.color = color;
            
            yield return null;
        }

        // Ensure fully depleted
        remainingCapacity = 0f;
        color.a = 0.1f;
        spriteRenderer.color = color;
        
        isGivingOxygen = false;
        decayCoroutine = null;
    }
}
