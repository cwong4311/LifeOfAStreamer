using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneSpamming : MonoBehaviour
{
    private string[] wordBank;
    public Chatbox myChatBox;

    private Transform PhoneMenu;
    private Transform screenCanvas;
    private TextHandler monologue;

    private GameObject myDay;

    private float counter = 0f;
    private float delay = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(OverridePhoneMenu());
        populateWordBank();
        counter = 0f;

        monologue = GameObject.Find("PlayerCanvas/PlayerMessage").GetComponent<TextHandler>();
        screenCanvas = GameObject.Find("PlayerCanvas").transform.Find("ScreenCanvas");
        myDay = GameObject.Find("LevelFader/Fader");
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter > delay) {
            string myMessage = makeName() + ":\n" + "    " + wordBank[UnityEngine.Random.Range(0, wordBank.Length)];
            myChatBox.SendChatMessage(myMessage);
            counter = 0;
        }
    }

    IEnumerator OverridePhoneMenu() {
        bool found = false;
        
        while (!found) {
            try {
                PhoneMenu = GameObject.Find("PlayerCanvas").transform.Find("PhoneMenu");
 
                MenuController phoneController = PhoneMenu.gameObject.GetComponent<MenuController>();
                found = true;
                for (int i = 0; i < phoneController.myMenus.Length; i++) {
                    phoneController.myMenus[i] = myChatBox.gameObject;
                }

                StartCoroutine(SpamMonologue());
            } catch (Exception e) {
            } 
            yield return new WaitForSeconds(0.2f);
        }
    }

    void populateWordBank() {
        wordBank = new string[] {"I can see you", "I know where you live", "Look behind you", "Stop streaming", "I'm gunna come kill you", "You're dead",
                                "I'm gunna call the police on you", "Don't you dare try streaming again", "You don't deserve to play games", "You don't deserve anything",
                                "Keep living in your mother's basement", "Don't walk outside ever again", "Rot in hell", "I know your address", "Your family's gunna pay",
                                "Better watch your back", "Goddam nerd, don't stream ever again", "Gunna SWAT ur ass", "Kill yourself", "We know your phone number",
                                "email address and your family details", "We know everything about you", "We're watching you", "We see you, whever you go", "You can't run",
                                "You can't escape", "I'm gunna kill you, then kill your family", "You deserve to be dead", 
                                "Can see you", "I know your address", "Right behind you", "Just stop streaming", "Gunna kill you", "You're so dead",
                                "Gunna call the police", "Don't stream again", "Games are too good for you", "You don't deserve squat",
                                "Keep living on the streets", "Don't you dare come out of that hole again", "Die", "Better call your fam",
                                "Say goodbye to your feidns", "Goddam, don't stream again", "ur ass is so dead", "kill yourself", "We know everything",
                                "All your details are leaked", "We've got you", "You'll never have peace and quiet again", "You're worthless. Just kill yourself already", 
                                "You can't hide", "Die", "Gunna drown both you and your family", "You're better off dead", "Drown or something, idk", "Noone loves you"};
    }

    string makeName() {
        string myName = "0";
        for (int i = 0; i < 9; i++) {
            myName += UnityEngine.Random.Range(0,10) + "";
        }
        return myName;
    }

    IEnumerator SpamMonologue() {
        while (!PhoneMenu.gameObject.activeInHierarchy) {
            yield return new WaitForSeconds(0.2f);
        }

        myChatBox.gameObject.transform.SetParent(PhoneMenu, false);
        PhoneMenu.transform.Find("LeaveButton").gameObject.GetComponent<Button>().interactable = false;

        //Glitch
        GameObject.Find("Effects").GetComponent<FX>().PlayEffects(4);

        yield return new WaitForSeconds(3f);

        monologue.SetText("What");

        yield return new WaitForSeconds(3f);

        monologue.SetText("How did they find my number?");

        //Glitch
        GameObject.Find("Effects").GetComponent<FX>().PlayEffects(4);

        yield return new WaitForSeconds(3f);

        monologue.SetText("They know where I live..?");

        yield return new WaitForSeconds(3f);

        monologue.SetText("What do I even do");

        //Glitch
        GameObject.Find("Effects").GetComponent<FX>().PlayEffects(4);
        Transform statEff1 = GameObject.Find("Effects").GetComponent<FX>().PlayEffects(6);
        statEff1.gameObject.GetComponent<StaticHandler>().max = 0.2f;
        statEff1.gameObject.GetComponent<StaticHandler>().speed = 0.5f;
        statEff1.gameObject.GetComponent<StaticHandler>().duration = 120f;

        yield return new WaitForSeconds(4f);

        monologue.SetText("The stream! What's happening on the stream?");

        yield return new WaitForSeconds(4f);
        PhoneMenu.transform.Find("LeaveButton").gameObject.GetComponent<Button>().interactable = true;

        //Vignette Pulsing
        Transform vigEff = GameObject.Find("Effects").GetComponent<FX>().PlayEffects(7);

        while(!screenCanvas.gameObject.activeInHierarchy) {
            yield return new WaitForSeconds(2f);
        }

        //Static Short
        Transform statEff = GameObject.Find("Effects").GetComponent<FX>().PlayEffects(6);
        statEff.gameObject.GetComponent<StaticHandler>().max = 0.4f;
        statEff.gameObject.GetComponent<StaticHandler>().duration = 1f;

        yield return new WaitForSeconds(4f);

        monologue.SetText("Where did I go wrong?");

        yield return new WaitForSeconds(4f);

        monologue.SetText("It wasn't my fault.. was it?");

        yield return new WaitForSeconds(8f);

        //Static Short
        statEff = GameObject.Find("Effects").GetComponent<FX>().PlayEffects(6);
        statEff.gameObject.GetComponent<StaticHandler>().max = 0.6f;
        statEff.gameObject.GetComponent<StaticHandler>().duration = 1.2f;
        monologue.SetText("Make it stop...");

        yield return new WaitForSeconds(4f);

        //Static Long
        statEff = GameObject.Find("Effects").GetComponent<FX>().PlayEffects(6);
        statEff.gameObject.GetComponent<StaticHandler>().max = 1f;
        statEff.gameObject.GetComponent<StaticHandler>().speed = 0.5f;
        statEff.gameObject.GetComponent<StaticHandler>().duration = 10f;
        monologue.SetText("Please...");

        yield return new WaitForSeconds(3f);
        
        // Stop Vignette Pulsing
        vigEff.gameObject.GetComponent<VignettePulse>().StopAll();
        myDay.GetComponent<DayHandler>().DayEnd();
    }
}
