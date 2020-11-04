using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextHandler : MonoBehaviour
{
    [System.Serializable]
    private class textEntry {
        public string message;
        public float duration;
        public float delay;
        public Color color;
    }

    private float TextDuration = 0f;
    private float fadeTime = 1f;
    private bool hasText = false;
    private bool running = false;
    private LinkedList<textEntry> mybacklog = new LinkedList<textEntry>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hasText) {
            if (TextDuration > 0f) {
                TextDuration -= Time.deltaTime;
            } else {
                StartCoroutine(FadeOut());
                hasText = false;
            }
        } else {
            if (mybacklog.Count > 0 && !running) {
                textEntry myMessage = mybacklog.First.Value;
                mybacklog.RemoveFirst();

                StartCoroutine(FadeIn(myMessage.message, myMessage.delay, myMessage.color));
                TextDuration = myMessage.duration;
            }
        }
    }
    public bool SetText(string message, float duration, float delay, Color myColor) {
        textEntry myMessage = new textEntry();
        myMessage.message = message; myMessage.duration = duration; myMessage.delay = delay; myMessage.color = myColor;

        mybacklog.AddLast(myMessage);
        return true;
    }

    public bool SetText(string message, float duration, float delay) {
        return SetText(message, duration, delay, Color.white);
    }

    public bool SetText(string message, Color myColor) {
        return SetText(message, 3, 0, myColor);
    }

    public bool SetText(string message) {
        return SetText(message, 3, 0, Color.white);
    }

    private IEnumerator FadeIn(string message, float delay, Color toColor)
    { 
        running = true;

        yield return new WaitForSeconds(delay);

        hasText = true;

        Text text = gameObject.GetComponent<Text>();
        text.text = message;
        text.color = Color.clear;

        for (float t = 0.01f; t < fadeTime; t += Time.deltaTime)
        {
            text.color = Color.Lerp(Color.clear, toColor, Mathf.Min(1, t/fadeTime));
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    { 
        Text text = gameObject.GetComponent<Text>();
        Color originalColor = text.color;
        for (float t = 0.01f; t < fadeTime; t += Time.deltaTime)
        {
            text.color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t/fadeTime));
            yield return null;
        }

        text.text = "";

        running = false;
    }


}
