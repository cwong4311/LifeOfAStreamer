using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptAnimation : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
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
        yield return null;
    }

    IEnumerator HidePrompts() {
        anim.SetBool("isShowing", false);
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
}
