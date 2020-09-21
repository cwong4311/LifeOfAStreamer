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
    private float highlightTimer = 0f;
    private bool clicked = false;
    public Material[] baseMat;
    public Material[] highlightMat;
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
        gameObject.GetComponent<MeshRenderer>().materials = baseMat;
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
        if (highlightTimer > 0) highlightTimer -= Time.deltaTime;
        if (highlightTimer <= 0) {
            gameObject.GetComponent<MeshRenderer>().materials = baseMat;
        } else {
            gameObject.GetComponent<MeshRenderer>().materials = highlightMat;
        }
    }

    public void TriggerHighlight() {
        highlightTimer = 0.1f;
    }

    public void ChangeViews(Transform destination) {
        playerMessage.GetComponent<TextHandler>().SetText(MyMessage);
    }

    public void InteractSelect() {
        if (clicked) return;
        if (Globals.hasStreamed) {
            playerMessage.GetComponent<TextHandler>().SetText("I've already started streaming. I can't leave halfway");
            return;
        }
        if (playerMessage.GetComponent<TextHandler>().SetText(MyMessage)) {
            clicked = true;
            StartCoroutine(WaitForDayEnd(2.5f, Random.Range(1f, 3f), Random.Range(-5f, -2f)));
        }
    }

    public void TextSelect() {
        playerMessage.GetComponent<TextHandler>().SetText(MyMessage);
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
