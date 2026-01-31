using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FuelCell : MonoBehaviour
{
    [SerializeField] private TMP_Text fuelCellText;
    private bool isWithinRange = false;

    private void Start()
    {
        if (fuelCellText != null)
        {
            fuelCellText.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(fuelCellText != null)
            {
                fuelCellText.enabled = true;
            }
            isWithinRange = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (fuelCellText != null)
            {
                fuelCellText.enabled = false;
            }
            isWithinRange = false;
        }
    }
}
