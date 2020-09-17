using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selectable : MonoBehaviour
{
    //public Material baseMat;
    //public Material highlightMat;
    private float highlightTimer = 0f;
    public Material[] baseMat;
    public Material[] highlightMat;
    public string MyMessage = "";
    private GameObject playerMessage;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Selectable";
        gameObject.GetComponent<MeshRenderer>().materials = baseMat;
        playerMessage = GameObject.Find("/PlayerCanvas/PlayerMessage");
    }

    // Update is called once per frame
    void Update()
    {
        if (highlightTimer > 0) highlightTimer -= Time.deltaTime;
        if (highlightTimer <= 0) {
            gameObject.GetComponent<MeshRenderer>().materials = baseMat;
        } else {
            gameObject.GetComponent<MeshRenderer>().materials = highlightMat;
        }
    }

    public void TriggerHighlight() {
        highlightTimer = 0.1f;
    }

    public void OnSelect() {
        playerMessage.GetComponent<TextHandler>().SetText(MyMessage);
    }

    public void ChangeViews(Transform destination) {
        playerMessage.GetComponent<TextHandler>().SetText(MyMessage);
    }
}
