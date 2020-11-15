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
        if (currentDay == 7) {
            currentDay = 10;
        } else if (currentDay == 8) {
            currentDay = 13;
        } else if (currentDay == 9) {
            currentDay = 14;
        } else if (currentDay == 10) {
            currentDay = 15;
        } else if (currentDay == 11) {
            currentDay = 20;
        } else if (currentDay == 12) {
            currentDay = 30;
        }

        for (int i = 0; i < 30; i++) {
            if (i < (currentDay - 1)) {
                tickedDays[i].SetActive(true);
            } else {
                tickedDays[i].SetActive(false);
            }
        }
    }
}
