using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calendar : MonoBehaviour
{
    public GameObject[] tickedDays;
    // Start is called before the first frame update
    void Start()
    {
        int currentDay = Globals.days;

        for (int i = 0; i < 30; i++) {
            if (i < (currentDay - 1)) {
                tickedDays[i].SetActive(true);
            } else {
                tickedDays[i].SetActive(false);
            }
        }
    }
}
