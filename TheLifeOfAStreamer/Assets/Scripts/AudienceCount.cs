using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudienceCount : MonoBehaviour
{
    public enum myTrackObject {
        None,
        Audience,
        Subscribers,
        Donations
    }

    public myTrackObject TrackedValue;
    private GameObject trackChildValue;

    public bool leftAlign = false;
    private int prevLength = 1;

    void Start() 
    {
    }

    // Update is called once per frame
    void Update()
    {
        int trackedValue;
        float x_shift = 0f;

        switch(TrackedValue) {
            default:
            case myTrackObject.None:
                trackedValue = -1;
                break;
            case myTrackObject.Audience:
                trackChildValue = GameObject.Find("Audience");
                trackedValue = trackChildValue.transform.childCount;
                x_shift = 9f;
                GetComponent<Text>().text = "" + trackedValue;
                break;
            case myTrackObject.Subscribers:
                trackedValue = Globals.subNumber;
                x_shift = 9f;
                GetComponent<Text>().text = "" + trackedValue;
                break;
            case myTrackObject.Donations:
                trackedValue = Globals.dayMoney;
                x_shift = 11.4f;
                GetComponent<Text>().text = "" + trackedValue;
                break;
        }
        
        if (trackedValue < 0) return;

        int curLength = trackedValue.ToString().Length;

        if (leftAlign && prevLength != curLength) {
            if (prevLength < curLength) {
                for (int i = 0; i < (curLength - prevLength); i++) {
                    transform.position += new Vector3(-x_shift, 0f, 0f);
                }
            } else {
                for (int i = 0; i < (prevLength - curLength); i++) {
                    transform.position += new Vector3(x_shift, 0f, 0f);
                }
            }
            prevLength = curLength;
        }
    }
}
