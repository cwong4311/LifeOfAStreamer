using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeMenu : MonoBehaviour
{
    public GameObject confirmation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowConfirmation()
    {
        if (Globals.GameExists())
        {
            confirmation.SetActive(true);
        } else
        {
            NewGame();
        }
    }

    public void CloseConfirmation()
    {
        confirmation.SetActive(false);
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Streamer_0.6-MenuEndingsAndGames");
    }

    public void LoadGame()
    {
        Globals.LoadGame();
        SceneManager.LoadScene("Streamer_0.6-MenuEndingsAndGames");
    }

    public void GetPastResults()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }
}
