using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBubble : MonoBehaviour
{
    public float boostAmount = 100f; // Total oxygen to give
    public float boostDuration = 5f; // Duration over which to give the oxygen
    private SpriteRenderer spriteRenderer;

    public bool isGivingOxygen = false;
    public bool canGiveOxygen = true;
    public float timerForNextActivation = 10f; // Time before the bubble can be used again
    private float activationTimer = 0f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(!canGiveOxygen)
        {
            activationTimer -= Time.deltaTime;
            if(activationTimer <= 0f)
            {
                canGiveOxygen = true;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canGiveOxygen && !isGivingOxygen)
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            GiveGradualOxygen(player);
        }
    }

    public void GiveGradualOxygen(PlayerController player)
    {
        player.StartCoroutine(GradualOxygenBoost(player));
    }

    public IEnumerator GradualOxygenBoost(PlayerController player)
    {
        isGivingOxygen = true;
        float boostPerSecond = boostAmount / boostDuration;
        float elapsed = 0f;
        while (elapsed < boostDuration)
        {
            player.currentOxygen = Mathf.Min(player.currentOxygen + boostPerSecond * Time.deltaTime, 100f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Deactivate();
    }

    public void Deactivate()
    {
        canGiveOxygen = false;
        isGivingOxygen = false;

        // reduce opacity of bubble to indicate cooldown
        Color color = spriteRenderer.color;
        color.a = 0.5f;
        spriteRenderer.color = color;

        activationTimer = timerForNextActivation;
    }
}
