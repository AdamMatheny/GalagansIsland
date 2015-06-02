using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenuGUIFocusController : MonoBehaviour 
{


	public List<string> mMainMenuButtonNames = new List<string>();
	
	public int mMainMenuButtonFocus = 0;
	
	public float mUIFocusTimer = 0f;
	public ResetScore mScoreResetter;
	public GetSome mGameStarter;


	void Start()
	{
		mMainMenuButtonNames.Add("InsertCoin");
		mMainMenuButtonNames.Add("QuitGame");
		mMainMenuButtonNames.Add("ResetStart");
		mMainMenuButtonNames.Add("ResetAsk");
		mMainMenuButtonNames.Add("ResetCancel");
		mMainMenuButtonNames.Add("ResetConfirm");
	}


	// Update is called once per frame
	void Update () 
	{
		if(mUIFocusTimer > 0f)
		{
			mUIFocusTimer -= Time.deltaTime;
		}

		if (Input.GetButtonDown("Thrusters") || Input.GetButtonDown("FireGun"))
		{
			switch(mMainMenuButtonFocus)
			{
			case 0:
				if(mGameStarter.isActiveAndEnabled == true)
				{
					mGameStarter.mSuperLaser.SetActive(true);
				}
				break;
			case 1:
				if(mGameStarter.isActiveAndEnabled == true)
				{
					Application.Quit();
				}
				break;
			case 2:
				mScoreResetter.StartScoreReset();
				break;
			case 4:
				mScoreResetter.CancelScoreReset();
				break;
			case 5:
				mScoreResetter.ConfirmScoreReset();
				break;
			default:
				break;

			}
		}


	}


	void OnGUI()
	{

		//Move from insert coin to quit game ~Adam
		if(Input.GetAxis ("Horizontal") > 0f && mUIFocusTimer <= 0f && mMainMenuButtonFocus == 0 && mGameStarter.isActiveAndEnabled == true)
		{
			mMainMenuButtonFocus = 1;
			mUIFocusTimer = 0.2f;
		}


		//Move from score quit game to insert coin ~Adam
		else if(Input.GetAxis ("Horizontal") < 0f && mUIFocusTimer <= 0f && mMainMenuButtonFocus == 1)
		{
			mMainMenuButtonFocus = 0;
			mUIFocusTimer = 0.2f;
		}



		//Move from focusing on "Insert Coin" to focusing on score reset ~Adam
		else if(Input.GetAxis ("Horizontal") < 0f && mUIFocusTimer<= 0f && mMainMenuButtonFocus == 0)
		{
			mMainMenuButtonFocus = 2;
			mUIFocusTimer = 0.2f;
		}
		
		//Move from score reset to insert coin ~Adam
		else if(Input.GetAxis ("Horizontal") > 0f && mUIFocusTimer <= 0f && mMainMenuButtonFocus == 2 && mGameStarter.isActiveAndEnabled == true)
		{
			mMainMenuButtonFocus = 0;
			mUIFocusTimer = 0.2f;
		}


		//Toggle between canceling/confirming the high score reset ~Adam
		else if(Input.GetAxis ("Horizontal") != 0f && mUIFocusTimer<= 0f && (mMainMenuButtonFocus == 5 || mMainMenuButtonFocus == 4))
		{
			if(mMainMenuButtonFocus == 4)
			{
				mMainMenuButtonFocus = 5;
				mUIFocusTimer = 0.2f;
			}
			else if(mMainMenuButtonFocus == 5)
			{
				mMainMenuButtonFocus = 4;
				mUIFocusTimer = 0.2f;
			}
			
		}

	}

}
