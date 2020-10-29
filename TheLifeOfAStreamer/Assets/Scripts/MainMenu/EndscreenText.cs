using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndscreenText : MonoBehaviour
{
    public GameObject[] endScreens;
    public GameObject CameraPan;

    private int pageNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        pageNum = 1;
        DisplayScreen();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        CameraPan.GetComponent<CameraPan>().ToggleController(false);
    }

    public void DisplayScreen()
    {
        switch(pageNum)
        {
            case 1:
                endScreens[0].SetActive(true);
                break;
            case 2:
                endScreens[1].SetActive(true);
                break;
            case 3:
                endScreens[2].SetActive(true);
                break;
            case 4:
                endScreens[3].SetActive(true);
                break;
            case 5:
                endScreens[4].SetActive(true);
                break;
            default:
            case 6:
                endScreens[5].SetActive(true);
                break;
        }
    }

    public void IncrementPage()
    {
        endScreens[pageNum - 1].SetActive(false);

        pageNum++;
        DisplayScreen();
    }

    public void FinishPlay()
    {
        Globals.DeleteGame();
        SceneManager.LoadScene("Menu_0.1-MainMenuPrototpye");
    }
}
