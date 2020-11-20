using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticHandler : MonoBehaviour
{
    public Image Grain;
    public float min;
    public float max;

    public float speed;
    public float duration;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Flash());
    }

    public void SetSpeedDur(float speed, float duration) {
        this.speed = speed;
        this.duration = duration;
    }

    IEnumerator Flash() {
        float t = 0.01f;
        bool rising = true;

        SoundHandler sound = GameObject.Find("Sound").GetComponent<SoundHandler>();
        int soundIndex = sound.PlayAudio(5, false);

        while (t > 0) {
            if (t >= 1f) {
                rising = false;
                yield return new WaitForSeconds(duration);
            }
            if (rising) {
                t += Time.deltaTime * speed;
                sound.ChangeVolume(soundIndex, t + 0.2f);
            } else {
                t -= Time.deltaTime * speed;
                sound.ChangeVolume(soundIndex, t);
            }

            var temp = Grain.color;
            temp.a = Mathf.Lerp(min, max, t);
            Grain.color = temp;
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
