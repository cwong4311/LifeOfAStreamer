using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Viewer : MonoBehaviour
{
    public string username = "Guest";
    public float attitude = 0;
    public Chatbox chatBox;
    public GameObject myObject;

    protected float downtime = 0f;
    protected string[,] wordBank;
    protected string[] fillerBank;
    protected string[] positiveBank;
    protected string[] negativeBank;
    protected bool paused = false;
    protected int step = 0;
    // Start is called before the first frame update
    protected float secondWait = 5f;
    protected string prevStreamerMsg = "";
    protected string prevTrollMsg = "";
    protected float fillerWait = 10f;
    protected float leaveCheck = 30f;

    protected int mainLurk = 1; // Message miss rate is 10% by default
    protected int fillerLurk = 3; // Message miss rate is 30% by default

    protected float startingAttitude;
    protected Color myColor;

    public bool amSubbed = false;

    protected int timesDonated = 1;

    protected string[] prefixes = new string[] {"Dude", "DUDE", "Yo", "LMAO", "lmao", "KEK", "kek", "LOL", "lol", "yoooo", "damn", "dayum"};
    protected string[] suffixes = new string[] {"gg", "GG", "ggz", "pog", "pogs", "pogchamp", "poggers", "LMAO", "lmao", "KEK", "kek", "LOL", "lol", "haha", "hahahaha"};

    public virtual void setup() {
        myColor = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
        startingAttitude = attitude;
        downtime = Random.Range(3f, 40f);
        populateDictionary();
        populateFillerBank();
        populatePositive();
        populateNegative();
    }

    protected virtual void Start()
    {
        myColor = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
        setup();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (paused) return;

        downtime -= Time.deltaTime;
        if (downtime <= 0) {
            string myMessage;
            switch(step) {
                case 0:
                    //Debug.Log(wordBank);
                    myMessage = wordBank[0, Random.Range(0, wordBank.GetLength(1))];
                    downtime = Random.Range(15f, 60f);
                    break;
                case 1:
                    myMessage = wordBank[1, Random.Range(0, wordBank.GetLength(1))];
                    downtime = Random.Range(30f, 120f);
                    break;
                case 2:
                    myMessage = wordBank[2, Random.Range(0, wordBank.GetLength(1))];
                    downtime = Random.Range(60f, 240f);
                    break;
                case 3:
                    myMessage = wordBank[3, Random.Range(0, wordBank.GetLength(1))];
                    downtime = Random.Range(120f, 360f);
                    break;
                case 4:
                    myMessage = wordBank[4, Random.Range(0, wordBank.GetLength(1))];
                    downtime = Random.Range(360f, 1440f);
                    break;
                default:
                    myMessage = wordBank[5, Random.Range(0, wordBank.GetLength(1))];
                    downtime = Random.Range(720f, 2880f);
                    break;
            }
            //Debug.Log(username + ": Request to Send");
            sendMyMessage(username, myMessage, mainLurk);
            step += 1;
        }

        fillerWait -= Time.deltaTime;
        if (fillerWait <= 0 && downtime > 5f) {
            fillerWait = Random.Range(30, 90);

            string myMessage = getFillerWords();
            if (myMessage != "") sendMyMessage(username, myMessage, fillerLurk);
        }

        secondWait -= Time.deltaTime;
        if (secondWait <= 0) {
            secondWait = Random.Range(3, 10);

            subToChannel();
            donateToChannel();

            bool sent = false;
            
            Message streamerMessage = chatBox.getLastPlayerMessage();
            if (streamerMessage.messageType == Message.MessageType.streamerMessage && streamerMessage.text != "") {
                if (streamerMessage.text != prevStreamerMsg) {
                    prevStreamerMsg = streamerMessage.text;

                    string myMessage = getStreamerResponse(streamerMessage.text);

                    if (myMessage != "") {
                        sendMyMessage(username, myMessage, mainLurk);
                        sent = true;
                    }
                }
            }

            if (!sent) {
                Message trollMessage = chatBox.getLastTrollMessage();
                if (trollMessage.messageType == Message.MessageType.trollMessage && trollMessage.text != "") {
                    if (trollMessage.text != prevTrollMsg) {
                        prevTrollMsg = trollMessage.text;

                        attitude -= 5f;

                        string myMessage = getTrollResponse(streamerMessage.text);

                        if (myMessage != "") {
                            sendMyMessage(username, myMessage, mainLurk);
                            sent = true;
                        }
                    }
                }
            }

            attitude += Random.Range(-0.5f, 0.5f);
        }

        checkIfLeaving();
    }

    public virtual void endDay()
    {
        subToChannel();
        Globals.dayAttitude += Mathf.Min(attitude, 100f) / 200f;

        Pause();
    }

    protected virtual void donateToChannel() {    
        //If viewer happy, and 5% (decreasing) chance
        int checkValue = 5 / timesDonated;
        
        if (attitude > 20 && Random.Range(0, 100) < (checkValue > 0 ? checkValue : 1)) {
            int donationAmount = (int)Random.Range(attitude / 4, attitude + 1);
            Globals.dayMoney += donationAmount;
            Globals.dayAttitude += ((float)donationAmount / 50);

            timesDonated++;
            DisplayDonation(donationAmount);
        }

    }

    protected virtual void subToChannel() {
        // If this viewer isn't subbed, chance of subbing
        if (!amSubbed) {
            if (attitude > 20) {
                int checkValue = (int) (Globals.popularity > 20 ? 20 : Globals.popularity); if (checkValue < 5) checkValue = 5;
                if (Random.Range(0, 50) < checkValue) {
                    Globals.subNumber++;
                    Globals.subNames += username + ",";
                    Globals.dayAttitude += 0.3f;
                    amSubbed = true;

                    DisplaySubbed();
                }         
            } else {
                int checkValue = (int) (Globals.popularity > 10 ? 10 : Globals.popularity); if (checkValue < 1) checkValue = 1;
                if (Random.Range(0, 100) < checkValue) {
                    Globals.subNumber++;
                    Globals.subNames += username + ",";
                    Globals.dayAttitude += 0.3f;
                    amSubbed = true;

                    DisplaySubbed();
                }
            }
        // If this viewer is subbed, chance of leaving
        } else {
            if (attitude < -50) {
                int checkValue = (int) (Globals.popularity > 10 ? 10 : Globals.popularity); if (checkValue < 1) checkValue = 1;
                if (Random.Range(0, 100) < checkValue) {
                    if (Globals.subNumber > 0) {

                        // Remove this user from the list of subs
                        ArrayList returnerNames = new ArrayList();
                        foreach (string name in Globals.subNames.Split(',')) {
                            if (name.Trim() == "") continue;
                            if (name.Trim() == username) continue;
                            returnerNames.Add(name.Trim());
                        }

                        // Add back to sublist without this name
                        Globals.subNames = string.Join(",", (string[])returnerNames.ToArray(typeof(string)));
                        Globals.subNumber--;
                        Globals.dayAttitude -= 0.7f;
                    }       
                }   
            } else {
                int checkValue = (int) (Globals.popularity > 10 ? 10 : Globals.popularity); if (checkValue < 1) checkValue = 1;
                if (Random.Range(0, 200) < checkValue) {
                    if (Globals.subNumber > 0) {

                        // Remove this user from the list of subs
                        ArrayList returnerNames = new ArrayList();
                        foreach (string name in Globals.subNames.Split(',')) {
                            if (name.Trim() == "") continue;
                            if (name.Trim() == username) continue;
                            returnerNames.Add(name.Trim());
                        }

                        // Add back to sublist without this name
                        Globals.subNames = string.Join(",", (string[])returnerNames.ToArray(typeof(string)));
                        Globals.subNumber--;
                        Globals.dayAttitude -= 0.7f;
                    }      
                }     
            }
        }
    }

    public void Pause() {
        paused = true;
    }

    public void Resume() {
        paused = false;
    }

    protected virtual void populateDictionary() {
        // Make this one the neutral viewer
        if (attitude >= 0) {
            wordBank = new string[,] {
                {"Hey, interesting game", "How are you doing?", "hi", "hello", "first time", "nice it's this game", "yo"},
                {"What's this called?", "New to streaming?", "Haven't played this in ages..", "Nice to see a streamer pick this up", "kek", "How're you finding it?", "Did you make this?"},
                {"Never heard it before", "How're you finding streaming", "Makes me wanna play", "Why don't more people play this", "Is it fun?", "How hard is it?", "how do you play this?"},
                {"That's cool. If you're liking it, man.", "Streaming's hard..", "Where do I get this?", "How much is it?", "Looks kinda boring.. idk", "Dude, you play league?", "kek"},
                {"Might give this a shot myself", "I think I prefer something else", "Dude u're pretty good at this", "You're pretty good at streaming", "looks cool", "I wanna play some games now", "nice"},
                {"ggz", "Aight, heading off..", "g2g, see ya later", "good luck", "cool", "lmao", "bb"}
            };
        } else {
            wordBank = new string[,] {
                {"What game is this?", "Lmao what is this", "didn't plan to talk, but dude u suck", "Came in for fun. Wut's up guys", "hi", "hello", "just dropping by for a bit", "yooo"},
                {"How's it going peeps", "Dude, you suck. I could play better than that", "You're pretty boring.", "this is kinda boring..", "idk about this", "pass for me", "nah", "This game looks mad low quality"},
                {"boooooring", "why are you even playing this", "go back to sudoku", "what game is this again?", "who made this?", "dude, u should play league instead", "hey dude wanna play something else", "kek, looks kinda trashy"},
                {"You're pretty boring.", "u should do something else with your time", "play something else", "you're still playing this?", "have you beaten the stage yet?", "you're still going?", "have you won yet?", "nahh"},
                {"still going?", "Dude, you suck", "how much longer is this..", "dude u played fps before?", "seriously... can't believe I'm still here", "nahhhh", "why r u still streaming dis", "yikes dude.. really>"},
                {"GGz I'm out", "nah I'm gonna go do something else", "too boring. I quit", "You should really try something else", "Streaming might not be for you, man", "this ain't it", "nahhh", "maybe I might try this out"}
            };
        }
        //TO DO: Add situationals. 10 for each step. 30 for others
    }

    protected virtual void populateFillerBank() {
        fillerBank = new string[] {"cool", "lmao", "gg", "ggz", "hahahah", "haha", "wow", "op", "poggers", "pogs", "troller", "nice", "damn", "wish I was this good", "woah", "omg",
        "that's awesome dude", "k", "aight", "okayyy", "lol", "noice", "brb", "be right back", "one sec", "gonna grab some food", "gonna grab some water", "grabbing food", "grabbing water",
        "leaving chat for a bit", "u're doin good", "You're doing good", "dude you can do better", "oh boy", "is it happening?", "Is It Happening!?", "niiiiiiiice", "that's crazy",
        "bonkers", "play something else!", "F", "....", "hmmm", "hmmmmm"};
    }

    protected virtual void populatePositive() {
        positiveBank = new string[] {"cool", "cool!", "nice", "niiiceee", "ayyy", "yoooo", "pogs", "poggers", "hahahaha alright", "aight", "coolios", "fair", "fairo", "pogchamp",
        "omg", "Poggers in the chat!", "hahahaha", "alright!", "thanks", "cheers", "gg", "no wayyyy", "that rocks dude", "LIT!", "cooooolll", "!!", "!"};
    }

    protected virtual void populateNegative() {
        negativeBank = new string[] {"no wayyy", "hmmm", "no", "shut up", "please no", "there's no way", "nahhh", "I don't think that's...", "you sure?", "nop", "stfu", "no u",
        "yiiiikesss", "yikes", "this ain't it..", "that's not cool, bro", "that's not cool man", "....", "..", "that's terrible", "what", "wot", "wut"};
    }

    protected string internetFormat(string inputString) {
        string originalString = inputString;
        int presufChance = Random.Range(0, 10);
        string[] joins = new string[] {" ", ", "};

        //Add prefix or suffix phrase with 20% chance each (total 40% chance)
        if (presufChance < 2) {
            originalString = prefixes[Random.Range(0, prefixes.Length)] + joins[Random.Range(0, joins.Length)] + originalString;
        } else if (presufChance < 4) {
            originalString = originalString + joins[Random.Range(0, joins.Length)] + suffixes[Random.Range(0, suffixes.Length)];
        }

        string formattedString = "";

        for (int i = 0; i < originalString.Length; i++) {
            if (i == 0) {
                if (Random.Range(0, 3) == 0) {
                    formattedString += char.ToUpper(originalString[i]);
                } else {
                    formattedString += char.ToLower(originalString[i]);
                }
            } else {
                int myRandom = Random.Range(0, 60);
                if (myRandom == 0) {
                    //Delete this letter, or keep it with 50% chance
                    if (Random.Range(0, 4) > 2) {
                        formattedString += originalString[i];
                    }
                } else if (myRandom == 1) {
                    formattedString += char.ToUpper(originalString[i]);
                } else if (myRandom == 2) {
                    char myChar = (char)((char.ToLower(originalString[i]) + (char) Random.Range(1, 4)) % 'z');
                    if (Random.Range(0, 10) < 2) myChar = char.ToUpper(myChar);
                    formattedString += myChar;
                } else {
                    formattedString += originalString[i];
                }
            }
        }

        return formattedString;
    }

    protected virtual string getFillerWords() {
        return fillerBank[Random.Range(0, fillerBank.Length)];
    }

    protected virtual string getStreamerResponse(string streamerMsg) {
        string myMessage = "";
        if (Random.Range(0, 2) == 0) {
            myMessage = positiveBank[Random.Range(0, positiveBank.Length)];
            attitude += Random.Range(0, 3);
        } else {
            myMessage = negativeBank[Random.Range(0, negativeBank.Length)];
            attitude -= Random.Range(0, 3);
        }

        return myMessage;
    }

    protected virtual string getTrollResponse(string trollMsg) {
        string myMessage = "";
        if (Random.Range(0, 2) == 0) {
            myMessage = positiveBank[Random.Range(0, positiveBank.Length)];
            attitude -= Random.Range(0, 3);
        } else {
            myMessage = negativeBank[Random.Range(0, negativeBank.Length)];
            attitude += Random.Range(0, 3);
        }

        return myMessage;
    }

    protected virtual void sendMyMessage(string username, string message, int myLurk) {
        string myMessage = internetFormat(message);
        if (Random.Range(0, 10) > myLurk) {
            chatBox.SendChatMessage(username + ": " + myMessage, myColor);
            //Debug.Log(username + ": Request Successful");
            return;
        }
        //Debug.Log(username + ": Lurk Rejection");
    }

    protected virtual void sendMyRawMessage(string username, string message) {
        chatBox.SendChatMessage(username + ": " + message, myColor);
    }

    protected virtual void sendSystemBan(string username) {
        chatBox.SendChatMessage("System: [" + username + "] has been banned.", Message.MessageType.info);
    }

    protected virtual void checkIfLeaving() {
        leaveCheck -= Time.deltaTime;
        if (leaveCheck <= 0) {
            leaveCheck = 20f;
            // If attitude has only got worse by 10
            if (attitude < (startingAttitude - 10f)) {
                // Leave with 30% chance
                if (Random.Range(0, 10) < 3) {
                    endDay();
                    if (myObject != null) Destroy(myObject, 3f);
                }
            } else {
                // Otherwise leave with 2% chance
                if (Random.Range(0, 100) < 2) {
                    endDay();
                    if (myObject != null) Destroy(myObject, 3f);
                }            
            }  
        }
    }

    protected virtual void DisplaySubbed() {
        TextHandler streamMessage = GameObject.Find("PlayerCanvas/ScreenCanvas/SubMessage").GetComponent<TextHandler>();
        float myDuration = 3f; float myDelay = 0.5f; Color color = Color.white;

        streamMessage.SetText("You got a new sub!:\n" + username, myDuration, myDelay, color);
    }

    protected virtual void DisplayDonation(int amount, string msg) {
        TextHandler streamMessage = GameObject.Find("PlayerCanvas/ScreenCanvas/SubMessage").GetComponent<TextHandler>();
        float myDuration = 4f; float myDelay = 0.5f; Color color = Color.white;

        streamMessage.SetText(username + " has donated: $" + amount + "\n\"" + msg +"\"", myDuration, myDelay, color);
    }

    protected virtual void DisplayDonation(int amount) {
        DisplayDonation(amount, "");
    }
}
