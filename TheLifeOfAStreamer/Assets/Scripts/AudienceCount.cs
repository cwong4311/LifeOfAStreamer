using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceCount : MonoBehaviour
{
    public GameObject trackChildValue;
    public bool leftAlign = false;
    private int prevLength = 1;

    void Start() 
    {
        trackChildValue = GameObject.Find("Audience");
    }

    // Update is called once per frame
    void Update()
    {
        int viewerCount = trackChildValue.transform.childCount;
        GetComponent<TextMesh>().text = "" + trackChildValue.transform.childCount;

        int curLength = viewerCount.ToString().Length;

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
