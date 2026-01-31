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
            playerController.currentOxygen -= damageAmount;
            Debug.Log($"Player oxygen: {playerController.currentOxygen}/{100f}");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.currentOxygen -= damageAmount;
            Debug.Log($"Player oxygen: {playerController.currentOxygen}/{100f}");

            // If this is a projectile, destroy it
            Destroy(gameObject);
        }
    }
}
