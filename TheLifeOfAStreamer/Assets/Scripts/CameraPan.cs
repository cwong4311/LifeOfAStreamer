using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
    public GameObject destinationObj;
    public GameObject originObj;
    public GameObject myScreen;
    private Transform destinationTrans;
    private Transform originalTrans;
    // Start is called before the first frame update
    void Start()
    {
        destinationTrans = destinationObj.transform;
        originalTrans = originObj.transform;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ChangeViews()
    {
        originalTrans.position = originObj.transform.position;
        originalTrans.rotation = this.transform.rotation;
        StartCoroutine(PanToPosition(this.transform, destinationTrans, 0.7f));
        StartCoroutine(ScreenFlick(true, 0.7f));
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ToggleController(false);
    }

    public void ResetViews()
    {
        StartCoroutine(PanToPosition(this.transform, originalTrans, 0.7f));
        StartCoroutine(ScreenFlick(false, 0.015f));
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(WaitToToggleController(0.7f));
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
    private IEnumerator ScreenFlick(bool flag, float timeToMove)
    {
        var t = 0f;
        while(t < 1)
        {
             t += Time.deltaTime / timeToMove;
             yield return null;
        }
        myScreen.SetActive(flag);
    }

    private void ToggleController(bool flag) {
        MonoBehaviour[] comps = transform.parent.GetComponents<MonoBehaviour>();
        foreach(MonoBehaviour c in comps)
        {
            c.enabled = flag;
        }
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
