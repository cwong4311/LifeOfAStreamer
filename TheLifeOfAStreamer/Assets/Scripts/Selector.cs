using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    public LayerMask IgnoreMe;
    public Material outlineColour;
    public float interactionDistance = 1.7f;
    public GameObject secondCamera;
    private GameObject crosshair;
    private float crosshairPersist = 0f;

    bool running = false;
    
    // Start is called before the first frame update
    void Start()
    {
        crosshair = GameObject.Find("PlayerCanvas/Crosshair");
    }

    // Update is called once per frame
    void Update () {
        if (!running) {
            StartCoroutine(TrackInteract());
        }

        if (crosshairPersist > 0f) {
            crosshair.SetActive(true);
            crosshairPersist -= Time.deltaTime;
        }
        else {
            crosshair.SetActive(false);
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
        if(Physics.Raycast (transform.position, transform.forward, out hit, interactionDistance, ~IgnoreMe)) {
            if (hit.collider.tag == "Selectable") {
                crosshairPersist = 0.1f;
                if (Input.GetMouseButtonDown(0)) {
                    Selectable myObject = hit.transform.gameObject.GetComponent<Selectable>();
                    if (myObject.objectType == Selectable.ObjectType.Stream) {
                        crosshairPersist = 0f;
                        try {
                            switch(hit.collider.name) {
                                default:
                                case "Table":
                                    GetComponent<CameraPan>().ChangeViews("pc");
                                    break;
                                case "IPhoneX":
                                    GetComponent<CameraPan>().ChangeViews("phone");
                                    break;
                            }
                        } catch (Exception e) {
                            Debug.Log("Selector Error");
                            // Ignore
                        }
                    } else if (myObject.objectType == Selectable.ObjectType.Interact) {
                        myObject.InteractSelect();
                    } else if (myObject.objectType == Selectable.ObjectType.Text) {
                        myObject.TextSelect();
                    }
                }
            }
        }

        running = false;

        yield return null;
    }
}
