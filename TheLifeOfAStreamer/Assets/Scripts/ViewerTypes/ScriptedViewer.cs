using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Ink.Runtime;

public class ScriptedViewer : Viewer
{
	// Set this file to your compiled json asset
	public TextAsset inkAsset;
    public GameObject promptCanvas;
    public GameObject[] buttons;
	// The ink story that we're wrapping
	Story _inkStory;

    private string[] viewerNames;
    private ArrayList dummySet;

    private bool choiceDisplayed = false;
    private bool storyLoaded = false;

    private float scriptDelay = 5f;     //Default - 15f, Debug - 5f
    private float scriptCounter = 0f;

    private GameObject chatboxDisplay;

    void Awake()
    {
        dummySet = new ArrayList();
        scriptCounter = Random.Range(scriptDelay / 2, scriptDelay);
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
            scriptCounter = Random.Range(scriptDelay / 3f, scriptDelay);

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
        scriptCounter = Random.Range(scriptDelay / 5f, scriptDelay / 2.5f);
    }

    private void ToggleAllOptions(bool flag) {
        foreach (GameObject button in buttons) {
            button.SetActive(flag);
        }
    }

    private void ToggleCanvas (bool flag) {
        promptCanvas.SetActive(flag);
    }

    public void LoadStory(TextAsset loadText) {
        _inkStory = new Story(inkAsset.text);
        viewerNames = new string[(int)_inkStory.variablesState["viewers"]];
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
        username = viewerNames[0];

        storyLoaded = true;
    }

    private string Sanitise(string rawMessage) {
        switch (rawMessage.Trim()) {
            default:
                return rawMessage.Trim();
            case string s when s.Contains("L_D"):
                scriptCounter = Random.Range(scriptDelay * 3, scriptDelay * 4);
                return "";
            case string s when s.Contains("S_D"):
                scriptCounter = 0.3f;
                return "";
            case string s when s.Contains("N_C"):
                // TODO:
                return "";
            case string s when s.Contains("D_T"):
                // TODO:
                return "";
            case string s when s.Contains("S_C"):
                // TODO:
                return "";
            case string s when s.Contains("E_C"):
                if (dummySet.Count > 0) {
                    GameObject temp = (GameObject)dummySet[0];
                    dummySet.RemoveAt(0);
                    Destroy(temp);
                } else {
                    Destroy(gameObject);
                }
                return "";
            case string s when s.Contains("F_X"):
                // TODO:
                return "";
            case string s when s.Contains("S_F"):
                // TODO:
                return "";
            case string s when s.Contains("I_M"):
                TextHandler myMessage = GameObject.Find("PlayerMessage").GetComponent<TextHandler>();
                float myDuration = 3f; float myDelay = 0.5f; Color myColor = Color.white;
                string myText = s.Split('_')[2];
                myMessage.SetText(myText, myDuration, myDelay, myColor);
                return "";
        }
    }
}
