using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideTentacle : MonoBehaviour
{
    [Header("Slide Settings")]
    public float shootOutSpeed = 15f;
    public float retractSpeed = 10f;
    public float shakeDuration = 0.5f;
    public float shakeIntensity = 0.08f;
    public float shakeFrequency = 35f;
    public float shootOutDistance = 8f;
    public float retractDistance = 2f;

    [Header("Direction Detection")]
    public bool isVulnerable = false; // Set this based on prefab type

    public int remainingHealthPoints;
    public Vector2 slideDirection;

    private Vector3 originalPosition;
    private bool hasAttacked = false;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        //Start with shake effect
        Vector3 originalLocalPosition = transform.localPosition;
        float shakeTimer = 0f;

        while (shakeTimer < shakeDuration)
        {
            float x = Mathf.Sin(shakeTimer * shakeFrequency) * shakeIntensity;
            float y = Mathf.Cos(shakeTimer * shakeFrequency * 1.3f) * shakeIntensity;

            transform.localPosition = originalLocalPosition + new Vector3(x, y, 0f);
            shakeTimer += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalLocalPosition;

        // Then shoot out based on shootOutSpeed until a certain distance is reached
        Vector2 targetPosition = (Vector2)originalPosition + slideDirection * shootOutDistance;
        float shootOutTime = 0f;
        float totalShootOutTime = shootOutDistance / shootOutSpeed;

        while (shootOutTime < totalShootOutTime)
        {
            float progress = shootOutTime / totalShootOutTime;
            transform.position = Vector2.Lerp((Vector2)originalPosition, targetPosition, progress);
            shootOutTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        hasAttacked = true;

        // Brief pause at full extension
        yield return new WaitForSeconds(0.2f);

        // Then retract back to further back from the original position based on retractSpeed
        Vector2 retractPosition = (Vector2)originalPosition - slideDirection * retractDistance;
        float retractTime = 0f;
        float totalRetractDistance = Vector2.Distance(targetPosition, retractPosition);
        float totalRetractTime = totalRetractDistance / retractSpeed;

        while (retractTime < totalRetractTime)
        {
            float progress = retractTime / totalRetractTime;
            transform.position = Vector2.Lerp(targetPosition, retractPosition, progress);
            retractTime += Time.deltaTime;
            yield return null;
        }

        transform.position = retractPosition;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.CompareTag("Player") && hasAttacked)
        //{
        //    // Apply damage to player
        //    PlayerController playerController = other.GetComponent<PlayerController>();
        //    if (playerController != null)
        //    {
        //        float damageAmount = isVulnerable ? 15f : 25f; // More damage from armored tentacles
        //        playerController.currentOxygen = Mathf.Max(0f, playerController.currentOxygen - damageAmount);
        //        Debug.Log($"Player hit by {(isVulnerable ? "vulnerable" : "armored")} tentacle for {damageAmount} damage!");
        //    }
        //}
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle collision with environment/walls
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || 
            collision.gameObject.CompareTag("Wall"))
        {
            // Stop movement if hitting a wall during shoot out
            StopAllCoroutines();
            StartCoroutine(RetractEarly());
        }
    }

    IEnumerator RetractEarly()
    {
        // If tentacle hits a wall, retract immediately
        Vector2 retractPosition = (Vector2)originalPosition - slideDirection * retractDistance;
        float retractTime = 0f;
        float totalRetractDistance = Vector2.Distance(transform.position, retractPosition);
        float totalRetractTime = totalRetractDistance / retractSpeed;

        while (retractTime < totalRetractTime)
        {
            float progress = retractTime / totalRetractTime;
            transform.position = Vector2.Lerp(transform.position, retractPosition, progress);
            retractTime += Time.deltaTime;
            yield return null;
        }

        transform.position = retractPosition;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    // Public method to take damage (for vulnerable tentacles)
    public void TakeDamage(float damage)
    {
        if (isVulnerable)
        {
            Debug.Log($"Vulnerable tentacle took {damage} damage!");
            // Add health system here if needed
            // For now, just destroy immediately when hit
            StopAllCoroutines();
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Armored tentacle is immune to damage!");
        }
    }
}
