using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LScreenController : MonoBehaviour
{
    public GameObject[] myGames;

    private GameObject spawnedGame = null;
    private int chooseGame = 0;

    public GameObject resetButton;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (spawnedGame.tag != "GameOver") {
            resetButton.SetActive(false);
        } else {
            resetButton.SetActive(true);
        }
    }

    void OnEnable()
    {
        Globals.gameSetting = -1;
        int delay; GameObject messageBox;

        switch(chooseGame) {
            case 0:
            default:
                if (spawnedGame == null) spawnedGame = Instantiate(myGames[0]);
                spawnedGame.SetActive(true);
                delay = 3; messageBox = spawnedGame.transform.Find("UI/GameOver").gameObject;
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

        while (t > 0) {
            countdown.GetComponent<TextMesh>().text = "" + t;
            yield return new WaitForSeconds(1);
            t--;
        }
        countdown.GetComponent<TextMesh>().text = "Go";
        Globals.gameSetting = 0;

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
