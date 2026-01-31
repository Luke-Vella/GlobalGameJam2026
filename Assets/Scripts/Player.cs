using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Underwater Movement")]
    public float swimSpeed = 5f;
    public float drag = 2f;
    public float maxSpeed = 10f;

    public Rigidbody2D Rb { get; private set; }
    public Vector2 MoveInput { get; set; }

    public PlayerStateMachine StateMachine { get; private set; }
    public IdleSwimState IdleSwimState { get; private set; }
    public SwimState SwimState { get; private set; }

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();

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

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // Jump removed - underwater movement only
    }
}
