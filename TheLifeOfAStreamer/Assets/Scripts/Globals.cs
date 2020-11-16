using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class Globals
{
    public static int days = 11;
    public static float popularity = 0f;
    public static float attitude = 0f;
    public static int totalMoney = 0;
    public static int dayMoney = 0;
    public static int dayViewer = 0;
    public static int prevViewer = 0;
    public static int totalViewer = 0;
    public static float dayAttitude = 0f;

    public static string prevAction = "stream";

    public static float mentalThreshold = 2f;
    public static int gameFlag = 0;
    public static int gameType = 0;
    public static bool hasStreamed = false;
    public static float gameScore = 0f;

    public static bool hasPosted = false;
    public static string reserveDays = "";

    public static int subNumber = 0;
    public static string subNames = "";

    public static string username = "Player1024";
    public static bool webcamEnabled = false;
    public static int platformSetting = 0;

    public static bool GameExists()
    {
        string destination = Application.dataPath + "/Save/save.dat";
        if (!Directory.Exists(Application.dataPath + "/Save")) return false;

        if (File.Exists(destination)) return true;
        else return false;
    }

    public static void SaveGame()
    {
        dayMoney = 0;

        string destination = Application.dataPath + "/Save/save.dat";
        if (!Directory.Exists(Application.dataPath + "/Save")) 
            {
                Directory.CreateDirectory(Application.dataPath + "/Save");
            }

        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        string myData = "";
        foreach (FieldInfo field in typeof(Globals).GetFields())
        {
            myData += (field.Name + ", " + field.GetValue(null) + "_");
        }

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, myData);
        file.Close();
    }

    public static bool LoadGame()
    {
        string destination = Application.dataPath + "/Save/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found");
            return false;
        }

        BinaryFormatter bf = new BinaryFormatter();
        string myData = (string)bf.Deserialize(file);
        file.Close();

        string[] parseLoad = myData.Split('_');

        foreach (string item in parseLoad)
        {
            if (item == "") continue;
            string fieldName = item.Split(',')[0].Trim();
            string fieldValue = item.Split(',')[1].Trim();

            foreach (FieldInfo field in typeof(Globals).GetFields())
            {
                if (field.Name.Equals(fieldName))
                {
                    if (field.GetValue(null) is int) {
                        field.SetValue(null, int.Parse(fieldValue));
                    }
                    else if (field.GetValue(null) is string)
                    {
                        field.SetValue(null, fieldValue);
                    }
                    else if (field.GetValue(null) is bool)
                    {
                        field.SetValue(null, bool.Parse(fieldValue));
                    }
                    else if (field.GetValue(null) is float)
                    {
                        field.SetValue(null, float.Parse(fieldValue));
                    }
                    break;
                }
            }
        }

        return true;
    }

    public static void DeleteGame()
    {
        string destination = Application.dataPath + "/Save/save.dat";

        if (File.Exists(destination)) {
            File.Delete(destination);
            
            days = 1;
            popularity = 0f;
            attitude = 0f;
            totalMoney = 0;
            dayMoney = 0;
            dayViewer = 0;
            prevViewer = 0;
            totalViewer = 0;
            dayAttitude = 0f;

            prevAction = "stream";

            mentalThreshold = 2f;
            gameFlag = 0;
            gameType = 0;
            hasStreamed = false;
            gameScore = 0f;

            hasPosted = false;
            reserveDays = "";

            subNumber = 0;
            subNames = "";
            
            webcamEnabled = false;
        }
    }
}
