using UnityEngine;
using UnityEngine.UI;

public class SonarIndicator : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RectTransform indicatorRect;
    [SerializeField] private Image indicatorImage;

    [Header("Visual Settings")]
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float minAlpha = 0.3f;
    [SerializeField] private float maxAlpha = 0.6f;
    [SerializeField] private float edgeOffset = 50f;

    private Transform playerTransform;
    private Transform targetFuelCell;
    private Camera mainCamera;
    private bool isActive = false;
    private float pulseTimer;

    private void Awake()
    {
        mainCamera = Camera.main;
        if (indicatorImage != null)
        {
            indicatorImage.enabled = false;
        }
    }

    public void ActivateSonar(Transform player, Transform target)
    {
        playerTransform = player;
        targetFuelCell = target;
        isActive = true;

        if (indicatorImage != null)
        {
            indicatorImage.enabled = true;
        }
    }

    public void DeactivateSonar()
    {
        isActive = false;
        if (indicatorImage != null)
        {
            indicatorImage.enabled = false;
        }
    }

    private void Update()
    {
        if (!isActive || targetFuelCell == null || playerTransform == null)
        {
            return;
        }

        UpdateIndicatorPosition();
        UpdatePulseEffect();
    }

    private void UpdateIndicatorPosition()
    {
        Vector2 directionToTarget = (targetFuelCell.position - playerTransform.position).normalized;

        // Convert direction to screen space
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Vector3 targetScreenPos = mainCamera.WorldToScreenPoint(targetFuelCell.position);

        // Check if target is on screen
        bool isOnScreen = targetScreenPos.z > 0
            && targetScreenPos.x > 0 && targetScreenPos.x < Screen.width
            && targetScreenPos.y > 0 && targetScreenPos.y < Screen.height;

        if (isOnScreen)
        {
            // Point directly at target if visible
            indicatorRect.position = targetScreenPos;
        }
        else
        {
            // Place indicator at screen edge pointing to target
            Vector2 screenDir = (new Vector2(targetScreenPos.x, targetScreenPos.y) - new Vector2(screenCenter.x, screenCenter.y)).normalized;

            float angle = Mathf.Atan2(screenDir.y, screenDir.x) * Mathf.Rad2Deg;
            indicatorRect.rotation = Quaternion.Euler(0f, 0f, angle);

            // Clamp to screen edges
            Vector2 edgePosition = screenCenter;
            edgePosition.x += screenDir.x * (Screen.width / 2f - edgeOffset);
            edgePosition.y += screenDir.y * (Screen.height / 2f - edgeOffset);

            indicatorRect.position = edgePosition;
        }
    }

    private void UpdatePulseEffect()
    {
        pulseTimer += Time.deltaTime * pulseSpeed;
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(pulseTimer) + 1f) / 2f);

        if (indicatorImage != null)
        {
            Color color = indicatorImage.color;
            color.a = alpha;
            indicatorImage.color = color;
        }
    }
}