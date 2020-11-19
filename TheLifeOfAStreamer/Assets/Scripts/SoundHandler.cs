using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    public GameObject[] audioList;
    private ArrayList activeList;
    // Start is called before the first frame update
    void Start()
    {
        activeList = new ArrayList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int PlayAudio(int index, bool looping) {
        GameObject myAudio = Instantiate(audioList[index], transform);
        myAudio.GetComponent<AudioSource>().loop = looping;
        myAudio.GetComponent<AudioSource>().Play();
        int listDex = activeList.Add(myAudio);
        return listDex;
    }

    public void StopAudio(int index) {
        StartCoroutine(FadeOut(((GameObject)activeList[index]).GetComponent<AudioSource>(), 1f));
    }

    public void ChangeVolume(int index, float vol) {
        ((GameObject)activeList[index]).GetComponent<AudioSource>().volume = vol;
    }

    public void StopAll() {
        while (activeList.Count > 0) {
            GameObject item = (GameObject)activeList[0];
            StartCoroutine(FadeOut(item.GetComponent<AudioSource>(), 1f));
            activeList.RemoveAt(0);
        }
    }

    IEnumerator FadeOut(AudioSource audioSource, float FadeTime) {
        float startVolume = audioSource.volume;
 
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
 
            yield return null;
        }
 
        audioSource.Stop ();
        audioSource.volume = startVolume;
    }
}
