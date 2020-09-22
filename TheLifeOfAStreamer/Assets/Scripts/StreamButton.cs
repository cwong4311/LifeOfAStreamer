using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonType {Leave, Finish, Reset};
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
			Globals.hasStreamed = true;
		} else if (myButton == ButtonType.Finish) {
			Globals.prevAction = "stream";
			myFade.GetComponent<DayHandler>().DayEnd();
		} else if (myButton == ButtonType.Reset) {
			myFade.GetComponent<LScreenController>().ResetGame();
		}
	}
}
