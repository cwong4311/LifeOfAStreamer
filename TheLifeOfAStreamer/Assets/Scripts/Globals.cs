using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    public static int days = 1;
    public static float popularity = 0f;
    public static float attitude = 0f;
    public static int dayViewer = 0;
    public static int prevViewer = 0;
    public static float dayAttitude = 0f;

    public static string prevAction = "stream";

    public static float mentalThreshold = 2f;
    public static int gameFlag = 0;
    public static int gameType = 0;
    public static bool hasStreamed = false;
    public static float gameScore = 0f;

    public static int returningViewers = 0;

    public static string username = "Player";
    public static bool webcamEnabled = false;

    public static int LoadScene = -1;
}
