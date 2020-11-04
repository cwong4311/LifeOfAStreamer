using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewerControlSystem : MonoBehaviour
{
    public GameObject chatBox;
    public GameObject[] viewerTypes;
    public TextAsset usernameFile;
    private bool paused;
    private bool dayEnd = false;
    private int test_created = 3;
    private string[] viewerNames;

    private ArrayList returnerNames = new ArrayList();

    private int dayTroll = 0;
    private int returningViewer = 0;
    private int todayReturner = 0;

    private float spawnTimer = 0;

    private string myRoute = "intro";

    // Start is called before the first frame update
    void Start()
    {
        viewerNames = usernameFile.text.Split('\n');
        if (Globals.subNumber > 0) {
            todayReturner = Globals.subNumber;

            foreach (string name in Globals.subNames.Split(',')) {
                if (name.Trim() == "") continue;
                returnerNames.Add(name.Trim());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dayEnd) return;

        if (!chatBox.activeInHierarchy) {
            paused = true;
            foreach (Viewer viewer in GetComponentsInChildren<Viewer>()) {
                viewer.Pause();
            }
            return;
        }

        if (paused) {
            paused = false; 
            foreach (Viewer viewer in GetComponentsInChildren<Viewer>()) {
                viewer.Resume();
            }
        }

        // RNG System
        if (Globals.days == 3 && Globals.gameFlag != -1 && test_created > 0) {
            //createViewer();
            createGoodViewer();
            if (test_created == 1) createTroll();
            test_created --;
        } 
        
        // Generate Viewer with delay (seconds)
        if (Globals.days >= 3 && Globals.hasStreamed && Globals.gameFlag != -1) {
            
            float spawnDelay = 30f - (float) Globals.days - (float) Globals.popularity;
            if (spawnDelay < 5) spawnDelay = 5;

            //Check if a viewer spawns every X seconds (based on day count)
            spawnTimer += Time.deltaTime;
            if (spawnTimer > spawnDelay) {
                spawnTimer = 0;

                if ((Random.Range(0, 10000) + (Globals.popularity * 10)) >= 10000) {
                    createViewer();
                }
            }
        }

        // If the Streamer has streamed, every 10 seconds, have a random amount of
        // subscribers return to stream.
        if (Globals.hasStreamed && Globals.gameFlag != -1) {
            float returnDelay = 10f;
            if ((int)spawnTimer % (int)returnDelay == 0) {
                if (returningViewer < todayReturner) {
                    for (int i = 0; i < Random.Range(1, todayReturner - returningViewer + 1); i++) {
                        if (returnerNames.Count == 0) break;

                        string thisName = returnerNames[0].ToString();
                        createReturner(thisName);
                        returningViewer++;

                        returnerNames.RemoveAt(0);
                    }
                }
            }
        }

        // On Day 5 onwards, make a troll for every 20 viewers on that day.
        if (Globals.days >= 5 && dayTroll < (int) (Globals.dayViewer / 20) && spawnTimer == 0) {
            createTroll();
        // Otherwise, if no popularity, make a troll every 5 days
        } else if ((int) (Globals.dayViewer / 20) == 0) {
            if (Globals.days % 5 == 0 && dayTroll == 0) {
                createTroll();
                dayTroll++;
            }
        }

        //Route System
        if (Globals.days == 5) {
            // Slow Increase to 10~20 viewers, random trolls
        } else if (Globals.days == 10) {
            // If Pop up & Att up, increase viewers
            // If Pop up & Att down, increase viewers
            // If Pop down & Att down, decrease viewers
            // If Pop down & Att up, increase trolls more
        } else if (Globals.days == 15) {

        } else if (Globals.days == 20) {

        } else if (Globals.days == 25) {

        }
    }

    private void createViewer() {
        Globals.dayViewer += 1;

        // 9 types of viewers. 
        /* 
           > 0 - 1 is friendly
           >     2 is lurker (neutral but low talk)
           > 3 - 5 is neutral
           > 6 - 8 is negative
        */
        int mySpawnType = Random.Range(0,100);
        int viewType;
        float myAttitude;
        string nameNumbers = "";

        // If low popularity and first 20 days, mostly only lurkers spawn
        if (Globals.popularity < 20 && Globals.days < 20) {
            // 80% chance to spawn a lurker
            if (mySpawnType < 80) {
                Debug.Log("Lurker Spawn [Low POP]");
                viewType = 2;
                myAttitude = Random.Range(-5f, 5f);
            // 15% chance to spawn something good or neutral
            } else if (mySpawnType < 95) {
                Debug.Log("Neutral Spawn [Low POP]");
                viewType = Random.Range(0, 6);
                myAttitude = Random.Range(-10f, 10f);
            // 5% chance to spawn something negative (no troll)
            } else {
                Debug.Log("Negative Spawn");
                viewType = Random.Range(6, 8);
                myAttitude = Random.Range(-20f, -10f);
            }
        // Once popularity over threshold, give a decent spawn difference
        } else {
            // 45% chance to spawn something related to attitude
            if (mySpawnType < 45) {
                Debug.Log("Attitude Spawn");
                if (Globals.attitude > 30) {
                    viewType = Random.Range(0, 2);
                    myAttitude = Random.Range(30f, 50f);
                } else if (Globals.attitude < -30) {
                    viewType = Random.Range(6, 9);
                    myAttitude = Random.Range(-50f, -30f);
                } else {
                    viewType = Random.Range(2, 6);
                    myAttitude = Random.Range(-30f, 30f);
                }
            // 30% chance to spawn a lurker
            } else if (mySpawnType < 75) {
                Debug.Log("Lurker Spawn");
                viewType = 2;
                myAttitude = Random.Range(-10f, 10f);
            // 20% chance to spawn something neutral
            } else if (mySpawnType < 95) {
                Debug.Log("Neutral Spawn");
                viewType = Random.Range(2, 6);
                myAttitude = Random.Range(-30f, 30f);
            // 5% chance to spawn something negative
            } else {
                Debug.Log("Negative Spawn");
                viewType = Random.Range(6, 9);
                myAttitude = Random.Range(-100f, -30f);
            }
        }

        if (Random.Range(0, 100) > 30) {
            for (int i = 0; i < Random.Range(0, 3); i++) {
                nameNumbers += Random.Range(0, 10).ToString();
            }
        }
        string myName = viewerNames[Random.Range(0, viewerNames.Length)] + "" + nameNumbers;

        GameObject newViewer = Instantiate(viewerTypes[viewType], transform);
        newViewer.name = viewType + "_" + myName;
        newViewer.GetComponent<Viewer>().chatBox = chatBox.GetComponent<Chatbox>();
        newViewer.GetComponent<Viewer>().username = myName;
        newViewer.GetComponent<Viewer>().attitude = myAttitude;
        newViewer.GetComponent<Viewer>().myObject = newViewer;
        newViewer.GetComponent<Viewer>().setup();
    }

    private void createReturner(string name) {
        Globals.dayViewer += 1;

        // 9 types of viewers. 
        /* 
           > 0 - 1 is friendly
           >     2 is lurker (neutral but low talk)
           > 3 - 5 is neutral
           > 6 - 8 is negative
        */
        int mySpawnType = Random.Range(0,5);
        int viewType;
        float myAttitude;

        // Returning viewers are usually supportive
        // Only have change of spawning good, lurkers or neutral.
        if (mySpawnType == 0) {
            viewType = 3;
            myAttitude = Random.Range(-5f, 5f);
        } else if (mySpawnType == 1) {
            viewType = 2;
            myAttitude = Random.Range(-5f, 5f);
        } else {
            viewType = 0;
            myAttitude = Random.Range(30f, 50f);
        }

        string myName = name;

        GameObject newViewer = Instantiate(viewerTypes[viewType], transform);
        newViewer.name = viewType + "_" + myName;
        newViewer.GetComponent<Viewer>().chatBox = chatBox.GetComponent<Chatbox>();
        newViewer.GetComponent<Viewer>().username = name;
        newViewer.GetComponent<Viewer>().attitude = myAttitude;
        newViewer.GetComponent<Viewer>().myObject = newViewer;
        newViewer.GetComponent<Viewer>().amSubbed = true;
        newViewer.GetComponent<Viewer>().setup();
    }

    private void createGoodViewer() {
        Globals.dayViewer += 1;

        // 9 types of viewers. 
        /* 
           > 0 - 1 is friendly
           >     2 is lurker (neutral but low talk)
           > 3 - 5 is neutral
           > 6 - 8 is negative
        */
        int viewType;
        float myAttitude;
        string myName = "Good_Demo_" + Random.Range(0,1000);

        // Good Viewer Only
        viewType = 0;
        myAttitude = Random.Range(30f, 50f);
        

        GameObject newViewer = Instantiate(viewerTypes[viewType], transform);
        newViewer.name = viewType + "_" + myName;
        newViewer.GetComponent<Viewer>().chatBox = chatBox.GetComponent<Chatbox>();
        newViewer.GetComponent<Viewer>().username = myName;
        newViewer.GetComponent<Viewer>().attitude = myAttitude;
        newViewer.GetComponent<Viewer>().myObject = newViewer;
        newViewer.GetComponent<Viewer>().setup();
    }

    private void createTroll() {
        Globals.dayViewer += 1;

        Debug.Log("Troll Spawn");

        float myAttitude = -100;
        string nameNumbers = "";
        int viewType = 8;

        if (Random.Range(0, 100) > 30) {
            for (int i = 0; i < Random.Range(0, 3); i++) {
                nameNumbers += Random.Range(0, 10).ToString();
            }
        }
        string myName = viewerNames[Random.Range(0, viewerNames.Length)] + "" + nameNumbers;

        GameObject newViewer = Instantiate(viewerTypes[viewType], transform);
        newViewer.name = "Troll_" + myName;
        newViewer.GetComponent<Viewer>().chatBox = chatBox.GetComponent<Chatbox>();
        newViewer.GetComponent<Viewer>().username = myName;
        newViewer.GetComponent<Viewer>().attitude = myAttitude;
        newViewer.GetComponent<Viewer>().myObject = newViewer;
        newViewer.GetComponent<Viewer>().setup();
    }

    public void endDay() {
        dayEnd = true;

        foreach (Viewer viewer in GetComponentsInChildren<Viewer>()) {
            viewer.endDay();
        }
    }
}
