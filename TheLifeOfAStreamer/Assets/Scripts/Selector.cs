using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    public Material outlineColour;
    public float interactionDistance = 1.7f;
    public GameObject secondCamera;

    bool running = false;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update () {
        if (!running) {
            StartCoroutine(TrackInteract());
        }
        if (Input.GetKeyDown("f"))
        {
            Debug.Log("Days: " + Globals.days + ", Pop: " + Globals.popularity + ", Att: " + Globals.attitude + ", Score: " + Globals.gameScore);
        }
    }
    private IEnumerator TrackInteract() {
        running = true;

        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * interactionDistance);
        if(Physics.Raycast (transform.position, transform.forward, out hit, interactionDistance)) {
            if (hit.collider.tag == "Selectable") {
                hit.transform.gameObject.GetComponent<Selectable>().TriggerHighlight();
                if (Input.GetMouseButtonDown(0)) {
                    Selectable myObject = hit.transform.gameObject.GetComponent<Selectable>();
                    if (myObject.objectType == Selectable.ObjectType.Stream) {
                        GetComponentInChildren<CameraPan>().ChangeViews();
                    } else if (myObject.objectType == Selectable.ObjectType.Interact) {
                        hit.transform.gameObject.GetComponent<Selectable>().InteractSelect();
                    } else if (myObject.objectType == Selectable.ObjectType.Text) {
                        hit.transform.gameObject.GetComponent<Selectable>().TextSelect();
                    }
                }
            }
        }

        running = false;

        yield return null;
    }
}
