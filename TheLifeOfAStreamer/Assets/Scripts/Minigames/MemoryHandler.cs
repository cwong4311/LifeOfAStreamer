using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryHandler : MonoBehaviour
{
    private bool initialised = false;
    private ArrayList possibleNumbers = new ArrayList();
    private ArrayList gameNumbers = new ArrayList();
    private int totalPossibility = 52;
    private int gameSize = 50;
    private float myCounter;

    private int matchedNumber = 0;
    private bool currentlyComparing = false;

    public GameObject gameOver;
    public GameObject Timer;
    public GameObject myRootObject;

    public Button resetButton;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.tag = "Untagged";
        gameOver.SetActive(false);
        myCounter = 0f;

        // Flush for reset
        possibleNumbers.Clear();
        gameNumbers.Clear();

        if (!initialised) {
            
            //Populate possibleNumbers
            for (int i = 1; i <= totalPossibility; i++) {
                possibleNumbers.Add(i);
            }
            //Possible numbers used in Game
            for (int i = 0; i < (gameSize / 2); i++) {
                int takeThisIndex = Random.Range(0, possibleNumbers.Count);
                
                // Insert twice
                gameNumbers.Add(possibleNumbers[takeThisIndex]);
                gameNumbers.Add(possibleNumbers[takeThisIndex]);

                // Remove the number from further selection
                possibleNumbers.RemoveAt(takeThisIndex);
            }

            if (gameNumbers.Count != 50) Debug.Log("Incorrect Item Count");

            populateCards();

            myCounter = 0f;
            initialised = true;
        }
    }

    void onEnable() {
        if (!initialised) Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (initialised) {
            //Update Timer
            myCounter += Time.deltaTime;
            Timer.GetComponent<Text>().text = "" + (int)myCounter;

            // Check if all cards are matched
            int existingCards = 0;
            foreach(Transform child in transform)
            {
                if (child.gameObject.activeSelf) existingCards++;
            }

            // If all cards matched, game over
            if (existingCards == 0) {
                GameOver();
                initialised = false;
                gameOver.SetActive(true);
            }
        }

        if (Globals.gameFlag == -1) {
            myRootObject.SetActive(false);
            GameOver();
        }
    }

    public void Reset() {
        //Turn all children on
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        initialised = false;
        gameOver.SetActive(false);
        Start();
    }

    public void GameOver() {
        Globals.gameFlag = -1;
        int totalScore = (420 - (int)myCounter > 0) ? (420 - (int)myCounter) : 0;
        Globals.gameScore = (Globals.gameScore > totalScore) ? Globals.gameScore : totalScore;
    }

    void populateCards() {
        foreach(Transform child in transform)
        {
            int takeThisNumber = Random.Range(0, gameNumbers.Count);
            child.gameObject.GetComponent<MemoryCards>().AssignNumber((int)gameNumbers[takeThisNumber]);

            gameNumbers.RemoveAt(takeThisNumber);
        }
    }

    public void ReportClicked(int clickedNumber) {

        if (matchedNumber == 0) {
            matchedNumber = clickedNumber;
        } else {
            toggleLeaveButton(false);

            foreach(Transform child in transform)
            {
                child.gameObject.GetComponent<MemoryCards>().togglePause(true);
            }
            StartCoroutine(CompareCards(clickedNumber));
        }
    }

    IEnumerator CompareCards(int clickedNumber) {
        yield return new WaitForSeconds(1);

        if (matchedNumber == clickedNumber) {
            foreach(Transform child in transform)
            {

                if (child.gameObject.GetComponent<MemoryCards>().IsFlipped()) {
                    child.gameObject.GetComponent<MemoryCards>().Reset();
                    child.gameObject.SetActive(false);
                }
            }
        } else {
            foreach(Transform child in transform)
            {
                if (child.gameObject.GetComponent<MemoryCards>().IsFlipped()) {
                    child.gameObject.GetComponent<MemoryCards>().Reset();
                }
            }
        }

        matchedNumber = 0;

        foreach(Transform child in transform)
        {
            child.gameObject.GetComponent<MemoryCards>().togglePause(false);
        }

        toggleLeaveButton(true); 
    }

    private void toggleLeaveButton(bool flag) {
        GameObject.Find("PlayerCanvas/ScreenCanvas/LeaveButton").GetComponent<Button>().interactable = flag;
        resetButton.interactable = flag;
    }
}
