using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodViewer : Viewer
{
    // Start is called before the first frame update
    protected override void populateDictionary() {
        wordBank = new string[,] {
            {"First time watching! c:", "Hey, how's it going", "Heyyyysss", "Woah cool game", "Woah what's this game called?", "Hi there!", "Oh hey, it's this game!"},
            {"Not bad!", "You're doing good!", "That was pretty cool", "You can do it!", "This game's hard, haha", "Niiicee", "Ayy"},
            {"Does this game go forever?", "This game looks really fun", "Where can I get this game?", "Nice moves", "Woah, this streamer's good", "Nice!", "Coooll"},
            {"Pogs, that's really good", "You should stream more!", "Very cool", "Woah hahaha", "U usually stream around this time?", "LOL", "hahahaha"},
            {"This dude's really hanging in there", "Woah that's a really high score", "This streamer's good", "Niiicee", "Keep it up!", "LMAO", "AyyYy"},
            {"ggz", "I'll tune bak in next time you stream", "That was pretty cool", "Gonna pop put for a bit. Good Luck!", "Woah, he's still going!", "op", "2 strong"}
        };
    }
    
    protected override string getStreamerResponse(string streamerMsg) {
        string myMessage = "";
        if (Random.Range(0, 5) > 0) {
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
        if (Random.Range(0, 5) == 5) {
            myMessage = positiveBank[Random.Range(0, positiveBank.Length)];
            attitude -= Random.Range(0, 3);
        } else {
            myMessage = negativeBank[Random.Range(0, negativeBank.Length)];
            attitude += Random.Range(0, 3);
        }

        return myMessage;
    }
}
