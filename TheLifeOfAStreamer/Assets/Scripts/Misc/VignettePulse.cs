using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class VignettePulse : MonoBehaviour
{
    public PostProcessProfile ppProfile;
    private Vignette vignette;

    private bool running = false;

    // Start is called before the first frame update
    void Awake()
    {
        vignette = ppProfile.GetSetting<Vignette>();
        StartAll();
    }

    public void StartAll() {
        if (!running) {
            running = true;
            StartCoroutine(PulseStart());
        }
    }

    public void StopAll() {
        if (running) {
            running = false;
        }
    }

    IEnumerator PulseStart() {
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
}
