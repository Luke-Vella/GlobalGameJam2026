using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelCellManager : MonoBehaviour
{
    public int fuelCellsAcquired = 0;

    public static FuelCellManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
