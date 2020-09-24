using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    public enum MenuButtonType {
        None,
        Genre,
        Platform,
        MultimediaInput
    }

    public Material[] baseColour;
    public Material[] highlightedColour;
    public MenuButtonType buttonType = MenuButtonType.None;
    public GameObject MenuController;
    public int myGameType = 0;
    public int myPlatformType = 0;
    private Camera myGameCam;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<MeshRenderer>().materials = baseColour;

        if (this.myGameCam == null) {
            this.myGameCam = findCamera();
        }

        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(this.myGameCam.ScreenPointToRay(Input.mousePosition), out hitInfo)) {
            if (hitInfo.transform.name == this.transform.name) {
                gameObject.GetComponent<MeshRenderer>().materials = highlightedColour;

                if (Input.GetMouseButtonDown(0)) {
                    if (buttonType == MenuButtonType.Genre) {
                        MenuController.GetComponent<MenuController>().SetGame(myGameType);
                    } else if (buttonType == MenuButtonType.Platform) {
                        MenuController.GetComponent<MenuController>().SetPlatform(myPlatformType);
                    } else if (buttonType == MenuButtonType.MultimediaInput) {
                        bool inputIsEnabled = false;
                        if (myGameType == 0) inputIsEnabled = true;
                        MenuController.GetComponent<MenuController>().SetWebcamAndMic(inputIsEnabled, inputIsEnabled);
                    }
                }
            }
        }
    }

    private Camera findCamera() {
        foreach (Camera c in Camera.allCameras) {
            if (c.gameObject.name == "Game Camera") {
                return c;
            }
        }

        return Camera.main;
    }
}
