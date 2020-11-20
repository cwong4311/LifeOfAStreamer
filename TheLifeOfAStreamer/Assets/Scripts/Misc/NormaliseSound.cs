using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormaliseSound : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameObject.Find("Sound").GetComponent<SoundHandler>().ToggleDistortion(false);
        Destroy(this.gameObject);
    }
}
