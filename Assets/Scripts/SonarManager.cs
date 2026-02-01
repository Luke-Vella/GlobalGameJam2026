using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SonarManager : MonoBehaviour
{
    [Header("Sonar Settings")]
    [SerializeField] private float sonarDuration = 10f;
    
    [Header("UI Reference")]
    [SerializeField] private SonarIndicator sonarIndicator;
    
    private Transform playerTransform;
    private Transform currentTargetFuelCell;
    private float sonarActiveTimer = 0f;
    private bool isSonarActive = false;

    public static SonarManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (isSonarActive)
        {
            sonarActiveTimer -= Time.deltaTime;
            
            if (sonarActiveTimer <= 0f)
            {
                DeactivateSonar();
            }
            else if (currentTargetFuelCell == null)
            {
                // Target was collected, find next one
                FindNearestFuelCell();
            }
        }
    }

    public void ActivateSonar(Transform player)
    {
        playerTransform = player;
        isSonarActive = true;
        sonarActiveTimer = sonarDuration;
        
        FindNearestFuelCell();
        
        if (currentTargetFuelCell != null && sonarIndicator != null)
        {
            sonarIndicator.ActivateSonar(playerTransform, currentTargetFuelCell);
        }
        else
        {
            // No fuel cells left
            DeactivateSonar();
        }
    }

    public void DeactivateSonar()
    {
        isSonarActive = false;
        sonarActiveTimer = 0f;
        currentTargetFuelCell = null;
        
        if (sonarIndicator != null)
        {
            sonarIndicator.DeactivateSonar();
        }
    }

    private void FindNearestFuelCell()
    {
        FuelCell[] allFuelCells = FindObjectsOfType<FuelCell>();
        
        if (allFuelCells.Length == 0)
        {
            currentTargetFuelCell = null;
            return;
        }

        FuelCell nearest = allFuelCells
            .OrderBy(fc => Vector2.Distance(playerTransform.position, fc.transform.position))
            .FirstOrDefault();

        currentTargetFuelCell = nearest != null ? nearest.transform : null;
    }

    public bool IsSonarActive()
    {
        return isSonarActive;
    }

    public float GetSonarTimeRemaining()
    {
        return sonarActiveTimer;
    }
}