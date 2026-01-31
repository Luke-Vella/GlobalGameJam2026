using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public bool isPaused = false;

    void Update()
    {
        // Controlliamo se il giocatore preme il tasto Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume(); // Se è già in pausa, torna a giocare
            }
            else
            {
                Pause();  // Se sta giocando, metti in pausa
            }
        }
    }
    public void Resume()
    {
        PauseMenu.SetActive(false); // Nasconde il menu UI
        Time.timeScale = 1f;          // Riporta il tempo alla velocità normale
        isPaused = false;             // Aggiorna lo stato
    }

    void Pause()
    {
        PauseMenu.SetActive(true);  // Mostra il menu UI
        Time.timeScale = 0f;          // "Congela" il mondo di gioco
        isPaused = true;              // Aggiorna lo stato
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

}
