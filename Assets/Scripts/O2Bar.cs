using UnityEngine;
using UnityEngine.UI;

public class BarraTempo : MonoBehaviour
{
    public Image O2Bar; 
    public float tempoMassimo = 10f;
    private float tempoRimanente;

    private Vector2 posizioneIniziale;
    private float altezzaBarra;

    void Start()
    {
        tempoRimanente = tempoMassimo;

        // 1. Impostiamo il Pivot a 1 via codice per essere sicuri
        // (X resta 0.5 per il centro, Y diventa 1 per il Top)
        O2Bar.rectTransform.pivot = new Vector2(0.5f, 1f);

        // 2. Salviamo la posizione di partenza
        posizioneIniziale = O2Bar.rectTransform.anchoredPosition;

        // 3. Prendiamo l'altezza esatta
        altezzaBarra = O2Bar.rectTransform.rect.height;
    }

    void Update()
    {
        if (tempoRimanente > 0)
        {
            tempoRimanente -= Time.deltaTime;
            float fill = tempoRimanente / tempoMassimo;

            // Svuota la barra dal basso
            O2Bar.fillAmount = fill;

            // Sposta il rettangolo verso il basso. 
            // Quando fill è 1 (piena), lo spostamento è 0.
            // Quando fill è 0.5 (metà), scende di metà altezza.
            float offset = 0.445f * (1 - fill) * altezzaBarra;
            O2Bar.rectTransform.anchoredPosition = new Vector2(posizioneIniziale.x, posizioneIniziale.y - offset);
        }
    }
}