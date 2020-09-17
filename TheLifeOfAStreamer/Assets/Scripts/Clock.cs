using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    private int myTime = 0;
    private float ms = 0;
    private int TimeRate = 60;
    // Start is called before the first frame update
    void Start()
    {
        myTime = 43200;
    }

    // Update is called once per frame
    void Update()
    {
        ms += Time.deltaTime;
        if (ms >= 1f) {
            ms = 0;
            myTime += TimeRate;
        }
        if (myTime >= 86400) {
            myTime = 0;
        }

        UpdateClock();
    }

    void UpdateClock() {
        int displaySeconds = myTime % 60;
        int displayMinutes = (myTime / 60) % 60;
        int displayHours = (myTime / 3600) % 24;

        gameObject.GetComponent<TextMesh>().text = displayHours.ToString("00") + ":" + displayMinutes.ToString("00");
    }
}
