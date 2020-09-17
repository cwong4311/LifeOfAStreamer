using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadViewer : Viewer
{
    // Start is called before the first frame update
    protected override void populateDictionary() {
        wordBank = new string[,] {
            {"Lmao what a bad player", "what's goin on right now", "what am I even watching", "someone tell this kid to stop lmao", "wtf is this..", "lmao..", "ok cool"},
            {"what game even is this?", "this looks like it's from the 90s", "game looks kinda bad, not gonna lie", "why did u even choose this game", "this is pretty boring", "I wanna see something else", "why am I even here.."},
            {"Really? still going?", "wtf are you even doing", "Please play something else", "Dude, do you play league?", "I swear i'd be better than this kid", "trash game", "trash player"},
            {"Still going..", "How long is this game even", "Just die already", "Please change games, I can't", "yikes..", "please..", "holy..."},
            {"why is he even playing this again?", "y'all should go do something better with your time", "gunna go play some league", "swap games already!", "this ain't it, chief..", "nahhh", "why..."},
            {"is he still going? Holy...", "just stop already, geez", "why is he still playing", "can you stop already?", "I can't watch this anymore", "can't believe there's someone worse than me out there", "why..."},
            {"i dunno if i'll watch this kid anymore", "aight, I'm gone", "gg, pls stop", "yeah, no way", "I just wasted my life watching this..", "nah, no way", "You should probably pick another game.."}
        };
    }

    protected override string getStreamerResponse(string streamerMsg) {
        string myMessage = "";
        if (Random.Range(0, 5) == 5) {
            myMessage = positiveBank[Random.Range(0, positiveBank.Length)];
            attitude += Random.Range(0, 3);
        } else {
            myMessage = negativeBank[Random.Range(0, negativeBank.Length)];
            attitude -= Random.Range(0, 3);
        }

        return myMessage;
    }

    protected override string getTrollResponse(string trollMsg) {
        string myMessage = "";
        if (Random.Range(0, 5) > 0) {
            myMessage = positiveBank[Random.Range(0, positiveBank.Length)];
            attitude -= Random.Range(0, 3);
        } else {
            myMessage = negativeBank[Random.Range(0, negativeBank.Length)];
            attitude += Random.Range(0, 3);
        }

        return myMessage;
    }
}
