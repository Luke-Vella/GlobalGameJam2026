using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public void OnPrimaryActionPressed(InputAction.CallbackContext context)
    {
        if (context.performed && isWithinRange)
        {
            // Increment fuel cell count in FuelCellManager
            FuelCellManager.Instance.fuelCellsAcquired += 1;

            AudioManager.Instance.PlaySFX(AudioDatabase.Instance.PickingUpFuelCellClip);

            // Destroy the fuel cell object
            Destroy(this.gameObject);
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
