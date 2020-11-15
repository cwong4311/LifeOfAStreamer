using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject[] myMenus;    // Game Screen always last item
    private int gameMode = 0;
    private int streamPlatform = 0;
    private int currentMenu = 0;
    private bool gameStarted = false;
    private bool webcamEnabled = false;
    private bool micEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    void OnEnable()
    {
        if (gameStarted) EnableGameScreen();
    }

    void OnDisable() 
    {
        myMenus[myMenus.Length - 1].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMenu < myMenus.Length - 1) {
            if (!myMenus[currentMenu].activeSelf) {
                myMenus[currentMenu].SetActive(true);
                if (currentMenu >= 1) myMenus[currentMenu - 1].SetActive(false);
            }
        } else {
            if (!gameStarted) {
                gameStarted = true;
                TransferGlobals();
                EnableGameScreen();
                foreach (Transform child in transform) {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    public void SetGame(int gameType) { // 1 = plat, 2 = inv, 3 = mem
        this.gameMode = gameType;
        currentMenu++;
    }

    public void SetPlatform(int platform) {
        this.streamPlatform = platform;
        currentMenu++;
    }

    public void SetWebcamAndMic(bool webcam, bool mic)
    {
        this.webcamEnabled = webcam;
        this.micEnabled = mic;
        currentMenu++;
    }

    public void GoToPage(int pageNum)
    {
        myMenus[currentMenu].SetActive(false);
        currentMenu = pageNum;
    }

    public void PostOrReserve(int postType)
    {
        int pageNum;

        switch(postType) {
            case 0:
                Globals.hasPosted = true;
                pageNum = 3;
                break;
            default:
                int bookedDay = int.Parse(myMenus[currentMenu].transform.Find("InputField").gameObject.GetComponent<InputField>().text);
                Globals.reserveDays += bookedDay + ",";
                pageNum = 4;
                break;
        }

        myMenus[currentMenu].SetActive(false);
        currentMenu = pageNum;
    }

    private void TransferGlobals() {
        Globals.gameType = gameMode;
        Globals.webcamEnabled = this.webcamEnabled;
        Globals.platformSetting = streamPlatform;
    }

    private void EnableGameScreen() {
        myMenus[currentMenu].SetActive(true);
    }
}
