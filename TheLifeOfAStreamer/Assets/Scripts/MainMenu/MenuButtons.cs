using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    public enum MenuButtonType {
        None,
        Genre,
        Platform,
        MultimediaInput
    }

    public Color baseColour;
    public Color highlightedColour;
    public MenuButtonType buttonType = MenuButtonType.None;
    public GameObject MenuController;
    public int myGameType = 0;
    public int myPlatformType = 0;
    private Camera myGameCam;

    private bool added = false;
    // Start is called before the first frame update
    void Start()
    {
		if (!added) {
            GetComponent<Button>().onClick.AddListener(TaskOnClick);
            added = true;
        }
    }

    void OnEnable() {
		if (!added) {
            GetComponent<Button>().onClick.AddListener(TaskOnClick);
            added = true;
        }
    }

    void TaskOnClick() {
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
