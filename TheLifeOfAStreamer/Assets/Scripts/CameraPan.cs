using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
    public GameObject destinationObj;
    public GameObject phoneDestinationObj;
    public GameObject originObj;
    public GameObject myScreen;
    public GameObject phoneScreen;
    private Transform destinationTrans;
    private Transform originalTrans;
    private Transform phoneDestinationTrans;
    private bool isFocused = false;
    // Start is called before the first frame update
    void Start()
    {
        destinationTrans = destinationObj.transform;
        originalTrans = originObj.transform;
        phoneDestinationTrans = phoneDestinationObj.transform;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ChangeViews(string destinationFlag)
    {
        GetComponent<Selector>().enabled = false;
        originalTrans.position = originObj.transform.position;
        originalTrans.rotation = this.transform.rotation;
        if (destinationFlag == "phone") {
            StartCoroutine(PanToPosition(this.transform, phoneDestinationTrans, 0.7f));
            StartCoroutine(ScreenFlick(true, 0.7f, 1));
        }
        else {
            StartCoroutine(PanToPosition(this.transform, destinationTrans, 0.7f));
            StartCoroutine(ScreenFlick(true, 0.7f, 0));
        }
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ToggleController(false);
    }

    public void ResetViews()
    {
        StartCoroutine(PanToPosition(this.transform, originalTrans, 0.7f));
        StartCoroutine(ScreenFlick(false, 0.015f, -1));
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(WaitToToggleController(0.7f));
        GetComponent<Selector>().enabled = true;
    }
    public IEnumerator PanToPosition(Transform start, Transform destination, float timeToMove)
    {
      var currentPos = start.position;
      var destPos = destination.position;

      var currentRot = start.rotation;
      var destRot = destination.rotation;
      var t = 0f;
      while(t < 1)
      {
             t += Time.deltaTime / timeToMove;
             transform.position = Vector3.Lerp(currentPos, destPos, t);
             transform.rotation = Quaternion.Slerp(currentRot, destRot, t);
             yield return null;
      }
    }
    private IEnumerator ScreenFlick(bool flag, float timeToMove, int screenType)
    {
        var t = 0f;
        while(t < 1)
        {
             t += Time.deltaTime / timeToMove;
             yield return null;
        }
        switch(screenType) {
            default:
            case 0:
                myScreen.SetActive(flag);
                break;
            case 1:
                phoneScreen.SetActive(flag);
                break;
            case -1:
                myScreen.SetActive(flag);
                phoneScreen.SetActive(flag);
                break;
        }
    }

    public void ToggleController(bool flag) {
        MonoBehaviour[] comps = transform.parent.GetComponents<MonoBehaviour>();
        foreach(MonoBehaviour c in comps)
        {
            c.enabled = flag;
        }
    }

    public bool getFocused() {
        return isFocused;
    }

    private IEnumerator WaitToToggleController(float delay) {
        var t = 0f;
        while(t < 1)
        {
             t += Time.deltaTime / delay;
             yield return null;
        }

        ToggleController(true);
    }
}
