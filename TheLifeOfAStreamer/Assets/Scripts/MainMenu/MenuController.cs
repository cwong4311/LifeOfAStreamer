using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void SetGame(int gameType) {
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

    private void TransferGlobals() {
        Globals.gameType = gameMode;
        Globals.webcamEnabled = this.webcamEnabled;
        //Globals.platformSetting = streamPlatform;
    }

    private void EnableGameScreen() {
        myMenus[currentMenu].SetActive(true);
    }
}
