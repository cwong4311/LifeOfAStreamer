using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndChildHandler : MonoBehaviour
{
    public enum ScreenType
    {
        Intro,
        Stats1,
        Stats2,
        Eval,
        Message,
        Final
    }

    public ScreenType myType = ScreenType.Intro;

    public GameObject[] items;

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
        SetMyText();

        foreach (GameObject i in items)
        {
            i.SetActive(false);
        }

        StartCoroutine(SlowShow());
    }

    private void SetMyText()
    {
        switch(myType)
        {
            case ScreenType.Intro:
                items[0].GetComponent<Text>().text = "It's been " + (Globals.days - 1) + " days\nsince you started streaming.";
                break;
            case ScreenType.Stats1:
                if (Globals.popularity > 60)
                {
                    items[0].GetComponent<Text>().text = "A lot of people loved your streams,";
                }
                else if (Globals.popularity > 30)
                {
                    items[0].GetComponent<Text>().text = "You became decently popular,";
                }
                else if (Globals.popularity > 10)
                {
                    items[0].GetComponent<Text>().text = "You became somewhat popular,";
                }
                else if (Globals.popularity < -10)
                {
                    items[0].GetComponent<Text>().text = "You were not popular at all,";
                }
                else if (Globals.popularity < -30)
                {
                    items[0].GetComponent<Text>().text = "Noone really cared about you,";
                }
                else if (Globals.popularity < -60)
                {
                    items[0].GetComponent<Text>().text = "You wasted your time,";
                }
                else
                {
                    items[0].GetComponent<Text>().text = "You didn't really go anywhere,";
                }
                items[0].GetComponent<Text>().text += "\nwith a total of " + Globals.totalViewer + " viewers.";
                items[1].GetComponent<Text>().text = "You made a total of $" + Globals.totalMoney + ".";
                break;
            case ScreenType.Stats2:
                items[0].GetComponent<Text>().text = "But were you happy doing it?";
                if (Globals.attitude > 60)
                {
                    items[1].GetComponent<Text>().text = "Absolutely.\nIn fact, you loved doing it.";
                }
                else if (Globals.attitude > 30)
                {
                    items[1].GetComponent<Text>().text = "You would say you were.\nYou thought it was a good time.";
                }
                else if (Globals.attitude > 10)
                {
                    items[1].GetComponent<Text>().text = "You would say you were.\nIt was a decent experience.";
                }
                else if (Globals.attitude < -10)
                {
                    items[1].GetComponent<Text>().text = "You wouldn't say you were.\nYou didn't really have a good time.";
                }
                else if (Globals.attitude < -30)
                {
                    items[1].GetComponent<Text>().text = "You wouldn't say you were.\nIn fact, you hated it.";
                }
                else if (Globals.attitude < -60)
                {
                    items[1].GetComponent<Text>().text = "Absolutely not.\nYou would've rather done literally\nanything else.";
                }
                else
                {
                    items[1].GetComponent<Text>().text = "You couldn't really say.\nIt was neither fun nor too painful.";
                }
                
                break;
            case ScreenType.Eval:
                if (Globals.attitude > 30)
                {
                    items[0].GetComponent<Text>().text = "Since you had fun, chances are\nyou would continue streaming\nregardless of your success.";
                    items[1].GetComponent<Text>().text = "Even though getting trolled or flamed is tough,\nyou think you can handle it.";
                }
                else if (Globals.attitude > 10)
                {
                    if (Globals.popularity > 30)
                    {
                        items[0].GetComponent<Text>().text = "Streaming wasn't bad,\nand you had a decent audience.\n You'd probably continue streaming.";
                        items[1].GetComponent<Text>().text = "Even though getting trolled or flamed is tough,\nyou think you can handle it.";
                    }
                    else if (Globals.popularity > 10)
                    {
                        items[0].GetComponent<Text>().text = "Streaming wasn't bad,\nbut you didn't make it big.\nWhether or not you continue streaming\nis hard to say.";
                        items[1].GetComponent<Text>().text = "Getting Trolled and flamed is tough";
                    }
                    else
                    {
                        items[0].GetComponent<Text>().text = "Streaming wasn't bad,\nbut you didn't get anywhere.\nChanges are you won't continue streaming.";
                        items[1].GetComponent<Text>().text = "Getting Trolled and flamed is tough";
                    }
                }
                else if (Globals.attitude < -10)
                {
                    if (Globals.popularity > 30)
                    {
                        items[0].GetComponent<Text>().text = "You had a bad time streaming,\nbut since you made it big,\nyou'd most likely continue.";
                        items[1].GetComponent<Text>().text = "Getting trolled and flamed is tough,\nbut you're successful do you can deal with it.";
                    }
                    else
                    {
                        items[0].GetComponent<Text>().text = "You had a bad time streaming,\n and you didn't get anywhere either.\nChances are you won't continue streaming.";
                        items[1].GetComponent<Text>().text = "Getting Trolled and flamed is tough.";
                    }
                }
                else if (Globals.attitude < -30)
                {
                    if (Globals.popularity > 60)
                    {
                        items[0].GetComponent<Text>().text = "Streaming was painful for you,\nbut since you made it big,\nthere's a good chance you'll continue";
                        items[1].GetComponent<Text>().text = "Trolling and flaming is really\ntaking its toll on you.";
                    }
                    else
                    {
                        items[0].GetComponent<Text>().text = "Streaming was excruciating for you.\nYou're done.";
                        items[1].GetComponent<Text>().text = "Trolling and flamed took\na massive toll on you.";
                    }
                }
                else
                {
                    items[0].GetComponent<Text>().text = "You didn't really get anywhere\nor feel anything streaming.\nYou might or might not continue streaming.";
                    items[1].GetComponent<Text>().text = "It's hard to say.\nGetting trolled and flamed is tough.";
                }
                break;
            case ScreenType.Message:
                items[0].GetComponent<Text>().text = "Only a handful of streamers ever\nsucceed, and most give up without\ngetting anywhere.";
                items[1].GetComponent<Text>().text = "Next time you comment on a stream,\nmake sure you know what you're saying.";
                break;
            default:
            case ScreenType.Final:
                items[0].GetComponent<Text>().text = "Are you happy with this result?";
                break;
        }
    }

    IEnumerator SlowShow()
    {

        foreach (GameObject i in items)
        {
            i.SetActive(true);
            Text myText = i.GetComponent<Text>();

            Color endColor;
            if (myText.name == "ButtonText")
            {
                i.GetComponentInParent<Button>().interactable = true;
                endColor = new Color(1f, 0f, 0f, 1f);
            }
            else
            {
                endColor = new Color(1f, 1f, 1f, 1f);
            }

            float t = 0;
            while (t < 1)
            {
                // Now the loop will execute on every end of frame until the condition is true
                myText.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 0.0f), endColor, t);
                t += Time.deltaTime / 2;
                yield return new WaitForEndOfFrame(); // So that I return something at least.
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
