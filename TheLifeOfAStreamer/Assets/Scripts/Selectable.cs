using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selectable : MonoBehaviour
{
    public enum ObjectType {
        None,
        Stream,
        Interact,
        Text
    }

    private bool clicked = false;
    public ObjectType objectType = ObjectType.None;
    public string[] Messages;
    private GameObject playerMessage;
    public GameObject myDayHandler;

    public bool attitudeModifier;

    private string MyMessage;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Selectable";
        playerMessage = GameObject.Find("/PlayerCanvas/PlayerMessage");
        
        if (Messages.Length > 0) {
            MyMessage = Messages[Random.Range(0, Messages.Length)].Replace("<br>", "\n");
        } else {
            MyMessage = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool InteractSelect() {
        if (clicked) return false;
        if (Globals.hasStreamed) {
            playerMessage.GetComponent<TextHandler>().SetTextPriority("I've already started streaming. I can't leave halfway");
            return false;
        } else if (Globals.days == 12) {
            playerMessage.GetComponent<TextHandler>().SetTextPriority("I promised to stream today. I can't miss it.");
            return false;
        } else if (Globals.days == 11 && this.name == "Door") {
            playerMessage.GetComponent<TextHandler>().SetTextPriority("I can't go out... They'll find me");
            return false;
        }
        if (playerMessage.GetComponent<TextHandler>().SetTextPriority(MyMessage)) {
            clicked = true;
            StartCoroutine(WaitForDayEnd(2.5f, Random.Range(1f, 3f), Random.Range(-5f, -2f)));
            return true;
        }
        return false;
    }

    public void TextSelect() {
        playerMessage.GetComponent<TextHandler>().SetTextPriority(MyMessage);
    }

    private IEnumerator WaitForDayEnd(float delay, float attitude, float popularity) {
        var t = 0f;
        while(t < 1)
        {
             t += Time.deltaTime / delay;
             yield return null;
        }

        float finalAttitude = attitude;
        if (!attitudeModifier) finalAttitude = attitude * -1;

        Globals.prevAction = "misc";
        myDayHandler.GetComponent<DayHandler>().DayEnd(finalAttitude, popularity);
    }
}
