using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class PauseManager : MonoBehaviour 
{

	[SerializeField] private GUIStyle mPauseButtonStyle;
	[SerializeField] private GUIStyle mPauseMenuStyle;

	[SerializeField] private Texture2D mContinueTex;
	[SerializeField] private Texture2D mContinueTexHighlight;
	[SerializeField] private Texture2D mReturnTex;
	[SerializeField] private Texture2D mReturnTexHighlight;
	[SerializeField] private Texture2D mQuitTex;
	[SerializeField] private Texture2D mQuitTexHighlight;


	List<string> mPauseMenuButtonNames = new List<string>();
	
	public int mPauseButtonFocus = 1;
	
	public float mUIFocusTimer = 0f;


	// Use this for initialization
	void Start () 
	{
		mPauseMenuButtonNames.Add("Pause");
		mPauseMenuButtonNames.Add("Continue");
		mPauseMenuButtonNames.Add("ReturnToMenu");
		mPauseMenuButtonNames.Add("QuitGame");

	}
	
	// Update is called once per frame
	void Update () 
	{
//		if(Input.GetAxis ("Vertical") < 0)
//		{
//			Debug.Log("Down button pressed");
//		}
//		else if(Input.GetAxis ("Vertical") > 0)
//		{
//			Debug.Log("Up button pressed");
//		}


		if(mUIFocusTimer > 0f)
		{
			mUIFocusTimer -= 0.01f;
		}

		if (Time.timeScale == 1) 
		{

			//Cursor.visible = false;
		} 
		else if(Time.timeScale == 0)
		{

			//if(Input.GetButtonDown("Vertical"))
			//{
				//For using keyboard/Gamepad to navigate pause menu ~Adam
				if(mUIFocusTimer <= 0f)
				{
					if(Input.GetAxisRaw ("Vertical") < 0 || InputManager.ActiveDevice.DPadDown.WasPressed)
					{
					Debug.Log("Down button pressed");
						switch(mPauseButtonFocus)
						{
						case 1:
							mPauseButtonFocus = 2;
							mUIFocusTimer = 0.2f;
							break;
						case 2:
							mPauseButtonFocus = 3;
							mUIFocusTimer = 0.2f;
							break;
						case 3:
							mPauseButtonFocus = 1;
							mUIFocusTimer = 0.2f;
						break;
						default:
							break;
						}
					}
				else if(Input.GetAxisRaw ("Vertical") > 0 || InputManager.ActiveDevice.DPadUp.WasPressed)
					{
					Debug.Log("Up button pressed");
					switch(mPauseButtonFocus)
						{
						case 1:
							mPauseButtonFocus = 3;
							mUIFocusTimer = 0.2f;
							break;
						case 2:
							mPauseButtonFocus =1;
							mUIFocusTimer = 0.2f;
							break;
						case 3:
							mPauseButtonFocus =2;
							mUIFocusTimer = 0.2f;
							break;
						default:
							break;
						}
					}
				}
			//}
			
			
			if(Input.GetButtonDown("Thrusters") || Input.GetButtonDown("FireGun") || InputManager.ActiveDevice.Action1.WasPressed)
			{
				switch(mPauseButtonFocus)
				{
				case 1:
					UnPause();
					break;
				case 2:
					Time.timeScale = 1;
					Destroy(FindObjectOfType<PlayerShipController>().gameObject);
					Destroy(FindObjectOfType<LevelKillCounter>().gameObject);
					Destroy(FindObjectOfType<ScoreManager>().gameObject);
					Application.LoadLevel(0);
					break;
				case 3:
					Application.Quit();
					break;
				default:
					break;
				}
			}
			//Cursor.visible = true;
		}

		//Press P to Pause/Unpause the game
		if (Input.GetButtonDown ("PauseButton"))// || InputManager.ActiveDevice.MenuWasPressed) 
		{
			
			if(Time.timeScale != 1)
			{
				
				UnPause();
			}
			else 
			{
				Pause();
			}
		}




	}


	void OnGUI()
	{
		mPauseMenuStyle.fontSize = Mathf.RoundToInt(Screen.width*0.01f);
		if(!Application.isMobilePlatform)
		{
			if (Time.timeScale == 0)
			{
				GUI.Box(new Rect(0,0, Screen.width,Screen.height),"");

				mPauseMenuStyle.normal.background = mContinueTex;
				mPauseMenuStyle.hover.background = mContinueTexHighlight;
				mPauseMenuStyle.active.background = mContinueTexHighlight;
				GUI.SetNextControlName("Continue");
				if (GUI.Button (new Rect (Screen.width*0.395f, Screen.height*0.21f, Screen.width*0.21f, Screen.height*0.14f), "", mPauseMenuStyle)) 
				{
					UnPause();
				}
				mPauseMenuStyle.normal.background = mReturnTex;
				mPauseMenuStyle.hover.background = mReturnTexHighlight;
				mPauseMenuStyle.active.background = mReturnTexHighlight;
				GUI.SetNextControlName("ReturnToMenu");
				if (GUI.Button (new Rect (Screen.width*0.395f, Screen.height*0.41f, Screen.width*0.21f, Screen.height*0.14f), "", mPauseMenuStyle)) 
				{
					Time.timeScale = 1;
					Destroy(FindObjectOfType<PlayerShipController>().gameObject);
					Destroy(FindObjectOfType<LevelKillCounter>().gameObject);
					Destroy(FindObjectOfType<ScoreManager>().gameObject);
					Application.LoadLevel(0);
				}
				mPauseMenuStyle.normal.background = mQuitTex;
				mPauseMenuStyle.hover.background = mQuitTexHighlight;
				mPauseMenuStyle.active.background = mQuitTexHighlight;
				GUI.SetNextControlName("QuitGame");
				if (GUI.Button (new Rect (Screen.width*0.395f, Screen.height*0.61f, Screen.width*0.21f, Screen.height*0.14f), "", mPauseMenuStyle)) 
				{
					Application.Quit();
				}
//				GUI.SetNextControlName("Pause");
//				if (GUI.Button (new Rect (Screen.width * .81f, Screen.height * 0.890f, Screen.width * .09f, Screen.height * .1f), "", mPauseButtonStyle)) 
//				{
//					UnPause();
//				}

			}
	//		else
	//		{
	//			GUI.SetNextControlName("Pause");
	//			if (GUI.Button (new Rect (Screen.width * .91f, Screen.height * 0.890f, Screen.width * .09f, Screen.height * .1f), "", mPauseButtonStyle)) 
	//			{
	//				Pause();
	//			}
	//			//For when we had meters attached to the side
	////			GUI.SetNextControlName("Pause");
	////			if (GUI.Button (new Rect (Screen.width * .81f, Screen.height * 0.890f, Screen.width * .09f, Screen.height * .1f), "", mPauseButtonStyle)) 
	////			{
	////				Pause();
	////			}
	//		}



			GUI.FocusControl(mPauseMenuButtonNames[mPauseButtonFocus]);
		}

		//Change layout slightly for mobile portrait view ~Adam
		else
		{
            if (Time.timeScale == 0)
            {

                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");

                mPauseMenuStyle.normal.background = mContinueTex;
                mPauseMenuStyle.hover.background = mContinueTexHighlight;
                mPauseMenuStyle.active.background = mContinueTexHighlight;
                if (GUI.Button(new Rect(Screen.width * 0.2f, Screen.height * 0.21f, Screen.width * 0.6f, Screen.height * 0.14f), "", mPauseMenuStyle))
                {
                    UnPause();
                }
                mPauseMenuStyle.normal.background = mReturnTex;
                mPauseMenuStyle.hover.background = mReturnTexHighlight;
                mPauseMenuStyle.active.background = mReturnTexHighlight;
                if (GUI.Button(new Rect(Screen.width * 0.2f, Screen.height * 0.41f, Screen.width * 0.6f, Screen.height * 0.14f), "", mPauseMenuStyle))
                {
                    Time.timeScale = 1;
                    Destroy(FindObjectOfType<PlayerShipController>().gameObject);
                    Destroy(FindObjectOfType<LevelKillCounter>().gameObject);
                    Destroy(FindObjectOfType<ScoreManager>().gameObject);
                    Application.LoadLevel(0);
                }
                mPauseMenuStyle.normal.background = mQuitTex;
                mPauseMenuStyle.hover.background = mQuitTexHighlight;
                mPauseMenuStyle.active.background = mQuitTexHighlight;
                if (GUI.Button(new Rect(Screen.width * 0.2f, Screen.height * 0.61f, Screen.width * 0.6f, Screen.height * 0.14f), "", mPauseMenuStyle))
                {
                    Application.Quit();
                }

            }
		}
	}//END of OnGUI



	void Pause()
	{
		Time.timeScale = 0;
		mPauseButtonFocus = 1;
	}//END of Pause()
	
	void UnPause()
	{
		Time.timeScale = 1;
	}//END of UnPause

}
