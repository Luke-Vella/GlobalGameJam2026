using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private Rigidbody2D rb;
    public float velocityDamping = 6f;
    public float floatSpeed = 0.25f; // Keep below 0.3f to avoid loop
    public float floatTime = 10f; // Seconds to float before destroying

    private bool isFloating = false;
    private float startFloatY;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!isFloating)
        {
            // Gradually slow down
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, velocityDamping * Time.fixedDeltaTime);

            // When nearly stopped start floating up
            if (rb.velocity.magnitude < 0.5f)
            {
                StartFloating();
            }
        }
        else
        {
            // Handle floating behavior
            HandleFloating();
        }
    }

    private void StartFloating()
    {
        isFloating = true;
        startFloatY = transform.position.y;
        rb.gravityScale = 0f; // Disable gravity
        rb.velocity = new Vector2(0f, floatSpeed); // Set gentle upward velocity
    }

    private void HandleFloating()
    {
        // Keep consistent upward movement
        rb.velocity = new Vector2(0f, floatSpeed);
        floatTime -= Time.fixedDeltaTime;

        // Check if we've floated long enough
        if (floatTime <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
