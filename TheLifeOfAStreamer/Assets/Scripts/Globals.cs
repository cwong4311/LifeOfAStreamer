using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
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

    public static bool GameExists()
    {
        return true;
    }

    public static void SaveGame()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        string myData = "";
        foreach (FieldInfo field in typeof(Globals).GetFields())
        {
            myData += (field.Name + ", " + field.GetValue(null) + "_");
        }

        Debug.Log(myData);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, myData);
        file.Close();
    }

    public static void LoadGame()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        string myData = (string)bf.Deserialize(file);
        file.Close();

        string[] parseLoad = myData.Split('_');

        foreach (string item in parseLoad)
        {
            string fieldName = item.Split(',')[0];
            string fieldValue = item.Split(',')[1];

            foreach (FieldInfo field in typeof(Globals).GetFields())
            {
                if (field.Name.Equals(fieldName))
                {
                    field.GetValue(null) = fieldValue;
                }
            }
        }
    }
}
