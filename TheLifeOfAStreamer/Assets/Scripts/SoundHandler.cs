using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    public GameObject[] audioList;
    private ArrayList activeList;

    private Coroutine fadeFX = null;
    private bool distortionOn = false;
    // Start is called before the first frame update
    void Start()
    {
        activeList = new ArrayList();
    }

    // Update is called once per frame
    void Update()
    {
        if (distortionOn) {
            for (int i = 0; i < activeList.Count; i++) {
                GameObject item = (GameObject)(activeList[i]);
                if (item.name.Contains("Dark") || item.name.Contains("Sunny") || item.name.Contains("Music")) {
                    item.GetComponent<AudioChorusFilter>().enabled = true;
                    float newPitch = Random.Range(-3f, 3f);
                    if (newPitch < 0 && newPitch > -1) {newPitch = -1;} else if (newPitch > 0 && newPitch < 1) {newPitch = 1;}
                    item.GetComponent<AudioSource>().pitch = newPitch;
                }
            }  
        }
    }

    public int PlayAudio(int index, bool looping) {
        GameObject myAudio = Instantiate(audioList[index], transform);
        myAudio.GetComponent<AudioSource>().loop = looping;
        myAudio.GetComponent<AudioSource>().Play();
        int listDex = activeList.Add(myAudio);
        return listDex;
    }

    public void StopAudio(int index, float duration) {
        if (index > activeList.Count) return;
        fadeFX = StartCoroutine(FadeOut(((GameObject)activeList[index]).GetComponent<AudioSource>(), 1f));
    }

    public void StopAudio(int index) {
        if (index > activeList.Count) return;
        StopAudio(index, 1f);
    }

    public void ResumeAudio(int index) {
        if (index > activeList.Count) return;
        if (fadeFX != null) StopCoroutine(fadeFX);
        ((GameObject)activeList[index]).GetComponent<AudioSource>().Play();
    }

    public void ChangeVolume(int index, float vol) {
        if (index > activeList.Count) return;
        ((GameObject)activeList[index]).GetComponent<AudioSource>().volume = vol;
    }

    public bool IsPlaying(int index) {
        if (index > activeList.Count) return false;
        return ((GameObject)activeList[index]).GetComponent<AudioSource>().isPlaying;
    }

    public void StopAll() {
        while (activeList.Count > 0) {
            GameObject item = (GameObject)activeList[0];
            StartCoroutine(FadeOut(item.GetComponent<AudioSource>(), 1f));
            activeList.RemoveAt(0);
        }
    }

    public void FadeInOutNoStop(int index, float FadeTime, float start, float end) {
        if (index > activeList.Count) return;
        StartCoroutine(Fade(((GameObject)activeList[index]).GetComponent<AudioSource>(), FadeTime, start, end));  
    }

    public void ToggleDistortion(bool flag) {
        distortionOn = flag;
        
        if (!flag) {
            for (int i = 0; i < activeList.Count; i++) {
                GameObject item = (GameObject)(activeList[i]);
                if (item.name.Contains("Dark") || item.name.Contains("Sunny") || item.name.Contains("Music")) {
                    item.GetComponent<AudioChorusFilter>().enabled = false;
                    item.GetComponent<AudioSource>().pitch = 1;
                }
            }  
        }
    }

    IEnumerator FadeOut(AudioSource audioSource, float FadeTime) {
        float startVolume = audioSource.volume;
 
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
 
            yield return null;
        }
 
        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    IEnumerator Fade(AudioSource audioSource, float FadeTime, float start, float end) {
        audioSource.volume = start;
        if (start < end) {
            while (audioSource.volume < end) {
                audioSource.volume += end * Time.deltaTime / FadeTime;
    
                yield return null;
            }
        } else {
            while (audioSource.volume > end) {
                audioSource.volume -= start * Time.deltaTime / FadeTime;
    
                yield return null;
            }
        }
    }
}
