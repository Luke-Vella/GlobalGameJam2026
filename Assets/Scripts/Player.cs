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
    public Light2D lightForward;
    public Light2D lightBackward;

    [Header("Rotation")]
    public float rotationSpeed = 5f;
    public float maxRotationSpeed = 180f;
    public float rotationDrag = 3f;
    public float movingRotationMultiplier = 0.3f;
    public float currentOxygen = 100f;

    [Header("Sprite Settings")]
    public SpriteRenderer spriteRenderer;

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
    private bool isFacingLeft = false;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        // Auto-assign sprite renderer if not set
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

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
        if(currentOxygen <= 0f)
        {
            GameStateManager.Instance.Restart();
            AudioManager.Instance.PlaySFX(AudioDatabase.Instance.GameOverClip);
        }


        StateMachine.Update();

        HandleRotation();
        
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

    [ContextMenu("Test Flip")]

    private void HandleFlip()
    {
        Debug.Log("Handled");
    }


    private void HandleRotation()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, mainCamera.nearClipPlane));
        mouseWorldPos.z = 0f;

        Vector2 direction = (mouseWorldPos - transform.position);
        
        if (direction.magnitude < 0.1f)
        {
            return;
        }

        direction.Normalize();

        // Calculate angle in degrees
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Subtract 90 because sprite faces up by default
        targetAngle -= 90f;

        // Directly set rotation without any smoothing or velocity
        transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
    }

    private void UpdateLightOrientation(bool facingLeft)
    {
        if (lightForward == null || lightBackward == null) return;

        if (facingLeft)
        {
            // Use backward light when facing left
            if (lightForward.enabled)
            {
                bool wasOn = lightForward.enabled;
                lightForward.enabled = false;
                lightBackward.enabled = wasOn; // Preserve on/off state
            }
        }
        else
        {
            // Use forward light when facing right
            if (lightBackward.enabled || !lightForward.enabled)
            {
                bool wasOn = lightBackward.enabled;
                lightBackward.enabled = false;
                lightForward.enabled = wasOn; // Preserve on/off state
            }
        }
    }

    public void SetLightState(bool isOn)
    {
        // Public method to turn lights on/off (called by LumenMask)
        if (isFacingLeft)
        {
            if (lightBackward != null)
                lightBackward.enabled = isOn;
        }
        else
        {
            if (lightForward != null)
                lightForward.enabled = isOn;
        }
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

        AudioManager.Instance.PlaySFX(AudioDatabase.Instance.ToggleBetweenMasksClip);

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
        AudioManager.Instance.PlaySFX(AudioDatabase.Instance.SpeedBoostClip);
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
