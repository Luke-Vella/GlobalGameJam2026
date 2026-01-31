using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    [Header("Underwater Movement")]
    public float swimSpeed = 2f;
    public float drag = 2f;
    public float maxSpeed = 10f;

    [Header("Boost Settings")]
    public float boostMultiplier = 4f;

    [Header("Lumen Mask Settings")]
    public Light2D lightSource;

    [Header("Rotation")]
    public float rotationSpeed = 5f;
    public float maxRotationSpeed = 180f;
    public float rotationDrag = 3f;
    public float movingRotationMultiplier = 0.3f;

    [Header("Mask System")]
    public Mask[] availableMasks; // Assign masks in order: [0] = Default, [1] = Lumen, [2] = Sonar
    private int currentMaskIndex = 0;
    private Mask currentMask;

    public Rigidbody2D Rb { get; private set; }
    public Vector2 MoveInput { get; set; }
    public bool IsBoostPressed { get; private set; }

    public PlayerStateMachine StateMachine { get; private set; }
    public IdleSwimState IdleSwimState { get; private set; }
    public SwimState SwimState { get; private set; }

    private Camera mainCamera;
    private float currentRotationVelocity = 0f;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        StateMachine = new PlayerStateMachine();
        IdleSwimState = new IdleSwimState(this, StateMachine);
        SwimState = new SwimState(this, StateMachine);

        InitializeMasks();
    }

    private void Start()
    {
        Rb.gravityScale = 0f;
        Rb.drag = drag;

        StateMachine.Initialize(IdleSwimState);
        
        // Equip default mask (index 0)
        EquipMask(0);
    }

    private void Update()
    {
        StateMachine.Update();
        
        // Only handle rotation when not boosting
        if (!IsBoostPressed)
        {
            HandleRotation();
        }
        else
        {
            // Apply drag to rotation velocity when boosting
            currentRotationVelocity = Mathf.Lerp(currentRotationVelocity, 0f, rotationDrag * Time.deltaTime);
        }
        
        if (currentMask != null)
        {
            currentMask.UpdateMask();
        }
    }

    private void FixedUpdate()
    {
        StateMachine.FixedUpdate();

        // Apply mask speed multiplier
        float effectiveMaxSpeed = maxSpeed;
        if (currentMask != null)
        {
            effectiveMaxSpeed *= currentMask.speedMultiplier;
        }

        if (Rb.velocity.magnitude > effectiveMaxSpeed)
        {
            Rb.velocity = Rb.velocity.normalized * effectiveMaxSpeed;
        }
    }

    private void HandleRotation()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, mainCamera.nearClipPlane));
        mouseWorldPos.z = 0f;

        Vector2 direction = (mouseWorldPos - transform.position);
        
        if (direction.magnitude < 0.1f)
        {
            currentRotationVelocity = Mathf.Lerp(currentRotationVelocity, 0f, rotationDrag * Time.deltaTime);
            return;
        }

        direction.Normalize();

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        float currentAngle = transform.eulerAngles.z;

        float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);

        // Determine if player is moving
        bool isMoving = MoveInput.magnitude > 0.1f;
        
        // Apply rotation speed multiplier when moving
        float effectiveRotationSpeed = isMoving ? rotationSpeed * movingRotationMultiplier : rotationSpeed;
        float effectiveMaxRotationSpeed = isMoving ? maxRotationSpeed * movingRotationMultiplier : maxRotationSpeed;

        float targetRotationVelocity = angleDifference * effectiveRotationSpeed;
        currentRotationVelocity = Mathf.Lerp(currentRotationVelocity, targetRotationVelocity, effectiveRotationSpeed * Time.deltaTime);

        currentRotationVelocity = Mathf.Clamp(currentRotationVelocity, -effectiveMaxRotationSpeed, effectiveMaxRotationSpeed);

        float newAngle = currentAngle + currentRotationVelocity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
    }

    private void InitializeMasks()
    {
        foreach (Mask mask in availableMasks)
        {
            if (mask != null)
            {
                mask.Initialize(this);
            }
        }
    }

    private void EquipMask(int index)
    {
        if (index < 0 || index >= availableMasks.Length) return;
        if (availableMasks[index] == null) return;

        // Don't re-equip if already equipped
        if (currentMaskIndex == index && currentMask != null)
        {
            Debug.Log($"Mask {index} already equipped");
            return;
        }

        // Unequip current mask
        if (currentMask != null)
        {
            currentMask.OnUnequip();
        }

        // Equip new mask
        currentMaskIndex = index;
        currentMask = availableMasks[currentMaskIndex];
        
        if (currentMask != null)
        {
            currentMask.OnEquip();
        }
    }

    public void CycleMask(bool forward = true)
    {
        int direction = forward ? 1 : -1;
        int newIndex = (currentMaskIndex + direction + availableMasks.Length) % availableMasks.Length;
        EquipMask(newIndex);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    public void OnBoost(InputAction.CallbackContext context)
    {
        IsBoostPressed = context.ReadValueAsButton();
    }

    // Number key inputs for mask selection
    public void OnMaskSlot0(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            EquipMask(0);
        }
    }

    public void OnMaskSlot1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            EquipMask(1);
        }
    }

    public void OnMaskSlot2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            EquipMask(2);
        }
    }

    // 'E' key to activate current mask ability
    public void OnActivateMaskAbility(InputAction.CallbackContext context)
    {
        if (context.performed && currentMask != null)
        {
            currentMask.UsePrimaryAbility();
        }
    }

    // Keep old cycling method for compatibility
    public void OnCycleMask(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            CycleMask(true);
        }
    }

    public void OnMaskPrimaryAbility(InputAction.CallbackContext context)
    {
        if (context.performed && currentMask != null)
        {
            currentMask.UsePrimaryAbility();
        }
    }

    public void OnMaskSecondaryAbility(InputAction.CallbackContext context)
    {
        if (context.performed && currentMask != null)
        {
            currentMask.UseSecondaryAbility();
        }
    }

    public Mask GetCurrentMask()
    {
        return currentMask;
    }
}
