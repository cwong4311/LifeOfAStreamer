using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchHandler : MonoBehaviour
{
	public Texture2D displacementMap;
	public Shader Shader;

    private GlitchEffect glitch;
    private float lifetime;

    private SoundHandler sound;
    private int soundFile;

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

        sound = GameObject.Find("Sound").GetComponent<SoundHandler>();

        StartCoroutine(AliveDecay());
    }

    public void SetLife(float life) {
        lifetime = life;
    }

    IEnumerator AliveDecay() {
        soundFile = sound.PlayAudio(10, true);
        sound.ToggleDistortion(true);

        float counter = 0f;
        while (counter < lifetime) {
            sound.ChangeVolume(soundFile, Mathf.Min((lifetime - counter) / lifetime, 0.3f));
            counter += Time.deltaTime;
            yield return null;
        }

        glitch.enabled = false;
    }
}
