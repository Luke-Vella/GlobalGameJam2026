using UnityEngine;
using TMPro; // Necessario per usare TextMeshPro

public class FuelDisplay : MonoBehaviour
{
    private TMP_Text textComponent;

    void Start()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (FuelCellManager.Instance != null)
        {
            textComponent.text = "" + FuelCellManager.Instance.fuelCellsAcquired;
        }
    }
}