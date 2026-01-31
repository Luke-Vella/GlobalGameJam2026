using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeOneManager : MonoBehaviour
{
    public bool isHovering = true;

    [Header("Hover Settings")]
    public float hoverRadius = 4f;
    public float hoverSpeed = 1.5f;
    public float hoverPointTolerance = 0.2f;

    [Header("Engage Settings")]
    public float engageSpeed = 6f;
    public float velocityDamping = 4f;
    public float playerDetectionRange = 5f;
    public float coolDown = 1f;

    [Header("Physics")]
    public LayerMask obstacleMask;

    private Rigidbody2D rb;
    private Transform player;
    private Vector2 currentTarget;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        PickNewHoverTarget();
    }

    void FixedUpdate()
    {
        if (isHovering)
        {
            HoverBehaviour();
            CheckForPlayer();
        }
        else
        {
            EngageBehaviour();
        }
    }

    // =========================
    // Hovering
    // =========================
    void HoverBehaviour()
    {
        Vector2 dir = (currentTarget - rb.position);

        if (dir.magnitude < hoverPointTolerance)
        {
            PickNewHoverTarget();
            return;
        }

        rb.velocity = dir.normalized * hoverSpeed;
    }

    void PickNewHoverTarget()
    {
        for (int i = 0; i < 10; i++) // try multiple times
        {
            Vector2 randomPoint = rb.position + Random.insideUnitCircle * hoverRadius;

            // Check line of sight
            RaycastHit2D hit = Physics2D.Raycast(
                rb.position,
                randomPoint - rb.position,
                Vector2.Distance(rb.position, randomPoint),
                obstacleMask
            );

            if (!hit)
            {
                currentTarget = randomPoint;
                return;
            }
        }

        // fallback: stay still if nothing found
        currentTarget = rb.position;
    }

    // =========================
    // Engagement
    // =========================
    void CheckForPlayer()
    {
        if (Vector2.Distance(rb.position, player.position) <= playerDetectionRange)
        {
            EngagePlayer();
        }
    }

    void EngagePlayer()
    {
        isHovering = false;

        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * engageSpeed;
    }

    void EngageBehaviour()
    {
        // Gradually slow down
        rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, velocityDamping * Time.fixedDeltaTime);

        // When nearly stopped
        if (rb.velocity.magnitude < 0.2f)
        {

            if (Vector2.Distance(rb.position, player.position) <= playerDetectionRange)
            {
                if (coolDown < 0)
                {
                    EngagePlayer(); // re-attack
                    coolDown = 1.0f; // reset cooldown
                }
                else
                {
                    coolDown -= Time.fixedDeltaTime;
                }
            }
            else
            {
                isHovering = true;
                PickNewHoverTarget();
            }

        }
    }
}