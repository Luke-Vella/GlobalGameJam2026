using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject WarningMenu;
    public bool isPaused = false;

    void Update()
    {
        // if player plays ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume(); // if paused, resume
            }
            else
            {
                Pause();  // if not paused, pause
            }
        }
    }
    public void Resume()
    {
        PauseMenu.SetActive(false); // hids UI
        Time.timeScale = 1f;          // time back to normal
        isPaused = false;
    }

    void Pause()
    {
        PauseMenu.SetActive(true);  // shows UI
        Time.timeScale = 0f;          // time is blocked
        isPaused = true;
    }

    public void OpenWarning()
    {
        WarningMenu.SetActive(true);  // shows warning

    }

    public void CloseWarning()
    {
        WarningMenu.SetActive(false);  // hids warning

    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

}
