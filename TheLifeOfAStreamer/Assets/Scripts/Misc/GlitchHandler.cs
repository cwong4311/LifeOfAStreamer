using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchHandler : MonoBehaviour
{
	public Texture2D displacementMap;
	public Shader Shader;

    private GlitchEffect glitch;
    private float lifetime;

    // Start is called before the first frame update
    void Awake()
    {
        glitch = GameObject.Find("FPSController/FirstPersonCharacter").AddComponent<GlitchEffect>();
        glitch.Shader = Shader;
        glitch.displacementMap = displacementMap;
        glitch.intensity = 1f;
        glitch.flipIntensity = 1f;
        glitch.colorIntensity = 0.5f;
        lifetime = 3f;

        StartCoroutine(AliveDecay());
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

        glitch.enabled = false;
    }
}
