using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonType {Leave, Finish};
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
			Globals.gameSetting = -1;
		} else if (myButton == ButtonType.Finish) {
			myFade.GetComponent<DayHandler>().DayEnd();
		} 
	}
}
