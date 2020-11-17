using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkHandler : MonoBehaviour
{
    public Material drunkMat;

    private Drunk drunk;
    private float lifetime;
    // Start is called before the first frame update
    void Awake()
    {
        drunk = GameObject.Find("FPSController/FirstPersonCharacter").AddComponent<Drunk>();
        drunk.material = drunkMat;
        lifetime = 10f;
        StartCoroutine(AliveDecay());
    }

    void Update() {
        if (GameObject.Find("FPSController").GetComponent<MonoBehaviour>().enabled) {
            if (!drunk.enabled) drunk.enabled = true;
        } else {
            if (drunk.enabled) drunk.enabled = false;
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
