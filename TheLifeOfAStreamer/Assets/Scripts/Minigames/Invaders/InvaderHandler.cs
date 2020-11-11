using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvaderHandler : MonoBehaviour
{
    public GameObject rootObj;
    public GameObject EnemyHolder;

    public Transform myUI;

    private int remainingEnemy = 0;
    private int score = 0;
    private int scoreGain = 3;
    private bool running = true;
    private bool movingStages = false;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.tag = "Untagged";
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (Globals.gameFlag == -1) {
            PauseAll();
            running = false;
        } else if (Globals.gameFlag != -1 && !running) {
            RunAll();
            running = true;
        }

        int existingEnemies = 0;
        foreach (Transform child in EnemyHolder.transform) {
            if (child.gameObject.activeSelf) existingEnemies++;
        }

        if (existingEnemies < remainingEnemy) {
            int multiplier = remainingEnemy - existingEnemies;
            remainingEnemy -= multiplier;

            score += scoreGain * multiplier;
        }

        if (existingEnemies == 0 && !movingStages) {
            movingStages = true;
            StartCoroutine(NextStage());
        }

        Globals.gameScore = (Globals.gameScore > score) ? Globals.gameScore : score;
        myUI.Find("Score").GetComponent<TextMesh>().text = "" + score;
    }

    IEnumerator StartGame() {
        yield return new WaitForSeconds(0.2f);

        Debug.Log("Start Called");
        PauseAll();

        GameObject GameOver = myUI.Find("GameOver").gameObject;
        GameOver.SetActive(true);

        GameOver.GetComponent<TextMesh>().text = "3";
        yield return new WaitForSeconds(1);
        GameOver.GetComponent<TextMesh>().text = "2";
        yield return new WaitForSeconds(1);
        GameOver.GetComponent<TextMesh>().text = "1";
        yield return new WaitForSeconds(2);

        RunAll();

        GameOver.SetActive(false);
    }

    IEnumerator NextStage() {
        PauseAll();

        GameObject GameOver = myUI.Find("GameOver").gameObject;
        GameOver.SetActive(true);

        GameOver.GetComponent<TextMesh>().text = "Next Stage...";
        yield return new WaitForSeconds(3);
        EnemyHolder.gameObject.GetComponent<InvaderObject>().Reset();
        EnemyHolder.gameObject.GetComponent<InvaderObject>().Pause();
        GameOver.GetComponent<TextMesh>().text = "3";
        yield return new WaitForSeconds(1);
        GameOver.GetComponent<TextMesh>().text = "2";
        yield return new WaitForSeconds(1);
        GameOver.GetComponent<TextMesh>().text = "1";
        yield return new WaitForSeconds(1);
        GameOver.GetComponent<TextMesh>().text = "GO!";
        
        RunAll();

        yield return new WaitForSeconds(1);
        GameOver.SetActive(false);
        movingStages = false;
        remainingEnemy = EnemyHolder.transform.childCount;
    }

    public void GameOver() {
        PauseAll();

        Globals.gameFlag = -1;

        myUI.Find("GameOver").gameObject.SetActive(true);
        myUI.Find("GameOver").gameObject.GetComponent<TextMesh>().text = "Game Over!";
    }

    public void Reset() {
        score = 0;
        remainingEnemy = EnemyHolder.transform.childCount;
        movingStages = false;

        myUI.Find("GameOver").gameObject.SetActive(false);

        foreach (Transform child in transform) {
            if (child.name == "LaserHolder") continue;

            child.gameObject.SetActive(true);
            child.gameObject.GetComponent<InvaderObject>().Reset();
        }

        StartCoroutine(StartGame());
    }

    private void PauseAll() {
        foreach (Transform child in transform) {
            if (child.name == "LaserHolder") continue;
            child.gameObject.GetComponent<InvaderObject>().Pause();
        }
    }

    private void RunAll() {
        foreach (Transform child in transform) {
            if (child.name == "LaserHolder") continue;
            child.gameObject.GetComponent<InvaderObject>().Run();
        }
    }
}
