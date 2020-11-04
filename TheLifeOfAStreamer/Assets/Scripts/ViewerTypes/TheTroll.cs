using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheTroll : Viewer
{
    // Start is called before the first frame update
    protected override void populateDictionary() {
        wordBank = new string[,] {
            {"Lmao what a bad player", "what's goin on right now", "what am I even watching", "someone tell this kid to stop lmao", "this kid's so bad I had to comment", "lmao..", "ok cool"},
            {"what game even is this?", "this looks like garbage", "game looks kinda bad, not gonna lie", "I think I've seen this streamer before..", "this is super boring", "kid probably chose this game to get a niche. He's trolling", "why am I even here.."},
            {"Really? still going?", "wtf are you even doing", "Please play something else", "Dude, do you even play league?", "I swear i'd be better than this kid", "trash game, trash player", "dude probs can't even play sudoku properly"},
            {"Get outta here nerd. Noone's gunna watch this stream", "Just turn off the game already. Noone cares", "Just die already", "Who's having fun right now? Not me", "Go home to ur mommy, nurd", "Don't play games if u suck this much", "Don't bother streaming ever again"},
            {"You're never gonna get big playing trash like this", "Dude, I'd rather watch sniperLord64 right now", "Even BeastBoy90 can play better than you, and he's 9", "BeastBoy90 plays this game MUCH better", "Never seen this streamer, never want to again", "Dude, you even got fingers? Use them", "Don't come back ever again"},
            {"Dude probably plays games all day", "Dude's probably a virgin", "Trash neckbeard", "What if this streamer's actually a girl? Damn I'm leaving", "SniperLord64, save us please", "Dudes, subscribe to BeastBoy90 for better content", "Dude probs only cares about the money. Don't donate to him"},
            {"If you're still watching this dude, he's actually a racist prick", "Dude's a psycho on social media lmao. Check his twitter", "Dude, no-one likes this game or you", "Go home and cry to mommy", "If you donate to this dude, you're wasting your money", "Guys, I heard this dude does drugs and stuff", "Aren't you JonnyGames? The dude who left Twitch last year cuz u flamed a girl on social media?"}

        };
    }

    public override void endDay() {
        return;
    }

    //TO DO: Make all of this viewer's comments trolling, and make sure he spawns at certain audience counts & randomly.
    // - Don't need to add reaction or situational comments to this viewer:
    // - HOWEVER, add situational comments for other viewers whenever a troll comments. ie for, against or ignore.
    // - Whenever a troll comments, reduce attitude by X amount (small if happy, large if sad), and reduce further
    //   for each 'for' AI reaction. increase slightly for each against reaction
    protected override string getFillerWords() {
        return "";
    }
    protected override string getStreamerResponse(string msg) {
        return "";
    }

    protected override string getTrollResponse(string msg) {
        return "";
    }

    //TO DO: VFX (such as red text, screen darken / fog, quick zoom etc) whenever a troll comments, for EMPHASIS.
    protected override void sendMyMessage(string username, string message, int myLurk) {
        Globals.dayAttitude -= 3;
        string myMessage = internetFormat(message);
        if (Random.Range(0, 10) > myLurk) {
            chatBox.SendChatMessage(username + ": " + myMessage, Message.MessageType.trollMessage);
            Debug.Log(username + ": Request Successful");
            return;
        }
        Debug.Log(username + ": Lurk Rejection");
    }

    protected override void checkIfLeaving() {
        return;
    }

    protected override void donateToChannel() {
        return;
    }

    protected override void subToChannel() {
        return;
    }
}
