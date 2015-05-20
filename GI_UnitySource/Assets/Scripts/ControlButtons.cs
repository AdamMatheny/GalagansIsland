using UnityEngine;
using System.Collections;

public class ControlButtons : MonoBehaviour {

	void OnGUI(){

		/*if (GUI.Button (new Rect (Screen.width * .2f, Screen.height * 0.00f, Screen.width * .80f, Screen.height * .25f), "Reset Game")) {
			Application.LoadLevel(0);
			Time.timeScale = 1;
		}*/
		if (GUI.Button (new Rect (Screen.width * .2f, Screen.height * 0.25f, Screen.width * .80f, Screen.height * .25f), "Quit Game")) {
			
			Application.Quit();
		}
		if (GUI.Button (new Rect (Screen.width * .2f, Screen.height * 0.50f, Screen.width * .80f, Screen.height * .25f ), "Continue")) {
			
			Time.timeScale = 1;
			gameObject.SetActive(false);
		}
	}
}