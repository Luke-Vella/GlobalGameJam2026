using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBarHandler : MonoBehaviour
{

    public Slider fuelSlider;

    void Start()
    {
        fuelSlider = GetComponent<Slider>();
    }

    void Update()
    {
        if (FuelCellManager.Instance != null)
        {
            fuelSlider.value = FuelCellManager.Instance.fuelCellsAcquired;
        }
    }
}
