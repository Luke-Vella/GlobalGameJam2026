using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeTwoManager : MonoBehaviour
{
    private bool isHovering = true;
    private bool isCollided = false;
    private bool isRotating = false; 
    private bool isOnCooldown = false;
    private float coolDownTimer = 0f;
    private Vector2 lookDirection = new Vector2();

    [Header("Hover Settings")]
    public float hoverRadius = 4f;
    public float hoverSpeed = 1.5f;
    public float hoverPointTolerance = 0.2f;

    [Header("Shooter Settings")]
    public float projectileSpeed = 6f;
    public float velocityDamping = 4f;
    public float playerDetectionRange = 5f;
    public float rotationSpeed = 720f;
    private Vector3 originalPosition;
    public float coolDown = 1f;
    public GameObject projectileObject;

    [Header("Physics")]
    public LayerMask obstacleMask;

    private Rigidbody2D rb;
    private Transform player;
    private Vector2 currentTarget;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
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
            Debug.Log("Entered hover stage");
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
            Vector2 randomPoint = originalPosition + Random.insideUnitCircle * hoverRadius; //constrained to move around in a space around original position

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
        lookDirection = (player.position - transform.position).normalized;
        StartCoroutine(RotateTowards(lookDirection));
    }

    void ShootPlayer()
    {
        //spawn gameobject
        //setup velocity etc
    }

    void EngageBehaviour() // This needs to move to the projectile class later
    {
        // Gradually slow down
        rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, velocityDamping * Time.fixedDeltaTime);

        // When nearly stopped
        if (rb.velocity.magnitude < 0.2f)
        {
            isOnCooldown = true;
            coolDownTimer += Time.fixedDeltaTime;

            if (coolDownTimer >= coolDown)
            {
                if (!CheckForPlayer()) //if player no longer in range, go back to hovering
                {
                    isHovering = true;
                    PickNewHoverTarget();
                }
                coolDownTimer = 0; // reset cooldown
                isOnCooldown = false;
            }
        }
        isCollided = false;
    }

    IEnumerator RotateTowards(Vector2 direction)
    {
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );
            isRotating = true;

            yield return new WaitForFixedUpdate();
        }

        // Snap exactly at the end
        transform.rotation = targetRotation;
        isRotating = false;
    }

    void ResetMovement()
    {
        // Stop physics movement
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Clear targets
        currentTarget = rb.position;

        Debug.Log("Resetting Movement after Collision");
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