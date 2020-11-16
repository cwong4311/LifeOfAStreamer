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

    public GameObject confirmPrompt;
    private Selectable tempSelect;
    private bool isPrompt = false;

    public GameObject Fader;

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
                        tempSelect = myObject;
                        switch(hit.collider.name) {
                            default:
                            case "Door":
                                if (!isPrompt) {
                                    ToggleController(false);
                                    GoOutPrompt();
                                    isPrompt = true;
                                }
                                break;
                            case "Bed":
                                if (!isPrompt) {
                                    ToggleController(false);
                                    BedPrompt();
                                    isPrompt = true;
                                }
                                break;
                        }
                    } else if (myObject.objectType == Selectable.ObjectType.Text) {
                        myObject.TextSelect();
                    }
                }
            }
        }

        running = false;

        yield return null;
    }

    /*
    public void DoorPrompt() {
        confirmPrompt.SetActive(true);
        confirmPrompt.transform.Find("Door").gameObject.SetActive(true);
    }
    */

    public void GoOutPrompt() {
        confirmPrompt.SetActive(true);
        //confirmPrompt.transform.Find("Door").gameObject.SetActive(false;
        confirmPrompt.transform.Find("GoOut").gameObject.SetActive(true);
    }

    public void ExitPrompt() {
        confirmPrompt.transform.Find("Door").gameObject.SetActive(false);
        confirmPrompt.transform.Find("Exit").gameObject.SetActive(true);
    }

    public void BedPrompt() {
        confirmPrompt.SetActive(true);
        confirmPrompt.transform.Find("Bed").gameObject.SetActive(true);
    }



    public void GoOut(bool flag) {
        if (flag) {
            if (!tempSelect.InteractSelect()) {
                ToggleController(true);
                tempSelect = null;
                isPrompt = false;
            }
        } else {
            ToggleController(true);
            tempSelect = null;
            isPrompt = false;
        }
        confirmPrompt.transform.Find("GoOut").gameObject.SetActive(false);
        confirmPrompt.SetActive(false);
    }

    public void Exit(bool flag) {
        if (flag) {
            Fader.GetComponent<DayHandler>().LeaveGame();
        } else {
            ToggleController(true);
            tempSelect = null;
            isPrompt = false;
        }
        confirmPrompt.transform.Find("Exit").gameObject.SetActive(false);
        confirmPrompt.SetActive(false);
    }

    public void Sleep(bool flag) {
        if (flag) {
            if (!tempSelect.InteractSelect()) {
                ToggleController(true);
                tempSelect = null;
                isPrompt = false;
            }
        } else {
            ToggleController(true);
            tempSelect = null;
            isPrompt = false;
        }
        confirmPrompt.transform.Find("Bed").gameObject.SetActive(false);
        confirmPrompt.SetActive(false);
    }

    
    private void ToggleController(bool flag) {
        if (!flag) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GetComponent<CameraPan>().ToggleController(false);
        } else {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            GetComponent<CameraPan>().ToggleController(true);
        }
    }
}
