using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Underwater Movement")]
    public float swimSpeed = 2f;
    public float drag = 2f;
    public float maxSpeed = 10f;

    [Header("Rotation")]
    public float rotationSpeed = 5f; // How quickly rotation accelerates
    public float maxRotationSpeed = 180f; // Maximum degrees per second
    public float rotationDrag = 3f; // How quickly rotation decelerates

    public Rigidbody2D Rb { get; private set; }
    public Vector2 MoveInput { get; set; }

    public PlayerStateMachine StateMachine { get; private set; }
    public IdleSwimState IdleSwimState { get; private set; }
    public SwimState SwimState { get; private set; }

    private Camera mainCamera;
    private float currentRotationVelocity = 0f; // Current rotation speed

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        StateMachine = new PlayerStateMachine();
        IdleSwimState = new IdleSwimState(this, StateMachine);
        SwimState = new SwimState(this, StateMachine);
    }

    private void Start()
    {
        // Configure Rigidbody2D for underwater physics
        Rb.gravityScale = 0f; // No gravity underwater
        Rb.drag = drag;

        StateMachine.Initialize(IdleSwimState);
    }

    private void Update()
    {
        StateMachine.Update();
        HandleRotation();
    }

    private void FixedUpdate()
    {
        StateMachine.FixedUpdate();

        // Clamp velocity to max speed
        if (Rb.velocity.magnitude > maxSpeed)
        {
            Rb.velocity = Rb.velocity.normalized * maxSpeed;
        }
    }

    private void HandleRotation()
    {
        // Get mouse position directly from Unity's input system
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        
        // Convert to world space
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, mainCamera.nearClipPlane));
        mouseWorldPos.z = 0f;

        // Calculate direction from player to mouse
        Vector2 direction = (mouseWorldPos - transform.position);
        
        // Only rotate if there's meaningful distance (prevents jittering when mouse is on player)
        if (direction.magnitude < 0.1f)
        {
            // Apply drag when not rotating
            currentRotationVelocity = Mathf.Lerp(currentRotationVelocity, 0f, rotationDrag * Time.deltaTime);
            return;
        }

        direction.Normalize();

        // Calculate target angle in degrees
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        float currentAngle = transform.eulerAngles.z;

        // Calculate the shortest angle difference
        float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);

        // Accelerate rotation velocity towards the target
        float targetRotationVelocity = angleDifference * rotationSpeed;
        currentRotationVelocity = Mathf.Lerp(currentRotationVelocity, targetRotationVelocity, rotationSpeed * Time.deltaTime);

        // Clamp rotation velocity
        currentRotationVelocity = Mathf.Clamp(currentRotationVelocity, -maxRotationSpeed, maxRotationSpeed);

        // Apply rotation with momentum
        float newAngle = currentAngle + currentRotationVelocity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }
}
