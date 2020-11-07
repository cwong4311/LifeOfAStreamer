using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomNumber : MonoBehaviour
{
    public enum trackType {
        None,
        Subs
    }
    public GameObject myTextObject;
    public trackType trackedValue;

    // Start is called before the first frame update
    void Start()
    {
        displayRandomNumber();
    }

    void onEnable() 
    {
        displayRandomNumber();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void displayRandomNumber() {
        switch(trackedValue) {
            case trackType.Subs:
                myTextObject.GetComponent<Text>().text = "" + Random.Range(0, Globals.subNumber + 1);
                break;
            default:
            case trackType.None:
                break;
        }
    }
}
