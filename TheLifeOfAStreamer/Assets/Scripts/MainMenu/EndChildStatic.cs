using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndChildStatic : MonoBehaviour
{
    public GameObject[] items;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        foreach (GameObject i in items)
        {
            i.SetActive(false);
        }

        StartCoroutine(SlowShow());
    }

    IEnumerator SlowShow()
    {

        foreach (GameObject i in items)
        {
            i.SetActive(true);
            Text myText = i.GetComponent<Text>();

            Color endColor;
            if (myText.name == "ButtonText")
            {
                i.GetComponentInParent<Button>().interactable = true;
                endColor = new Color(1f, 0f, 0f, 1f);
            }
            else
            {
                endColor = new Color(1f, 1f, 1f, 1f);
            }

            float t = 0;
            while (t < 1)
            {
                // Now the loop will execute on every end of frame until the condition is true
                myText.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 0.0f), endColor, t);
                t += Time.deltaTime / 2;
                yield return new WaitForEndOfFrame(); // So that I return something at least.
            }

            yield return new WaitForSeconds(1f);
        }

        Time.timeScale = 0f;
    }
}
