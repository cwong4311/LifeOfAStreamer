using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum ButtonType {Leave, Finish, Reset, Exit};
public class StreamButton : MonoBehaviour
{
	public GameObject myCamera;
	public ButtonType myButton;
	public GameObject myFade;

	void Start () {
		Button btn = GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick(){
		if (myButton == ButtonType.Leave) {
			myCamera.GetComponentInChildren<CameraPan>().ResetViews();
			Globals.gameFlag = -1;
		} else if (myButton == ButtonType.Finish) {
			Globals.prevAction = "stream";
			myFade.GetComponent<DayHandler>().StopPostProcessing();
			myFade.GetComponent<DayHandler>().DayEnd();
		} else if (myButton == ButtonType.Reset) {
			myFade.GetComponent<LScreenController>().ResetGame();
		} else if (myButton == ButtonType.Exit) {
            myFade.GetComponent<DayHandler>().LeaveGame();
        }
	}

	public void Leave() {
		myCamera.GetComponentInChildren<CameraPan>().ResetViews();
		Globals.gameFlag = -1;
	}
}
