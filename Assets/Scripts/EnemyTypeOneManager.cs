using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeOneManager : MonoBehaviour
{
    private bool isHovering = true;
    private bool isWindingUp = false;
    private bool isCollided = false;
    private float windUpTimer = 0f;
    private float coolDownTimer = 0f;
    private Vector2 engageDirection = new Vector2();

    [Header("Hover Settings")]
    public float hoverRadius = 4f;
    public float hoverSpeed = 1.5f;
    public float hoverPointTolerance = 0.2f;

    [Header("Engage Settings")]
    public float engageSpeed = 6f;
    public float velocityDamping = 4f;
    public float playerDetectionRange = 5f;
    public float rotationSpeed = 720f;
    public float windUpDuration = 0.5f;
    public float shakeIntensity = 0.08f;
    public float shakeFrequency = 35f;
    private Vector3 originalLocalPosition;
    private float shakeTime;
    public float coolDown = 1f;

    [Header("Physics")]
    public LayerMask obstacleMask;

    private Rigidbody2D rb;
    private Transform player;
    private Vector2 currentTarget;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalLocalPosition = transform.localPosition;
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
        else if (isWindingUp)
        {
            // Handle winding up state
            windUpTimer += Time.fixedDeltaTime;

            ApplyShake();

            if (windUpTimer >= windUpDuration)
            {
                AttackPlayer();
            }
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
        isCollided = false;
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
    bool CheckForPlayer()
    {
        if (Vector2.Distance(rb.position, player.position) <= playerDetectionRange)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                rb.position,
                (Vector2)player.position - rb.position,
                Vector2.Distance(rb.position, (Vector2)player.position),
                obstacleMask
            );

            if (!hit)
            {
                EngagePlayer();
                return true;
            }
        }
        return false;
    }

    void EngagePlayer()
    {
        isHovering = false;
        currentTarget = rb.position; //nullify hover target
        //set wind up direction
        engageDirection = (player.position - transform.position).normalized;
        RotateTowards(engageDirection); //currently wind up will start during rotation
        isWindingUp = true;
        shakeTime = 0f;
        originalLocalPosition = transform.localPosition;
    }

    void ApplyShake()
    {
        shakeTime += Time.fixedDeltaTime;

        float x = Mathf.Sin(shakeTime * shakeFrequency) * shakeIntensity;
        float y = Mathf.Cos(shakeTime * shakeFrequency * 1.3f) * shakeIntensity;

        transform.localPosition = originalLocalPosition + new Vector3(x, y, 0f);
    }

    void AttackPlayer()
    {
        transform.localPosition = originalLocalPosition; // stop shake
        rb.velocity = engageDirection * engageSpeed;
        windUpTimer = 0; // reset wind-up
        isWindingUp = false;
    }

    void EngageBehaviour()
    {
        // Gradually slow down
        rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, velocityDamping * Time.fixedDeltaTime);

        // When nearly stopped
        if (rb.velocity.magnitude < 0.2f)
        {
            Debug.Log("Entering Cooldown");
            coolDownTimer += Time.fixedDeltaTime;

            if (coolDownTimer >= coolDown)
            {
                Debug.Log("Cooldown passed");
                if(!CheckForPlayer()) //if player no longer in range, go back to hovering
                {
                    isHovering = true;
                    PickNewHoverTarget();
                }
                coolDownTimer = 0; // reset cooldown
            }
        }
        isCollided = false;
    }

    void RotateTowards(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
        );
    }

    void ResetMovement()
    {
        // Stop physics movement
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Reset states
        isHovering = true;
        isWindingUp = false;

        windUpTimer = 0f;
        coolDownTimer = 0f;
        shakeTime = 0f;

        // Clear targets
        currentTarget = rb.position;
        engageDirection = Vector2.zero;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && !isCollided)
        {
            ResetMovement();
            isCollided = true;
        }
    }
}