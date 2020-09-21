using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject myPlatform;
    public GameObject myPlayer;
    public GameObject myUI;
    public GameObject myRootObject;
    private Vector2 y_boundary = new Vector2(-7f, 7f);
    private Vector3 origin = new Vector3(18f, 0f, -0.5f);
    private float origin_y = 3f;
    private float lastY = 6.684f;
    private float lengthDelay = 1.5f;

    private float delayTimer;

    private float totalScore;

    private bool running = false;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.tag = "Untagged";
    }

    // Update is called once per frame
    void Update()
    {
        if (Globals.gameSetting == -1) {
            foreach(MonoBehaviour c in myPlayer.GetComponents<MonoBehaviour>()) {c.enabled = false;}
            myPlayer.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            running = false;
            return;
        }

        if (!running) {
            running = true;
            foreach(MonoBehaviour c in myPlayer.GetComponents<MonoBehaviour>()) {c.enabled = true;}
            myPlayer.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }

        bool spawnPlatform = false;

        totalScore += Time.deltaTime;
        myUI.transform.Find("Score").GetComponent<TextMesh>().text = "" + (int) totalScore;

        delayTimer += Time.deltaTime;
        if (delayTimer > lengthDelay) {
            if (Random.Range(0, 50) < 49) {
                if (delayTimer > lengthDelay + 2.5f) {
                    spawnPlatform = true;
                    delayTimer = 0f;
                }
            } else {
                spawnPlatform = true;
                delayTimer = 0f;
            }
        }

        if (spawnPlatform) {
            GameObject newPlatform = Instantiate(myPlatform, transform.parent, false);
            newPlatform.transform.localPosition = origin;

            float myLength = Random.Range(-0.5f, 0.5f) * 4;
            float myY = 0.5f + Random.Range(-2.5f, 2f) * 2;
            int breakLoop = 0;
            while (myY + lastY < y_boundary.x || myY + lastY > y_boundary.y) {
                myY = 0.5f + Random.Range(-2.5f, 2f) * 2;
                breakLoop += 1;
                if (breakLoop >= 30) {
                    myY = origin_y;
                    lastY = 0;
                    break;
                }
            }

            newPlatform.transform.localScale += new Vector3(myLength, 0f, 0f);
            newPlatform.transform.localPosition += new Vector3(0f, lastY + myY, 0f);
            lastY += myY;
            lengthDelay = 1.5f + (myLength / 2f);
        }

        if (myPlayer.transform.localPosition.y < -8 || myPlayer.transform.localPosition.x < -35 ) GameOver();
    }

    public void GameOver() {
        Globals.gameSetting = -1;
        Globals.gameScore = (Globals.gameScore > totalScore) ? Globals.gameScore : totalScore;

        myRootObject.tag = "GameOver";

        myPlayer.SetActive(false);
        myUI.transform.Find("GameOver").gameObject.SetActive(true);
    }
}
