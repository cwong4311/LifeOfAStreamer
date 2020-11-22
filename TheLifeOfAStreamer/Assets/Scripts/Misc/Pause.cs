using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public MonoBehaviour player;
    public GameObject pauseMenu;
    public GameObject Fader;

    public int wasGameFlag = -1;
    private bool wasEnabledOnPause = false;
    private bool selectorEnabledOnPause = false;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (pauseMenu.activeSelf) {
                Continue();
            } else {
                Paused();
            }
        }
    }

    public void Paused() {
        if (player.enabled) {
            wasEnabledOnPause = true;
            player.enabled = false;
            
        } else {
            wasEnabledOnPause = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        wasGameFlag = Globals.gameFlag;
        if (Globals.gameType != 3) Globals.gameFlag = -1;

        Time.timeScale = 0f;
        pauseMenu.SetActive(true);

        selectorEnabledOnPause = player.GetComponentInChildren<Selector>().enabled;
        player.GetComponentInChildren<Selector>().enabled = false;
    }

    public void Continue() {
        if (wasEnabledOnPause) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            player.enabled = true;
        }

        Globals.gameFlag = wasGameFlag;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        player.GetComponentInChildren<Selector>().enabled = selectorEnabledOnPause;
    }

    public void LeaveGame() {
        //Fader.GetComponent<DayHandler>().StopPostProcessing();
        Fader.GetComponent<DayHandler>().LeaveGame();
        Continue();
    }
}
