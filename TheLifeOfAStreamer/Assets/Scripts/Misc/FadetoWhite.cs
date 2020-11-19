using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadetoWhite : MonoBehaviour
{
    private GameObject myFader;
    // Start is called before the first frame update
    void Start()
    {
        myFader = GameObject.Find("LevelFader/Fader");
        myFader.GetComponent<Image>().color = Color.white;
        Globals.prevAction = "stream";

        myFader.GetComponent<Animator>().speed = 0.3f;
        myFader.GetComponent<DayHandler>().DayEnd();
    }
}
