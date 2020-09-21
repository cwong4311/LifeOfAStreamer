using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LScreenController : MonoBehaviour
{
    public GameObject[] myGames;
    private int chooseGame = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        
        Globals.gameSetting = -1;
        int delay; GameObject messageBox;

        switch(chooseGame) {
            case 0:
            default:
                myGames[0].SetActive(true);
                delay = 3; messageBox = myGames[0].transform.Find("UI/GameOver").gameObject;
                break;
        }

        StartCoroutine(Countdown(delay, messageBox));
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
}
