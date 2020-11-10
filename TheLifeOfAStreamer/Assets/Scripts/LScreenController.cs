using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LScreenController : MonoBehaviour
{
    public GameObject[] myGames;

    private GameObject spawnedGame = null;

    public GameObject resetButton;

    public GameObject GameSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (spawnedGame != null) {
            if (spawnedGame.tag != "GameOver") {
                resetButton.SetActive(false);
            } else {
                resetButton.SetActive(true);
            }
        }
    }

    void OnEnable()
    {
        Globals.gameFlag = -1;
        int delay; GameObject messageBox;
        Debug.Log(Globals.gameType);
        switch(Globals.gameType) {
            default:
            case 1:
                if (spawnedGame == null) spawnedGame = Instantiate(myGames[0], GameSpawnPoint.transform);
                spawnedGame.SetActive(true); Globals.hasStreamed = true;
                delay = 3; messageBox = spawnedGame.transform.Find("UI/GameOver").gameObject;
                if (!Globals.webcamEnabled) { spawnedGame.transform.Find("UI/WebCam").gameObject.SetActive(false); }
                break;
            //case 2:
            case 3:
                if (spawnedGame == null) spawnedGame = Instantiate(myGames[1], GameSpawnPoint.transform);
                spawnedGame.SetActive(true); Globals.hasStreamed = true;
                delay = 3; messageBox = spawnedGame.transform.Find("GameOver").gameObject;
                if (!Globals.webcamEnabled) { spawnedGame.transform.Find("WebCam").gameObject.SetActive(false); }
                break;
        }

        if (spawnedGame.tag != "GameOver") {
            StartCoroutine(Countdown(delay, messageBox));
        } else {
            messageBox.SetActive(true);
        }
    }

    public IEnumerator Countdown(int duration, GameObject countdown)
    {
        int t = duration;
        countdown.SetActive(true);

        try {
            countdown.GetComponent<TextMesh>().text = "";
        } catch (Exception e2) {
            Globals.gameFlag = 0;
            countdown.SetActive(false);
            yield break;
        }

        // Do the countdown
        while (t > 0) {
            countdown.GetComponent<TextMesh>().text = "" + t;
            yield return new WaitForSeconds(1);
            t--;
        }
        countdown.GetComponent<TextMesh>().text = "Go";
        Globals.gameFlag = 0;

        yield return new WaitForSeconds(1);

        countdown.GetComponent<TextMesh>().text = "Game Over!";
        countdown.SetActive(false);
    }

    public void ResetGame() {
        Destroy(spawnedGame);
        spawnedGame = null;
        OnEnable();
    }
}
