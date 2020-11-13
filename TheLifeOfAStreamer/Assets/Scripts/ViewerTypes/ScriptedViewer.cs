using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Ink.Runtime;

public class ScriptedViewer : Viewer
{
	// Set this file to your compiled json asset
	public TextAsset inkAsset;
    public GameObject[] buttons;
	// The ink story that we're wrapping
	Story _inkStory;

    private string[] viewerNames;

    private bool choiceDisplayed = false;
    private bool storyLoaded = false;

    private float scriptDelay = 15f;
    private float scriptCounter = 0f;

    void Awake()
    {
        DisplayAllOptions(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!storyLoaded) return;

        if (scriptCounter > 0) scriptCounter -= Time.deltaTime;

        while (_inkStory.canContinue && scriptCounter <= 0) {
            scriptCounter = Random.Range(scriptDelay / 5f, scriptDelay);

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

        DisplayAllOptions(false);
        scriptCounter = Random.Range(scriptDelay / 5, scriptDelay / 2);
    }

    private void DisplayAllOptions(bool flag) {
        foreach (GameObject button in buttons) {
            button.SetActive(flag);
        }
    }

    public void LoadStory(TextAsset loadText) {
        _inkStory = new Story(inkAsset.text);
        viewerNames = new string[(int)_inkStory.variablesState["viewers"]];
    }

    public int GetViewerCount() {
        return (int)_inkStory.variablesState["viewers"];
    }

    public void AddName(string name, int index) {
        if (index > viewerNames.Length) return;
        viewerNames[index] = name;
    }

    public void SetupComplete() {
        username = viewerNames[0];
        storyLoaded = true;
    }

    private string Sanitise(string rawMessage) {
        switch (rawMessage.Trim()) {
            default:
                return rawMessage.Trim();
            case "L_D":
                scriptCounter = Random.Range(scriptDelay * 3, scriptDelay * 4);
                return "";
            case "Name":
                return "";
            case "DNT":
                return "";
            case "SBC":
                return "";
            case "E_C":
                return "";
            case "FX":
                return "";
            case "SFX":
                return "";
        }
    }
}
