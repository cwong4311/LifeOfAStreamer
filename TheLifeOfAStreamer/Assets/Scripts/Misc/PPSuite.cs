using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityStandardAssets.Characters.FirstPerson;

public class PPSuite : MonoBehaviour
{    //remember to drag and drop your scriptable object into this field in the inspector...
    public PostProcessProfile ppProfile;
    public LayerMask IgnoreMe;
    public Material drunkMat;

    private CameraPan camera;
    private Transform player;
    private Vignette vignette;
    private LensDistortion lens;
    private Drunk drunk;

    private SoundHandler sound;
    private int mySound;

    private bool isActive = true;
    private bool running = false;
    private bool firstRun = true;

    void Awake() {
        player = GameObject.Find("FPSController/FirstPersonCharacter").transform;
        camera = GameObject.Find("FPSController/FirstPersonCharacter").GetComponent<CameraPan>();
        vignette = ppProfile.GetSetting<Vignette>();
        lens = ppProfile.GetSetting<LensDistortion>();

        drunk = GameObject.Find("FPSController/FirstPersonCharacter").AddComponent<Drunk>();
        drunk.material = drunkMat;
        drunk.enabled = false;

        sound = GameObject.Find("Sound").GetComponent<SoundHandler>();
        mySound = sound.PlayAudio(3, true);
        sound.ChangeVolume(mySound, 0f);
    }

    void Update()
    {
        if (camera.isPanning) isActive = false;

        if (isActive) {
            RaycastHit hit;
            Debug.DrawRay(player.position, player.forward * 500f);
            if(Physics.Raycast (player.position, player.forward, out hit, 500f, ~IgnoreMe)) {
                if (hit.collider.tag == "Selectable") {
                    switch(hit.collider.name) {
                        case "Bed":
                            StopAll();
                            break;
                        case "Table":
                            if (firstRun) {
                                firstRun = false;
                                StartAll();
                            }
                            break;
                        case "Door":
                            if (firstRun) {
                                firstRun = false;
                                StartAll();
                            }
                            break;
                    }
                } else {
                    if (!firstRun) StartAll();
                }
            } else {
                if (!firstRun) StartAll();
            }
        } else {
            StopAll();

            GameObject.FindObjectOfType<FirstPersonController>().m_WalkSpeed = 1.3f;
            GameObject.FindObjectOfType<FirstPersonController>().m_RunSpeed = 2f;
            GameObject.FindObjectOfType<FirstPersonController>().m_StepInterval = 1.5f;
        }
    }

    void StartAll() {
        if (!running) {
            running = true;
            drunk.enabled = true;
            StartCoroutine(VignettePulse());
            StartCoroutine(LensStretch());

            sound.ChangeVolume(mySound, 0.1f);
            sound.ResumeAudio(mySound);
        }
    }

    void StopAll() {
        if (running) {
            running = false;
            drunk.enabled = false;

            sound.StopAudio(mySound);
        }
    }

    IEnumerator VignettePulse() {
        vignette.active = true;

        float t = 0;
        float direction = 1;
        float intensity = 0.5f;

        while (running) {
            t += direction * Time.deltaTime / 2f;
            if (t > 1f) {
                direction = -1;
                t = 1f;
            } else if (t <= 0f) {
                direction = 1;
                t = 0f;
            }

            intensity = Mathf.Lerp(0.4f, 0.8f, t);
            
            vignette.intensity.value = intensity;
            yield return null;
        }

        t = 1f;
        float temp = intensity;
        while (t > 0) {
            t -= Time.deltaTime;

            intensity = Mathf.Lerp(0f, temp, t);
            vignette.intensity.value = intensity;
        }

        vignette.active = false;
    }

    IEnumerator LensStretch() {
        lens.active = true;

        float t = 0;
        float intensity = 0f;

        while (intensity <= 1f) {
            t += Time.deltaTime;
            intensity = Mathf.Lerp(-30f, -90f, t);
            lens.intensity.value = intensity;

            if (!running) break;

            yield return null;
        }

        while (running) {
            yield return null;
        }

        lens.active = false;
    }
}
