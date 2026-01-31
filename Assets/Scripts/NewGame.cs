using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class NewGame : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartGame()
    {
        SceneManager.LoadScene(1);

        SceneManager.LoadScene(3, LoadSceneMode.Additive);

    }

    public void OpenCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void GoBack()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
