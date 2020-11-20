using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkHandler : MonoBehaviour
{
    public Material drunkMat;

    private Drunk drunk;
    private float lifetime;

    private SoundHandler sound;
    private int mySound;
    // Start is called before the first frame update
    void Awake()
    {
        drunk = GameObject.Find("FPSController/FirstPersonCharacter").AddComponent<Drunk>();
        drunk.material = drunkMat;
        lifetime = 10f;
        StartCoroutine(AliveDecay());

        sound = GameObject.Find("Sound").GetComponent<SoundHandler>();
        mySound = sound.PlayAudio(3, true);
    }

    void Update() {
        if (GameObject.Find("FPSController").GetComponent<MonoBehaviour>().enabled) {
            if (!drunk.enabled) {
                drunk.enabled = true;
                sound.ResumeAudio(mySound);
            }
        } else {
            if (drunk.enabled) {
                drunk.enabled = false;
                sound.StopAudio(mySound);
            }
        }
    }

    public void SetLife(float life) {
        lifetime = life;
    }

    IEnumerator AliveDecay() {
        float counter = 0f;
        while (counter < lifetime) {
            counter += Time.deltaTime;
            yield return null;
        }

        drunk.enabled = false;
    }
}
