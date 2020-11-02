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

            if (Globals.prevAction == "stream")
            {
                if (i.name == "time")
                {
                    float ingameMinutes = ((float)timeTag / 60) * 24;
                    if (ingameMinutes > 60) {
                        int finalHour = (int)Mathf.Floor(ingameMinutes / 60);
                        int finalMinute = (int)(ingameMinutes - (finalHour * 60));
                        i.GetComponent<Text>().text = "I streamed for " + finalHour + "h " + finalMinute + "m today.";
                    } else {
                        i.GetComponent<Text>().text = "I streamed for " + (int)((float)timeTag / 60 * 24) + " minutes today.";
                    }
                }
                else if (i.name == "audience")
                {
                    i.GetComponent<Text>().text = viewTag + " people watched me.";
                }
                else if (i.name == "money")
                {
                    i.GetComponent<Text>().text = "I got " + moneyTag + " in donations.";
                }
            }
            else
            {
                if (i.name == "time")
                {
                    i.GetComponent<Text>().text = "I didn't stream today.";
                }
                else if (i.name == "audience")
                {
                    i.GetComponent<Text>().text = "Just decided to take a break.";
                }
                else if (i.name == "money")
                {
                    i.GetComponent<Text>().text = "I wonder what they think about me now...";
                }
            }

            if (i.name == "feelings")
            {
                i.GetComponent<Text>().text = "";

                if (popDelta > 5)
                {
                    i.GetComponent<Text>().text += "I think today went really well. ";
                }
                else if (popDelta > 2 && popDelta <= 5)
                {
                    i.GetComponent<Text>().text += "I think today didn't go too badly. ";
                }
                else if (popDelta < -1 && popDelta >= -5)
                {
                    i.GetComponent<Text>().text += "I didn't do too well today. ";
                }
                else if (popDelta < -5)
                {
                    i.GetComponent<Text>().text += "Today went really badly. ";
                }
                else
                {
                    i.GetComponent<Text>().text += "Nothing much happened. ";
                }

                if (attDelta > 15)
                {
                    if (popDelta < -1)
                    {
                        i.GetComponent<Text>().text += "Except somehow, I feel really good.";
                    }
                    else
                    {
                        i.GetComponent<Text>().text += "I feel really good.";
                    }
                }
                else if (attDelta > 5 && attDelta <= 15)
                {
                    if (popDelta < -1)
                    {
                        i.GetComponent<Text>().text += "I feel pretty good though.";
                    }
                    else
                    {
                        i.GetComponent<Text>().text += "I feel pretty good.";
                    }
                }
                else if (attDelta < -5 && attDelta >= -15)
                {
                    if (popDelta > 2)
                    {
                        i.GetComponent<Text>().text += "Except I don't feel too good.";
                    }
                    else
                    {
                        i.GetComponent<Text>().text += "I don't feel too good.";
                    }
                }
                else if (attDelta < -15)
                {
                    if (popDelta > 2)
                    {
                        i.GetComponent<Text>().text += "Why does it feel so bad then...";
                    }
                    else
                    {
                        i.GetComponent<Text>().text += "Hm...";
                    }
                }
            }
        }

        StartCoroutine(SlowShow());
    }

    IEnumerator SlowShow()
    {
        foreach (GameObject i in results)
        {
            i.SetActive(true);
            Text myText = i.GetComponent<Text>();

            Color endColor;
            if (myText.name == "ButtonText")
            {
                i.GetComponentInParent<Button>().interactable = true;
                endColor = new Color(1f, 0f, 0f, 1f);
            } else
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
