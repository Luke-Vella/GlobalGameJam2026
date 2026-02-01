using UnityEngine;
using UnityEngine.UI;

public class MaskUIHandler : MonoBehaviour
{
    // Aggiungendo [SerializeField] appariranno gli slot nell'Inspector
    [SerializeField] private GameObject defaultMask;
    [SerializeField] private GameObject sonarMask;
    [SerializeField] private GameObject lumenMask;

    private PlayerController _playerController;

    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();

        // Rimuoviamo i FindObjectOfType<GameObject>() che avevi qui.
        // Non servono più perché li trascini tu a mano!
    }

    void Update()
    {
        if (_playerController != null)
        {
            // Recuperiamo l'ID attuale dal Player
            int maskID = _playerController.GetCurrentMask().maskID;

            // Attiviamo l'oggetto giusto in base all'ID
            // Questo metodo è più corto e pulito dei vari "if"
            if (defaultMask != null) defaultMask.SetActive(maskID == 0);
            if (sonarMask != null)   sonarMask.SetActive(maskID == 1);
            if (lumenMask != null)   lumenMask.SetActive(maskID == 2);
        }
    }
}