using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptAnimation : MonoBehaviour
{
    Animator anim;
    SoundHandler sound;
    private int soundFile;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        sound = GameObject.Find("Sound").GetComponent<SoundHandler>();
        soundFile = sound.PlayAudio(6, false);
        sound.ChangeVolume(soundFile, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Toggle(bool flag) {
        switch(flag) {
            case true:
                gameObject.SetActive(true);
                StartCoroutine(ShowPrompts());
                break;
            default:
            case false:
                StartCoroutine(HidePrompts());
                break;
        }
    }

    IEnumerator ShowPrompts() {
        anim.SetBool("isShowing", true);
        yield return new WaitForSeconds(0.1f);
        sound.ChangeVolume(soundFile, 0.6f);
        sound.ResumeAudio(soundFile);
        
    }

    IEnumerator HidePrompts() {
        anim.SetBool("isShowing", false);
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
}
