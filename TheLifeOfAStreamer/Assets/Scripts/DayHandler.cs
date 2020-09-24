using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DayHandler : MonoBehaviour
{
    public Animator myFade;
    public GameObject viewerSystem;

    private float dailyTimeLimit = 60f; //Time limit of each session, in Seconds.

    // Start is called before the first frame update
    void Start()
    {
        if (Globals.days == 1) runDailyQuote();
        SceneManager.sceneLoaded += OnSceneLoaded;

        StartCoroutine(DayLimitHandler());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DayEnd(float attitude, float popularity) {
        viewerSystem.GetComponent<ViewerControlSystem>().endDay();

        Globals.gameFlag = -1;
        Globals.days += 1;
        Globals.prevViewer = Globals.dayViewer;
        Globals.hasStreamed = false;
		
        Globals.attitude += attitude;
        Globals.popularity += popularity;
        Globals.gameScore = 0;
        Globals.dayViewer = 0;
        FadeOut();
    }
    public void DayEnd() {
        float attitude = Random.Range(-3f, 3f) + Globals.dayAttitude;   // TO DO: Better Formula
        float popularity = Random.Range(-3f, 3f) + (Globals.gameScore / 300f) + 
                                ((float) (Globals.dayViewer - Globals.prevViewer) / 10f) +
                                ((float) (Mathf.Min(Globals.dayAttitude, 50f) / 10f));
        DayEnd(attitude, popularity);
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
            if (Globals.prevAction == "stream") {
                myText = "Things didn't go too bad last time.\nHope today goes alright too";
            } else {
                myText = "I ended up putting it off yesterday\nHope today's the day";
            }
        } else if (Globals.days == 3) {
            if (Globals.prevAction == "stream") {
                myText = "Another day on the job.\nHere goes nothing!";
            } else {
                myText = "Gotta start somewhere. Let's stream today";
            }
        } else if (Globals.days == 4) {
            if (Globals.prevAction == "stream") {
                myText = "Yeah, I feel pretty used to streaming now.\nIf only I could actually start making money off this";
                myDuration = 4f;
            } else {
                myText = "Taking breaks like this might feel good but I'll never get anywhere";
                myDuration = 4f;  
            }
        } else if (Globals.days == 60) {
            myText = "GAME OVER! End of Demo";
        } else {
            if (Globals.attitude > -THRESHOLD && Globals.attitude < THRESHOLD) {
                if (Globals.prevAction == "stream") {
                    myText = "Let's stream again today!";
                } else {
                    myText = "I didn't end up streaming yesterday..\nI probably should today";
                }
            } else if (Globals.attitude >= THRESHOLD && Globals.attitude < THRESHOLD*2) {
                if (Globals.prevAction == "stream") {
                    myText = "Streaming's pretty fun!";
                } else {
                    myText = "I got a good break yesterday.\nMy viewers are waiting for me!";
                }
            } else if (Globals.attitude >= THRESHOLD*2) {
                myText = "Let's get right into it again today!";
            } else if (Globals.attitude > -THRESHOLD*2 && Globals.attitude <= -THRESHOLD) {
                if (Globals.prevAction == "stream") {
                    myText = "Another day of streaming";
                } else {
                    myText = "Maybe I should go somewhere and fun today";
                    myColor = Color.red;                    
                }
            } else if (Globals.attitude <= -THRESHOLD*2) {
                if (Globals.prevAction == "stream") {
                    myText = "Gotta stream again today...";
                    myColor = Color.red;
                } else {
                    myText = "Is streaming really what I want to do..?";
                    myColor = Color.red;
                }
            } else if (Globals.attitude <= -THRESHOLD*3) {
                if (Globals.prevAction == "stream") {
                    myText = "I'm pretty done with this whole thing...";
                    myColor = Color.red;
                } else {
                    myText = "Even though I didn't stream yesterday, I still feel so bad today";
                    myColor = Color.red;
                }
            }
        }

        myMessage.SetText(myText, myDuration, myDelay, myColor);
    }

    IEnumerator DayLimitHandler()
    {
        TextHandler myMessage = GameObject.Find("PlayerMessage").GetComponent<TextHandler>();
        string myText = ""; float myDuration = 3f; float myDelay = 0.5f; Color myColor = Color.white;
        int dayMilestones = 0;
        float time = 0f;

        while (time < dailyTimeLimit)
        {
            yield return new WaitForSeconds(1);
            time++;

            if (time >= dailyTimeLimit * 0.7f && dayMilestones == 0)
            {
                dayMilestones++;
                myText = "I'm starting to get a bit tired";
                while (!myMessage.SetText(myText, myDuration, myDelay, myColor))
                {
                    yield return new WaitForSeconds(6);
                }
            }
            else if (time >= dailyTimeLimit * 0.9f && dayMilestones == 1)
            {
                dayMilestones++;
                myText = "Should probably wrap up soon...";
                while (!myMessage.SetText(myText, myDuration, myDelay, myColor))
                {
                    yield return new WaitForSeconds(6);
                }
            }
        }

        myText = "I'm gonna pass ou-...";
        myColor = Color.red;
        while (!myMessage.SetText(myText, myDuration, myDelay, myColor))
        {
            yield return new WaitForSeconds(6);
        }
        Globals.attitude -= 5f;

        yield return new WaitForSeconds(3);

        DayEnd();
    }
}
