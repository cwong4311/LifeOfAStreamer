using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedPulse : MonoBehaviour
{
    public Image anger;
    public float min;
    public float max;
    public bool stay = false;

    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Flash());
    }

    IEnumerator Flash() {
        float t = 0.01f;
        bool rising = true;

        while (t > 0) {
            if (t >= 1f) {
                rising = false;
                yield return new WaitForSeconds(0.2f);
            }
            if (rising) {
                t += Time.deltaTime * speed;
            } else {
                t -= Time.deltaTime * speed;
            }
            
            var temp = anger.color;
            temp.a = Mathf.Lerp(min, max, t);
            if (stay && t <= 0.2f) break;
            anger.color = temp;
            yield return null;
        }

        if (!stay) {
            Destroy(this.gameObject);
            yield break;
        }

        t = 0.6f;
        while (t > 0) {
            t -= Time.deltaTime / 60;
            
            var temp = anger.color;
            temp.a = Mathf.Lerp(min, max, t);
            anger.color = temp;
            yield return null;
        }
    }
}
