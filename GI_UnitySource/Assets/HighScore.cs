using UnityEngine;
using System.Collections;

public class HighScore : MonoBehaviour {

	public int startScore = 0;

	void Update(){

		if (Input.GetKeyDown (KeyCode.Space)) {

			startScore += 1;
			StoreHighscore(startScore);
		}
	}

	void StoreHighscore(int newHighscore)
	{
		int oldHighscore = PlayerPrefs.GetInt("highscore", 0);    
		if(newHighscore > oldHighscore)
			PlayerPrefs.SetInt("highscore", newHighscore);
	}
	
	void OnGUI()
	{

		GUI.Box (new Rect (Screen.width * 0.01f, Screen.height * 0.01f, Screen.width * 0.2f, Screen.height * 0.2f), "Score: " + PlayerPrefs.GetInt ("highscore", 0));

	}
}
