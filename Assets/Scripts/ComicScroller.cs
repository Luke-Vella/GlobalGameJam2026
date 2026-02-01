using UnityEngine;

public class CameraScroller : MonoBehaviour
{
    public float speed = 15f; // Puoi alzare o abbassare questo valore dall'Inspector

    void Update()
    {
        // Prende l'input orizzontale (A/D o frecce Sinistra/Destra)
        // Restituisce -1 per A, 1 per D, 0 se non premi nulla
        float moveX = Input.GetAxis("Horizontal"); 

        // Prende l'input verticale (W/S o frecce Su/Giù)
        // Restituisce 1 per W, -1 per S, 0 se non premi nulla
        float moveY = Input.GetAxis("Vertical");

        // Crea un vettore di movimento
        Vector3 movement = new Vector3(moveX, moveY, 0);

        // Muove la camera. 
        // Time.deltaTime serve a rendere il movimento fluido indipendentemente dalla potenza del PC.
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }
}