using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    public float damageAmount = 10f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

            // Check if player is invulnerable
            if (playerController.IsInvulnerable)
            {
                return;
            }

            playerController.Damage(damageAmount);
            Debug.Log($"Player oxygen: {playerController.CurrentOxygen}/{100f}");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            
            // Check if player is invulnerable
            if (playerController.IsInvulnerable)
            {
                return;
            }

            playerController.Damage(damageAmount);
            Debug.Log($"Player oxygen: {playerController.CurrentOxygen}/{100f}");
        }
    }
}
