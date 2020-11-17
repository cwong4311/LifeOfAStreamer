using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GIFPlayer : MonoBehaviour
{
    public Sprite[] frames;
    public int fps = 10;
    private float index;

    // Update is called once per frame
    void Update()
    {
        index += Time.deltaTime * fps;
        if (index > frames.Length - 1) {
            index = 0;
        }

        GetComponent<Image>().sprite = frames[(int)index];
    }
}
