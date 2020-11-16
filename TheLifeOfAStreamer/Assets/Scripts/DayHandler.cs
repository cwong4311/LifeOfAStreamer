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

    public GameObject GoodEnd;
    public GameObject BadEnd;

    private Material blurObj;

    private readonly bool continuousPlay = true;

    private float dailyTimeLimit = 900f; //Time limit of each session, in Seconds.

    private bool leaveGame = false;

    private float dayPopularity = 0f;
    private float dayAttitude = 0f;
    
    private float timer = 0f;
    private float streamTimer = 0f;

    private int totalDays = 30;

    // Start is called before the first frame update
    void Start()
    {
        OnStartFX();
        StartCoroutine(runDailyQuote());

        if (Globals.days >= 15) {dailyTimeLimit = 1800f;}   //Streamer can stream twice as long after they get used to it
        StartCoroutine(DayLimitHandler());

        if (myBlur != null) {
            blurObj = Instantiate(myBlur.GetComponent<Image>().material);
            myBlur.GetComponent<Image>().material = blurObj;
        }

        // Cap framerate to 60
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        // If streamTimer hasn't been set, and gameFlag is no longer -1 (ie Game has started)
        if (streamTimer == 0f && Globals.hasStreamed) {
            streamTimer = timer;
        }
    }

    public void DayEnd(float attitude, float popularity) {
        viewerSystem.GetComponent<ViewerControlSystem>().endDay();

        // If I say I'm streaming, but I don't stream, lose popularity
        if (Globals.hasPosted && !Globals.hasStreamed) {
            popularity -= (0.2f * Globals.subNumber);
        } else if (Globals.hasPosted && Globals.hasStreamed) {
            popularity += (0.1f * Globals.subNumber);
        }
        // If today is a day I've booked for streaming, but I don't stream, lose popularity.
        if (Globals.reserveDays.Contains(Globals.days + "") && !Globals.hasStreamed) {
            popularity -= (0.5f * Globals.subNumber);
        } else if (Globals.reserveDays.Contains(Globals.days + "") && Globals.hasStreamed) {
            popularity += (0.2f * Globals.subNumber);
        }

        Globals.days += 1;
        Globals.prevViewer = Globals.dayViewer;
        Globals.totalViewer += Globals.dayViewer;
        Globals.hasStreamed = false;
        Globals.hasPosted = false;
		
        ScriptedIncrement();
        Globals.attitude += attitude;
        Globals.popularity += popularity;

        Globals.dayAttitude = 0;
        Globals.gameScore = 0;
        Globals.dayViewer = 0;
        Globals.totalMoney += Globals.dayMoney;

        Globals.gameFlag = -1;
        FadeOut();
    }

    public void DayEnd() {
        dayAttitude = Random.Range(-2f, 2f) + Globals.dayAttitude;   // TO DO: Better Formula
        dayPopularity = Random.Range(-3f, 3f) + (Globals.gameScore / 300f) 
                                + ((float) (Globals.dayViewer - Globals.prevViewer) / 10f)
                                + ((float) (Mathf.Min(Globals.dayAttitude, 50f) / 10f))
                                + ((float) Globals.subNumber / 10f);

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
            GoodEnd.SetActive(true);
        // If all days finished (check value - 1)
        } else if ((Globals.days - 1) == 12) {
            GoodEnd.SetActive(true);
        // Of if on day 11, the player doesn't stream
        } else if ((Globals.days - 1) == 11 && Globals.prevAction != "stream") {
            BadEnd.SetActive(true);
        } else
        {
            myResultsScreen.SetActive(true);

            myResultsScreen.GetComponent<ResultsText>().timeTag = (int) (timer - streamTimer);
            myResultsScreen.GetComponent<ResultsText>().viewTag = "" + Globals.prevViewer;
            myResultsScreen.GetComponent<ResultsText>().moneyTag = "$" + Globals.dayMoney;
            Globals.dayMoney = 0;
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

    private void ScriptedIncrement() {
       switch(Globals.days) {
            case 2:
                Globals.popularity = 0;
                Globals.attitude = -10;
                break;
            case 3:
                Globals.popularity = 0;
                Globals.attitude = -35;
                break;
            case 4:
                Globals.popularity = 5;
                Globals.attitude = 10;
                break;
            case 5:
                Globals.popularity = 5;
                Globals.attitude = 5;
                break;
            case 6:
                Globals.popularity = 10;
                Globals.attitude = 10;
                break;    
            case 7:
                Globals.popularity = 20;
                Globals.attitude = 20;
                break;  
            case 8:
                Globals.popularity = 30;
                Globals.attitude = 30;
                break;  
            case 9:
                Globals.popularity = 40;
                Globals.attitude = -35;
                break;  
            case 10:
                Globals.popularity = 45;
                Globals.attitude = -60;
                break;  
            case 11:
                Globals.popularity = 10;
                Globals.attitude = -100;
                break;  
            case 12:
                Globals.popularity = 70;
                Globals.attitude = 30;
                break;
            default:
                break;
        }
   
    }

    IEnumerator runDailyQuote() {
        TextHandler myDay = GameObject.Find("DayMessage").GetComponent<TextHandler>();
        TextHandler myMessage = GameObject.Find("PlayerMessage").GetComponent<TextHandler>();
        float THRESHOLD = Globals.mentalThreshold;
        string myText = ""; float myDuration = 3f; float myDelay = 0.5f; Color myColor = Color.white;

        int dayNum = Globals.days;
        if (dayNum == 7) {
            dayNum = 10;
        } else if (dayNum == 8) {
            dayNum = 13;
        } else if (dayNum == 9) {
            dayNum = 14;
        } else if (dayNum == 10) {
            dayNum = 15;
        } else if (dayNum == 11) {
            dayNum = 20;
        } else if (dayNum == 12) {
            dayNum = 30;
        }
        myDay.SetText("Day " + dayNum, 1.7f, 0.5f, myColor);

        yield return new WaitForSeconds(2);

        switch(Globals.days) {
            case 1:
                Globals.popularity = 0;
                Globals.attitude = 0;

                myText = "Finally got my streaming equipment all set up.\nNow I can stream!";
                myDuration = 4f;

                StartCoroutine(TriggerBeginningMonologue(myMessage, "Doubt I'll get anyone\nBut let's give it a shot anyway", 3f));
                StartCoroutine(TriggerInternalMonologue(myMessage, "No viewers, as expected.\nMaybe it's time to call it...", 300f));
                break;
            case 2:
                Globals.popularity = 0;
                Globals.attitude = -5;

                if (Globals.prevAction == "stream") {
                    myText = "Alright, time to stream\nHope I get some people today";
                } else {
                    myText = "I ended up putting it off yesterday\nLet's start today, for sure";
                }

                StartCoroutine(TriggerInternalMonologue(myMessage, "Still no one.\nWonder when someone will pop in to chat...", 300f));
                StartCoroutine(TriggerInternalMonologue(myMessage, "Let's call it a day.\nHopefully tomorrow will go better.", 500f));
                break;
            case 3:
                Globals.popularity = 0;
                Globals.attitude = -10;

                if (Globals.prevAction == "stream") {
                    myText = "It's really hard to get viewers huh.\nLet's hope today's the day";
                } else {
                    myText = "Gotta start somewhere. I've bought all this stuff!";
                }
                break;
            case 4:
                Globals.popularity = 5;
                Globals.attitude = 10;

                if (Globals.prevAction == "stream") {
                    myText = "Finally got someone yesterday!\n Let's keep this up";
                    myDuration = 4f;
                } else {
                    myText = "Taking breaks like this might feel good\n But I'll never get anywhere";
                    myDuration = 4f;  
                }

                StartCoroutine(TriggerInternalMonologue(myMessage, "No one's coming in today.\n Guess you can't always be lucky...", 300f));
                StartCoroutine(TriggerInternalMonologue(myMessage, "Maybe it's time to wrap for the day", 500f));
                break;
            case 5:
                Globals.popularity = 5;
                Globals.attitude = 5;

                if (Globals.prevAction == "stream") {
                    myText = "Guess I just got lucky the other day\n Streaming's hard...";
                    myDuration = 4f;
                } else {
                    myText = "Feels like I haven't streamed in a while\nStrange..";
                    myDuration = 4f;  
                }
                break;
            case 6:
                Globals.popularity = 10;
                Globals.attitude = 10;

                if (Globals.prevAction == "stream") {
                    myText = "That was an interesting pair yesterday\n Wonder if I'll get anyone today";
                    myDuration = 4f;
                } else {
                    myText = "That was a good break.\n Let's stream today";
                    myDuration = 4f;  
                }
                break;    
            case 7:
                Globals.popularity = 20;
                Globals.attitude = 20;

                if (Globals.prevAction == "stream") {
                    myText = "The last few days went really well!\n I think I'm getting the hang of streaming now";
                    myDuration = 4f;
                } else {
                    myText = "Haven't really streamed the last few days...\n Let's stream today!";
                    myDuration = 4f;  
                }
                break;  
            case 8:
                Globals.popularity = 30;
                Globals.attitude = 30;

                if (Globals.prevAction == "stream") {
                    myText = "Another day of streaming.\n Another day of meeting new people!";
                    myDuration = 4f;
                } else {
                    myText = "Haven't really streamed the last few days...\n Let's stream today";
                    myDuration = 4f;  
                }
                break;  
            case 9:
                Globals.popularity = 40;
                Globals.attitude = -20;

                if (Globals.prevAction == "stream") {
                    myText = "What was that yesterday?\n Hope he doesn't show up ever again";
                    myDuration = 4f;
                } else {
                    myText = "Had a good break yesterday\nI wonder if I missed anything?...";
                    myDuration = 4f;  
                }
                break;  
            case 10:
                Globals.popularity = 45;
                Globals.attitude = -40;

                if (Globals.prevAction == "stream") {
                    myText = "It's getting worse...\nWhat did I do to deserve this?";
                    myDuration = 4f;

                    StartCoroutine(TriggerBeginningMonologue(myMessage, "I'm not really that bad, am I?", 3f));
                    StartCoroutine(TriggerBeginningMonologue(myMessage, "Maybe I should just skip streaming today", 6f));
                    StartCoroutine(TriggerBeginningMonologue(myMessage, "I don't want to hear any more", 9f));
                } else {
                    myText = "Getting some text about trolls increasing in activity lately.\n Hopefully I won't run into any";
                    myDuration = 4f;  
                }
                break;  
            case 11:
                Globals.popularity = 50;
                Globals.attitude = -100;

                myText = "Why am I still here";
                myDuration = 4f;

                if (Globals.prevAction == "stream") {
                    StartCoroutine(TriggerBeginningMonologue(myMessage, "I'm sick of streaming", 5f));
                } else {
                    StartCoroutine(TriggerBeginningMonologue(myMessage, "I'm sick of trying to stream", 3f));
                    StartCoroutine(TriggerBeginningMonologue(myMessage, "Then getting bashed on social media", 3f));
                }
                
                StartCoroutine(TriggerBeginningMonologue(myMessage, "I don't want to deal with that again", 10f));
                StartCoroutine(TriggerBeginningMonologue(myMessage, "I'm only going to get flamed", 15f));
                StartCoroutine(TriggerBeginningMonologue(myMessage, "It's useless", 20f));
                StartCoroutine(TriggerBeginningMonologue(myMessage, "I'm done", 25f));
                StartCoroutine(TriggerBeginningMonologue(myMessage, "I feel sick", 30f));
                StartCoroutine(TriggerBeginningMonologue(myMessage, "Let's go back to sleep", 35f));
                StartCoroutine(TriggerBeginningMonologue(myMessage, "And forget this all happened", 40f));
                break;  
            case 12:
                Globals.popularity = 70;
                Globals.attitude = 300;

                myText = "It's been a while since I started streaming";
                myDuration = 4f;

                StartCoroutine(TriggerBeginningMonologue(myMessage, "Things didn't always go well", 3f));
                StartCoroutine(TriggerBeginningMonologue(myMessage, "But that's ok", 6f));
                StartCoroutine(TriggerBeginningMonologue(myMessage, "People are waiting for me today too", 9f));
                StartCoroutine(TriggerBeginningMonologue(myMessage, "Let's start the stream", 12f));
                break;
            default:
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
                break;
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

    IEnumerator TriggerInternalMonologue(TextHandler handler, string message, float delay) {
        float timer = 0f;
        float myDuration = 3f; float myDelay = 0.5f; Color myColor = Color.white;

        while (timer < delay) {
            timer ++;
            yield return new WaitForSeconds(1);
        }

        if (Globals.hasStreamed) handler.SetText(message, myDuration, myDelay, myColor);
    }

    IEnumerator TriggerBeginningMonologue(TextHandler handler, string message, float delay) {
        float timer = 0f;
        float myDuration = 3f; float myDelay = 0.5f; Color myColor = Color.white;

        while (timer < delay) {
            timer ++;
            yield return new WaitForSeconds(1);
        }

        handler.SetText(message, myDuration, myDelay, myColor);
    }

    void OnStartFX() {
        if (Globals.days == 11) {
            FX myFX = GameObject.Find("Effects").GetComponent<FX>();
            myFX.PlayEffects(3);
        }
    }
}
