using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Ink.Runtime;

public class ScriptedViewer : Viewer
{
	// Set this file to your compiled json asset
    public GameObject promptCanvas;
    public GameObject[] buttons;
	// The ink story that we're wrapping
	Story _inkStory;

    private string[] viewerNames;
    private Color[] viewerColors;
    private ArrayList dummySet;

    private bool choiceDisplayed = false;
    private bool storyLoaded = false;

    private float scriptDelay = 5f;     //Default - 15f, Debug - 5f
    private float scriptCounter = 0f;

    private GameObject chatboxDisplay;

    void Awake()
    {
        dummySet = new ArrayList();
        scriptCounter = UnityEngine.Random.Range(scriptDelay / 2, scriptDelay);
        ToggleAllOptions(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!storyLoaded) return;

        if (!chatboxDisplay.activeInHierarchy || !choiceDisplayed) {
            if (promptCanvas.activeSelf) ToggleCanvas(false);
        } else {
            if (!promptCanvas.activeSelf) ToggleCanvas(true);
        }

        if (scriptCounter > 0) scriptCounter -= Time.deltaTime;

        while (_inkStory.canContinue && scriptCounter <= 0) {
            scriptCounter = UnityEngine.Random.Range(scriptDelay / 3f, scriptDelay);

            string myMessage = Sanitise(_inkStory.Continue());
            if (myMessage != "") {
                sendMyRawMessage(username, myMessage);
            }
        }

        if ( _inkStory.currentChoices.Count > 0 && !choiceDisplayed) 
        {
            choiceDisplayed = true;

            for (int i = 0; i < _inkStory.currentChoices.Count; ++i) {
                Choice choice = _inkStory.currentChoices [i];
                buttons[i].SetActive(true);
                buttons[i].GetComponentInChildren<Text>().text = choice.text;
            }
        }
    }

    public void SelectOption(int index) {
        choiceDisplayed = false;
        _inkStory.ChooseChoiceIndex (index);
        _inkStory.Continue(); //Swallow the response

        ToggleAllOptions(false);
        scriptCounter = UnityEngine.Random.Range(scriptDelay / 5f, scriptDelay / 2.5f);
    }

    private void ToggleAllOptions(bool flag) {
        foreach (GameObject button in buttons) {
            button.SetActive(flag);
        }
    }

    private void ToggleCanvas (bool flag) {
        promptCanvas.GetComponent<PromptAnimation>().Toggle(flag);
    }

    public void LoadStory(TextAsset loadText) {
        _inkStory = new Story(loadText.text);
        viewerNames = new string[(int)_inkStory.variablesState["viewers"]];
        viewerColors = new Color[(int)_inkStory.variablesState["viewers"]];

        string platformName = "";
        switch (Globals.platformSetting) {
            case 1:
                platformName = "MyTube";
                break;
            default:
            case 2:
                platformName = "Twotch";
                break;
        }
        try {
            _inkStory.variablesState["platform"] = platformName;
            _inkStory.variablesState["playername"] = Globals.username;
        } catch (Exception e3) {
            Debug.Log("Not Existing Variables");
        }

        switch(Globals.gameType) {
            default:
            case 1:
                _inkStory.variablesState["game_type"] = "plat";
                break;
            case 2:
                _inkStory.variablesState["game_type"] = "inv";
                break;
            case 3:
                _inkStory.variablesState["game_type"] = "mem";
                break;
        }
        
    }

    public int GetViewerCount() {
        return (int)_inkStory.variablesState["viewers"];
    }

    public void AddName(string name, int index) {
        if (index > viewerNames.Length) return;
        viewerNames[index] = name;
    }

    public void AttachDummies(ArrayList dummySet) {
        this.dummySet = dummySet;
    }

    public void SetupComplete() {
        chatboxDisplay = GameObject.Find("PlayerCanvas/ScreenCanvas/Right Window");

        for (int i = 0; i < viewerNames.Length; i++) {
            viewerColors[i] = UnityEngine.Random.ColorHSV(0f, 0.8f, 0.5f, 1f, 0.5f, 0.8f);
        }

        username = viewerNames[0];
        myColor = viewerColors[0];  // TO DO: Color bug when changing names

        storyLoaded = true;
    }

    private string Sanitise(string rawMessage) {
        switch (rawMessage.Trim()) {
            default:
                return rawMessage.Trim();
            case string s when s.Contains("L_D"):
                scriptCounter = UnityEngine.Random.Range(scriptDelay * 3, scriptDelay * 4);
                return "";
            case string s when s.Contains("S_D"):
                scriptCounter = 0.3f;
                return "";
            case string s when s.Contains("N_C"):
                int nameIndex = int.Parse(s.Split('_')[2]);
                username = viewerNames[nameIndex - 1];
                myColor = viewerColors[nameIndex - 1];
                return "";
            case string s when s.Contains("D_T"):
                int money = int.Parse(s.Split('_')[2]);
                string msg = s.Split('_')[3];
                Globals.dayMoney += money;
                DisplayDonation(money, msg);
                return "";
            case string s when s.Contains("S_C"):
                Globals.subNumber++;
                Globals.subNames += username + ",";
                DisplaySubbed();
                return "";
            case string s when s.Contains("B_N"):
                sendSystemBan(username);
                DeleteViewer();
                return "";
            case string s when s.Contains("E_C"):
                DeleteViewer();
                return "";
            case string s when s.Contains("F_X"):
                // TODO:
                return "";
            case string s when s.Contains("S_F"):
                // TODO:
                return "";
            case string s when s.Contains("I_M"):
                TextHandler myMessage = GameObject.Find("PlayerMessage").GetComponent<TextHandler>();
                float myDuration = 3f; float myDelay = 0.5f; Color color = Color.white;
                string myText = s.Split('_')[2];
                myMessage.SetText(myText, myDuration, myDelay, color);
                return "";
        }
    }

    private void DeleteViewer() {
        if (dummySet.Count > 0) {
            GameObject temp = (GameObject)dummySet[0];
            dummySet.RemoveAt(0);
            Destroy(temp);
        } else {
            Destroy(gameObject);
        }
    }
}
