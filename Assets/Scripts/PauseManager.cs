using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("Main Menus")]
    public GameObject PauseMenu;
    public GameObject WarningMenu;
    public GameObject InstructionsMenu;

    public GameObject ControlsScreen;
    public GameObject InstructionsScreen;

    [Header("Pages")]
    public GameObject[] instructionPages;

    [Header("Variables")]
    private int currentPageIndex = 0;
    public bool isPaused = false;

    [Header("Buttons")]
    public GameObject BackButton;
    public GameObject NextButton;
    public GameObject PlayButton;

    void Start()
    {
        Time.timeScale = 0f;
    }

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

    public void NextPage()
    {
        if (currentPageIndex < instructionPages.Length - 1)
        {
            currentPageIndex++;
            ShowCurrentPage();
        }
    }

    public void PreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            ShowCurrentPage();
        }
    }

    private void ShowCurrentPage()
    {
        for (int i = 0; i < instructionPages.Length; i++)
        {
            instructionPages[i].SetActive(i == currentPageIndex);
        }

        BackButton.SetActive(currentPageIndex != 0);
        NextButton.SetActive(currentPageIndex != 2);
        PlayButton.SetActive(currentPageIndex == 2);

    }

    public void Resume()
    {
        PauseMenu.SetActive(false); // hids UI
        Time.timeScale = 1f;          // time back to normal
        isPaused = false;
    }

    void Pause()
    {
        AudioManager.Instance.PlaySFX(AudioDatabase.Instance.PauseMenuClip);

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

    public void OpenInstructions()
    {
        InstructionsMenu.SetActive(true);
        PauseMenu.SetActive(false);
    }
    public void ContinueInstructions()
    {
        InstructionsScreen.SetActive(false);
        ControlsScreen.SetActive(true);
    }

    public void CloseInstructions()
    {
        InstructionsMenu.SetActive(false);  // hids InstructionsMenu
        Resume();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
