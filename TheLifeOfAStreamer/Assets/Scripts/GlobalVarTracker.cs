using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVarTracker : MonoBehaviour
{
    public enum TrackVariable {
        None,
        dayViewer,            //AI messages
        popularity,
        attitude,    // Streamer (the real player)'s messages
        days        // Troll messages
    }
    public TrackVariable myValue = TrackVariable.None;
    public bool leftAlign = false;
    private int trackValue; 
    private int prevLength = 1;

    // Update is called once per frame
    void Update()
    {
        switch(myValue) {
            case TrackVariable.dayViewer:
                trackValue = (int) Globals.dayViewer;
                break;
            case TrackVariable.popularity:
                trackValue = (int) Globals.popularity;
                break;
            case TrackVariable.attitude:
                trackValue = (int) Globals.attitude;
                break;
            case TrackVariable.days:
                trackValue = (int) Globals.days;
                break;
            default:
                break;
        }

        GetComponent<TextMesh>().text = "" + trackValue;
        int curLength = trackValue.ToString().Length;

        if (leftAlign && prevLength != curLength) {
            if (prevLength < curLength) {
                for (int i = 0; i < (curLength - prevLength); i++) {
                    transform.position += new Vector3(-0.7f, 0f, 0f);
                }
            } else {
                for (int i = 0; i < (prevLength - curLength); i++) {
                    transform.position += new Vector3(0.7f, 0f, 0f);
                }
            }
            prevLength = curLength;
        }
    }
}
