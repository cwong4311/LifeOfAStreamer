using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkHandler : MonoBehaviour
{
    public Material drunkMat;
    // Start is called before the first frame update
    void Awake()
    {
        GameObject.Find("FPSController/FirstPersonCharacter").AddComponent<Drunk>().material = drunkMat;
    }
}
