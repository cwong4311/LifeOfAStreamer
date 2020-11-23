using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsText : MonoBehaviour
{
    public GameObject[] results;

    public GameObject CameraPan;

    public int timeTag = 0;
    public string viewTag = "0";
    public string moneyTag = "$0";

    public float popDelta = 0f;
    public float attDelta = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        CameraPan.GetComponent<CameraPan>().ToggleController(false);
    }

    public void RunScript()
    {
        foreach (GameObject i in results)
        {
            i.SetActive(false);
            if (i.name == "ButtonText")
            {
                i.GetComponentInParent<Button>().interactable = false;
            }

            string myMsg = "";

            // time
            // audience
            // money
            // feelings

            switch(Globals.days - 1) {
                case 1:
                    if (Globals.prevAction == "stream")
                    {
                        switch(i.name) {
                            case "time":
                                myMsg = "No-one, as expected";
                                break;
                            case "audience":
                                myMsg = "Not sure what I was hoping for";
                                break;
                            case "money":
                                myMsg = "I streamed for about " + getTime();
                                break;
                            case "feelings":
                                myMsg = "Hopefully tomorrow goes better";
                                break;
                        }
                    }
                    else
                    {
                        switch(i.name) {
                            case "time":
                                myMsg = "I skipped my first day";
                                break;
                            case "audience":
                                myMsg = "Maybe I was feeling a bit anxious";
                                break;
                            case "money":
                                myMsg = "Getting judged on screen";
                                break;
                            case "feelings":
                                myMsg = "Hopefully I'll be more ready tomorrow";
                                break;
                        }
                    }
                    break;
                case 2:
                    if (Globals.prevAction == "stream")
                    {
                        switch(i.name) {
                            case "time":
                                myMsg = "No-one again today";
                                break;
                            case "audience":
                                myMsg = "This is harder than I thought";
                                break;
                            case "money":
                                myMsg = "I need to try something else";
                                break;
                            case "feelings":
                                myMsg = "Otherwise my channel won't grow..";
                                break;
                        }
                    }
                    else
                    {
                        switch(i.name) {
                            case "time":
                                myMsg = "Skipped today";
                                break;
                            case "audience":
                                myMsg = "Not sure why";
                                break;
                            case "money":
                                myMsg = "Maybe I wasn't feeling it";
                                break;
                            case "feelings":
                                myMsg = "Hopefully I'll actually get to it tomorrow";
                                break;
                        }
                    }
                    break;
                case 3:
                    if (Globals.prevAction == "stream")
                    {
                        switch(i.name) {
                            case "time":
                                myMsg = "I finally got someone today!";
                                break;
                            case "audience":
                                myMsg = "He seemed a pretty nice guy";
                                break;
                            case "money":
                                myMsg = "Even subbed to my channel!";
                                break;
                            case "feelings":
                                myMsg = "I feel like tomorrow's gonna be great";
                                break;
                        }
                    }
                    else
                    {
                        switch(i.name) {
                            case "time":
                                myMsg = "Skipped today";
                                break;
                            case "audience":
                                myMsg = "Not sure why";
                                break;
                            case "money":
                                myMsg = "Maybe I wasn't feeling it";
                                break;
                            case "feelings":
                                myMsg = "Hopefully I'll actually get to it tomorrow";
                                break;
                        }
                    }
                    break;
                case 4:
                    if (Globals.prevAction == "stream")
                    {
                        switch(i.name) {
                            case "time":
                                myMsg = "Back to no-one";
                                break;
                            case "audience":
                                myMsg = "I even tried for " + getTime();
                                break;
                            case "money":
                                myMsg = "Guess I was just lucky huh?";
                                break;
                            case "feelings":
                                myMsg = "That kinda blows...";
                                break;
                        }
                    }
                    else
                    {
                        switch(i.name) {
                            case "time":
                                myMsg = "Skipped today";
                                break;
                            case "audience":
                                myMsg = "Maybe I wasn't feeling it";
                                break;
                            case "money":
                                myMsg = "I wasn't gonna get anyone anyway";
                                break;
                            case "feelings":
                                myMsg = "Nothing lost... Hopefully";
                                break;
                        }
                    }
                    break;
                case 5:
                    if (Globals.prevAction == "stream")
                    {
                        switch(i.name) {
                            case "time":
                                myMsg = "Some people dropped by today!";
                                break;
                            case "audience":
                                myMsg = "They were pretty cool guys";
                                break;
                            case "money":
                                myMsg = "There weren't many, but that's ok";
                                break;
                            case "feelings":
                                myMsg = "As long as I'm getting people";
                                break;
                        }
                    }
                    else
                    {
                        switch(i.name) {
                            case "time":
                                myMsg = "Skipped today";
                                break;
                            case "audience":
                                myMsg = "Maybe I wasn't feeling it";
                                break;
                            case "money":
                                myMsg = "I wasn't gonna get anyone anyway";
                                break;
                            case "feelings":
                                myMsg = "Nothing lost... Hopefully";
                                break;
                        }
                    }
                    break;
                case 6:
                    if (Globals.prevAction == "stream")
                    {
                        switch(i.name) {
                            case "time":
                                myMsg = "It's happening!";
                                break;
                            case "audience":
                                myMsg = "I'm getting viewers AND subs";
                                break;
                            case "money":
                                myMsg = "Things are gonna pick up now";
                                break;
                            case "feelings":
                                myMsg = "I can just feel it";
                                break;
                        }
                    }
                    else
                    {
                        switch(i.name) {
                            case "time":
                                myMsg = "Skipped today";
                                break;
                            case "audience":
                                myMsg = "Maybe I wasn't feeling it";
                                break;
                            case "money":
                                myMsg = "Wonder if I would've gotten any viewers";
                                break;
                            case "feelings":
                                myMsg = "Nothing lost... Hopefully";
                                break;
                        }
                    }
                    break;
                case 7:
                    if (Globals.prevAction == "stream")
                    {
                        switch(i.name) {
                            case "time":
                                myMsg = "Today went well";
                                break;
                            case "audience":
                                myMsg = "Got more subs and more viewers";
                                break;
                            case "money":
                                myMsg = "My channel's finally growing";
                                break;
                            case "feelings":
                                myMsg = "Hope things go just as well tomorrow too!";
                                break;
                        }
                    }
                    else
                    {
                        switch(i.name) {
                            case "time":
                                myMsg = "Skipped today";
                                break;
                            case "audience":
                                myMsg = "Maybe I wasn't feeling it";
                                break;
                            case "money":
                                myMsg = "Wonder if I would've gotten any viewers";
                                break;
                            case "feelings":
                                myMsg = "Nothing lost... Hopefully";
                                break;
                        }
                    }
                    break;
                case 8:
                    if (Globals.prevAction == "stream")
                    {
                        switch(i.name) {
                            case "time":
                                myMsg = "Who even was that?";
                                break;
                            case "audience":
                                myMsg = "Just said whatever he liked";
                                break;
                            case "money":
                                myMsg = "Is that what they call trolling?";
                                break;
                            case "feelings":
                                myMsg = "Hope he doesn't come back";
                                break;
                        }
                    }
                    else
                    {
                        switch(i.name) {
                            case "time":
                                myMsg = "I took a break today";
                                break;
                            case "audience":
                                myMsg = "Social media flooded by bad comments";
                                break;
                            case "money":
                                myMsg = "I don't know why this is happening";
                                break;
                            case "feelings":
                                myMsg = "But I feel sick";
                                break;
                        }
                    }
                    break;
                case 9:
                    if (Globals.prevAction == "stream")
                    {
                        switch(i.name) {
                            case "time":
                                myMsg = "Why is this happening...";
                                break;
                            case "audience":
                                myMsg = "What did I do wrong";
                                break;
                            case "money":
                                myMsg = "I can't believe it";
                                break;
                            case "feelings":
                                myMsg = "I'm so mad";
                                break;
                        }
                    }
                    else
                    {
                        switch(i.name) {
                            case "time":
                                myMsg = "I skipped streaming today";
                                break;
                            case "audience":
                                myMsg = "My social media's getting pretty bad";
                                break;
                            case "money":
                                myMsg = "Why is this happening?";
                                break;
                            case "feelings":
                                myMsg = "I can't believe it";
                                break;
                        }
                    }
                    break;
                case 10:
                    if (Globals.prevAction == "stream")
                    {
                        switch(i.name) {
                            case "time":
                                myMsg = "";
                                break;
                            case "audience":
                                myMsg = "";
                                break;
                            case "money":
                                myMsg = "...";
                                break;
                            case "feelings":
                                myMsg = "";
                                break;
                            case "ButtonText":
                                myMsg = "No more";
                                break;
                        }
                    }
                    else
                    {
                        switch(i.name) {
                            case "time":
                                myMsg = "";
                                break;
                            case "audience":
                                myMsg = "";
                                break;
                            case "money":
                                myMsg = "...";
                                break;
                            case "feelings":
                                myMsg = "";
                                break;
                            case "ButtonText":
                                myMsg = "No more";
                                break;
                        }
                    }
                    break;
                case 11:

                    if (Globals.prevAction == "stream")
                    {
                        switch(i.name) {
                            case "time":
                                myMsg = "\"Streaming isn't worth it\"";
                                break;
                            case "audience":
                                myMsg = "I wanted to stop";
                                break;
                            case "money":
                                myMsg = "I felt sick";
                                break;
                            case "feelings":
                                myMsg = "But that's not the case anymore";
                                break;
                            case "ButtonText":
                                myMsg = "I want to stream";
                                break;
                        }
                    }
                    else
                    {
                        switch(i.name) {
                            case "time":
                                myMsg = "BAD Ending";
                                break;
                            case "audience":
                                myMsg = "";
                                break;
                            case "money":
                                myMsg = "";
                                break;
                            case "feelings":
                                myMsg = "";
                                break;
                        }
                    }
                    break;
                case 12:
                    if (Globals.prevAction == "stream")
                    {
                        switch(i.name) {
                            case "time":
                                myMsg = "GOOD Ending";
                                break;
                            case "audience":
                                myMsg = "";
                                break;
                            case "money":
                                myMsg = "";
                                break;
                            case "feelings":
                                myMsg = "";
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }
            
            i.GetComponent<Text>().text = myMsg;
            if (myMsg == "" && i.name == "ButtonText") i.GetComponent<Text>().text = "Only tomorrow will tell";
        }

        StartCoroutine(SlowShow());
    }

    IEnumerator SlowShow()
    {
        foreach (GameObject i in results)
        {
            i.SetActive(true);
            Text myText = i.GetComponent<Text>();
            Image myButton = null; //Specifically for buttonShow

            Color endColor;
            Color endButtonColor; //Specifically for buttonShow
            if (myText.name == "ButtonText")
            {
                i.GetComponentInParent<Button>().interactable = true;
                endColor = new Color(1f, 0f, 0f, 1f);
                myButton = i.GetComponentInParent<Image>();
                endButtonColor = new Color(0.15f, 0.15f, 0.15f, 0.5f);
            } else
            {
                endColor = new Color(1f, 1f, 1f, 1f);
                endButtonColor = new Color(1f, 1f, 1f, 1f);
            }

            float t = 0;
            while (t < 1)
            {
                // Now the loop will execute on every end of frame until the condition is true
                myText.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 0.0f), endColor, t);
                if (myButton != null) myButton.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 0.0f), endButtonColor, t);
                t += Time.deltaTime / 2;
                yield return new WaitForEndOfFrame(); // So that I return something at least.
            }

            yield return new WaitForSeconds(1f);
        }

        Time.timeScale = 0f;
    }

    private string getTime() {
        float ingameMinutes = ((float)timeTag / 60) * 24;
        if (ingameMinutes > 60) {
            int finalHour = (int)Mathf.Floor(ingameMinutes / 60);
            int finalMinute = (int)(ingameMinutes - (finalHour * 60));
            return ("" + finalHour + "h " + finalMinute + "m today.");
        } else {
            return ("" + (int)((float)timeTag / 60 * 24) + " minutes today.");
        }
    }
}
