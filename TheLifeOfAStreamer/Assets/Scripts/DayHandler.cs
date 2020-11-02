using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class DayHandler : MonoBehaviour
{
    public Animator myFade;

    public GameObject viewerSystem;

    public GameObject myBlur;

    public GameObject myResultsScreen;

    public GameObject myEndScren;

    private Material blurObj;

    private readonly bool continuousPlay = true;

    private float dailyTimeLimit = 900f; //Time limit of each session, in Seconds.

    private bool leaveGame = false;

    private float dayPopularity = 0f;
    private float dayAttitude = 0f;
    private float timer = 0f;

    private int totalDays = 30;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(runDailyQuote());

        if (Globals.days >= 15) {dailyTimeLimit = 1800f;}   //Streamer can stream twice as long after they get used to it
        StartCoroutine(DayLimitHandler());

        if (myBlur != null) {
            blurObj = Instantiate(myBlur.GetComponent<Image>().material);
            myBlur.GetComponent<Image>().material = blurObj;
        }
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
        Globals.totalViewer += Globals.dayViewer;
        Globals.hasStreamed = false;
		
        Globals.attitude += attitude;
        Globals.popularity += popularity;
        Globals.gameScore = 0;
        Globals.dayViewer = 0;
        FadeOut();
    }

    public void DayEnd() {
        dayAttitude = Random.Range(-3f, 3f) + Globals.dayAttitude;   // TO DO: Better Formula
        dayPopularity = Random.Range(-3f, 3f) + (Globals.gameScore / 300f) + 
                                ((float) (Globals.dayViewer - Globals.prevViewer) / 10f) +
                                ((float) (Mathf.Min(Globals.dayAttitude, 50f) / 10f));
        DayEnd(dayAttitude, dayPopularity);
    }

    public void FadeOut() {
        leaveGame = false;
        myFade.SetTrigger("FadeOut");
    }

    public void LeaveGame()
    {
        leaveGame = true;
        myFade.SetTrigger("FadeOut");
    }

    public void OnFadeComplete() {
        if (leaveGame)
        {
            SceneManager.LoadScene("Menu_0.1-MainMenuPrototpye");
            return;
        }
        Globals.SaveGame();

        if (Globals.days > totalDays)
        {
            myEndScren.SetActive(true);
        }
        else
        {
            myResultsScreen.SetActive(true);

            myResultsScreen.GetComponent<ResultsText>().timeTag = (int) timer;
            myResultsScreen.GetComponent<ResultsText>().viewTag = "" + Globals.prevViewer;
            myResultsScreen.GetComponent<ResultsText>().moneyTag = "$0";
            myResultsScreen.GetComponent<ResultsText>().popDelta = dayPopularity;
            myResultsScreen.GetComponent<ResultsText>().attDelta = Globals.attitude;

            myResultsScreen.GetComponent<ResultsText>().RunScript();
        }
    }

    public void ProgressDay()
    {
        if (continuousPlay)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            SceneManager.LoadScene("Menu_0.1-MainMenuPrototpye");
        }
    }

    IEnumerator runDailyQuote() {
        TextHandler myDay = GameObject.Find("DayMessage").GetComponent<TextHandler>();
        TextHandler myMessage = GameObject.Find("PlayerMessage").GetComponent<TextHandler>();
        float THRESHOLD = Globals.mentalThreshold;
        string myText = ""; float myDuration = 3f; float myDelay = 0.5f; Color myColor = Color.white;

        myDay.SetText("Day " + Globals.days, 1.7f, 0.5f, myColor);

        yield return new WaitForSeconds(2);

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
        } else if (Globals.days == totalDays) {
            myText = "This is the last day...";
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
        bool triggerLapse = false;

        TextHandler myMessage = GameObject.Find("PlayerMessage").GetComponent<TextHandler>();
        string myText = ""; float myDuration = 3f; float myDelay = 0.5f; Color myColor = Color.white;
        int dayMilestones = 0;
        timer = 0f;

        while (timer < dailyTimeLimit)
        {
            yield return new WaitForSeconds(1);
            timer++;

            if (timer >= dailyTimeLimit * 0.7f && dayMilestones == 0)
            {
                dayMilestones++;
                myText = "I'm starting to get a bit tired";
                triggerLapse = true;
                while (!myMessage.SetText(myText, myDuration, myDelay, myColor))
                {
                    yield return new WaitForSeconds(6);
                }
            }
            else if (timer >= dailyTimeLimit * 0.9f && dayMilestones == 1)
            {
                dayMilestones++;
                myText = "Should probably wrap up soon...";
                while (!myMessage.SetText(myText, myDuration, myDelay, myColor))
                {
                    yield return new WaitForSeconds(6);
                }
            }

            if (triggerLapse && blurObj != null) {
                StartCoroutine(BlurLapse());
                triggerLapse = false;
            }
        }

        myText = "I'm gonna pass ou-...";
        myColor = Color.red;
        while (!myMessage.SetText(myText, myDuration, myDelay, myColor))
        {
            yield return new WaitForSeconds(6);
        }
        Globals.attitude -= 5f;

        yield return new WaitForSeconds(2);

        StopCoroutine(BlurLapse());
        DayEnd();
    }

    IEnumerator BlurLapse() {
        float currentTime = dailyTimeLimit * 0.7f;
        float endTime = dailyTimeLimit;

        bool goingUp = true;
        float spectrum = 0f;
        float blurLevel = 0f;
        float blurMulti = 1f;

        float waitTime = (endTime - currentTime) / 4f * -1;
        float waitInc = (waitTime / 10) * -1;
        float blurTime = 0.8f;

        while (true) {
            if (goingUp) {
                blurObj.SetFloat("_Size", blurLevel);
                blurLevel += (Time.deltaTime * blurMulti);

                spectrum += Time.deltaTime;

                if (spectrum >= blurTime) {
                    goingUp = false;
                }
            } else {
                if (spectrum >= 0f) {
                    blurObj.SetFloat("_Size", blurLevel);
                    blurLevel -= (Time.deltaTime * Mathf.Max(blurMulti / 1.2f, 1f));
                }

                spectrum -= Time.deltaTime;

                if (spectrum <= waitTime) {
                    spectrum = 0f;
                    goingUp = true;
                    blurMulti += (3f / (endTime - currentTime)); 
                    waitTime += waitInc;    
                    blurTime += 0.2f;
                }
            }

            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}
