using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DayHandler : MonoBehaviour
{
    public Animator myFade;
    public GameObject viewerSystem;
    // Start is called before the first frame update
    void Start()
    {
        if (Globals.days == 1) runDailyQuote();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DayEnd() {
        viewerSystem.GetComponent<ViewerControlSystem>().endDay();

        Globals.gameSetting = -1;
        Globals.days += 1;
        Globals.prevViewer = Globals.dayViewer;
		
        Globals.attitude += Random.Range(-3f, 3f) + Globals.dayAttitude; // TO DO: Add Factors
        Globals.popularity += Random.Range(-3f, 3f) + (Globals.gameScore / 300f) + 
                                ((float) (Globals.dayViewer - Globals.prevViewer) / 10f) +
                                ((float) (Mathf.Min(Globals.dayAttitude, 50f) / 10f));
        Globals.gameScore = 0;
        Globals.dayViewer = 0;
        FadeOut();
    }

    public void FadeOut() {
        myFade.SetTrigger("FadeOut");
    }

    public void OnFadeComplete() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SaveScene() {
        // Write Global Params to SaveFile
    }

    public void LoadScene(int sceneNum) {
        Globals.LoadScene = sceneNum;
        FadeOut();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (Globals.LoadScene != -1) {
            // Load in Globals Params from SaveFile
            Globals.LoadScene = -1;
        }
        
        runDailyQuote();
    }

    private void runDailyQuote() {
        TextHandler myMessage = GameObject.Find("PlayerMessage").GetComponent<TextHandler>();
        float THRESHOLD = Globals.mentalThreshold;
        string myText = ""; float myDuration = 3f; float myDelay = 0.5f; Color myColor = Color.white;

        if (Globals.days == 1) {
            myText = "Finally got my streaming equipment all set up.\nI can finally start streaming!";
            myDuration = 4f;
        } else if (Globals.days == 2) {
            myText = "Things didn't go too bad last time.\nHope today goes alright too";
        } else if (Globals.days == 3) {
            myText = "Third day on the job.\nHere goes nothing!";
        } else if (Globals.days == 4) {
            myText = "Yeah, I feel pretty used to streaming now.\nIf only I could actually start making money off this";
            myDuration = 4f;
        } else if (Globals.days == 60) {
            myText = "GAME OVER! End of Demo";
        } else {
            if (Globals.attitude > -THRESHOLD && Globals.attitude < THRESHOLD) {
                myText = "Let's stream again today!";
            } else if (Globals.attitude >= THRESHOLD && Globals.attitude < THRESHOLD*2) {
                myText = "Streaming's pretty fun!";
            } else if (Globals.attitude >= THRESHOLD*2) {
                myText = "Let's get right into it again today!";
            } else if (Globals.attitude > -THRESHOLD*2 && Globals.attitude <= -THRESHOLD) {
                myText = "Another day of streaming";
            } else if (Globals.attitude <= -THRESHOLD*2) {
                myText = "Gotta stream again today...";
                myColor = Color.red;
            }
        }

        myMessage.SetText(myText, myDuration, myDelay, myColor);
    }
}
