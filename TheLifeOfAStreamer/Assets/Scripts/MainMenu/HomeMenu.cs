using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeMenu : MonoBehaviour
{
    public GameObject confirmation;
    public GameObject namePlate;

    // Start is called before the first frame update
    void Start()
    {
        // Cap framerate to 60
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 60;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (!Globals.GameExists())
        {
            GameObject continueButton = GameObject.Find("ContinueButton");
            GameObject continueText = GameObject.Find("Continue");

            continueButton.GetComponent<Button>().interactable = false;
            continueText.GetComponent<Text>().color = Color.grey;
        }
        if (!Globals.ResultExists())
        {
            GameObject resultsButton = GameObject.Find("ResultsButton");
            GameObject resultsText = GameObject.Find("Results");

            resultsButton.GetComponent<Button>().interactable = false;
            resultsText.GetComponent<Text>().color = Color.grey;
        }
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
            ShowName();
        }
    }

    public void ShowName()
    {
        CloseConfirmation();
        namePlate.SetActive(true);
    }

    public void FinishNameInput()
    {
        Globals.username = namePlate.transform.Find("InputField").gameObject.GetComponent<InputField>().text;
        NewGame();
    }

    public void CloseConfirmation()
    {
        confirmation.SetActive(false);
    }

    public void NewGame()
    {
        Globals.DeleteGame();
        SceneManager.LoadScene("Streamer_0.6-MenuEndingsAndGames");
    }

    public void LoadGame()
    {
        if (Globals.LoadGame())
        {
            SceneManager.LoadScene("Streamer_0.6-MenuEndingsAndGames");
        } else
        {
            Debug.Log("No Currently Active Game");
        }
    }

    public void GetPastResults()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }
}
