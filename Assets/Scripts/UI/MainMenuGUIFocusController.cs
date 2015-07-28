using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class MainMenuGUIFocusController : MonoBehaviour 
{


	public List<string> mMainMenuButtonNames = new List<string>();
	
	public int mMainMenuButtonFocus = 0;
	
	public float mUIFocusTimer = 0f;
	public ResetScore mScoreResetter;
	public GetSome mGameStarter;

	//For opening/closing the volume control menu ~Adam
	VolumeControlSliders mVolumeMenu;

	void Start()
	{
		mMainMenuButtonNames.Add("InsertCoin");//0
		mMainMenuButtonNames.Add("QuitGame");//1
		//mMainMenuButtonNames.Add("ResetStart");//2
		//mMainMenuButtonNames.Add("ResetAsk");//3
		//mMainMenuButtonNames.Add("ResetCancel");//4
		//mMainMenuButtonNames.Add("ResetConfirm");//5
		mMainMenuButtonNames.Add("StartCoOp");//6
		mMainMenuButtonNames.Add("Options");//7
		mVolumeMenu = FindObjectOfType<VolumeControlSliders>();
	}


	// Update is called once per frame
	void Update () 
	{
		if(mUIFocusTimer > 0f)
		{
			mUIFocusTimer -= Time.deltaTime;
		}

		if ( (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Thrusters") || Input.GetButtonDown("FireGun") || (InputManager.ActiveDevice.Action1.IsPressed && mUIFocusTimer<=0f)) && !mVolumeMenu.mMenuOpen)
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
			/*case 2:
				mScoreResetter.StartScoreReset();
				break;
			case 4:
				mScoreResetter.CancelScoreReset();
				break;
			case 5:
				mScoreResetter.ConfirmScoreReset();
				break;*/
			case 6:
				if(mGameStarter.isActiveAndEnabled == true)
				{
					FindObjectOfType<CoOpSelector>().mCoOpEnabled = true;
					Destroy (mGameStarter.mSuperLaser);
					mGameStarter.mCoOpLaser.SetActive(true);
				}
				break;
			case 7:
				if(mUIFocusTimer<=0f)
				{
					Debug.Log("Opening Volume menu"+mUIFocusTimer);
					mVolumeMenu.mMenuOpen = true;
				}
				break;
			default:
				break;
			}
			if(InputManager.ActiveDevice.Action1.IsPressed)
			{
				mUIFocusTimer+=0.2f;
			}
		}

	}


	void OnGUI()
	{
		if(!mVolumeMenu.mMenuOpen)
		{


			//Move from insert coin to options ~Adam
			if((Input.GetAxis ("Horizontal") > 0f || InputManager.ActiveDevice.DPadRight.IsPressed) && mUIFocusTimer <= 0f && mMainMenuButtonFocus == 0 && mGameStarter.isActiveAndEnabled == true)
			{
				mMainMenuButtonFocus = 1;
				mUIFocusTimer = 0.2f;
			}

			if((Input.GetAxis ("Horizontal") > 0f || InputManager.ActiveDevice.DPadRight.IsPressed) && mUIFocusTimer <= 0f && mMainMenuButtonFocus == 7 && mGameStarter.isActiveAndEnabled == true)
			{
				mMainMenuButtonFocus = 0;
				mUIFocusTimer = 0.2f;
			}

			if((Input.GetAxis ("Horizontal") < 0f || InputManager.ActiveDevice.DPadRight.IsPressed) && mUIFocusTimer <= 0f && mMainMenuButtonFocus == 0 && mGameStarter.isActiveAndEnabled == true)
			{
				mMainMenuButtonFocus = 7;
				mUIFocusTimer = 0.2f;
			}

			//Move from options to insert coin ~Adam
			else if((Input.GetAxis ("Horizontal") < 0f || InputManager.ActiveDevice.DPadLeft.IsPressed) && mUIFocusTimer <= 0f && mMainMenuButtonFocus == 7)
			{
				mMainMenuButtonFocus = 1;
				mUIFocusTimer = 0.2f;
			}

			//Move from quit game to insert coin ~Adam
			else if((Input.GetAxis ("Horizontal") < 0f || InputManager.ActiveDevice.DPadLeft.IsPressed) && mUIFocusTimer <= 0f && mMainMenuButtonFocus == 1)
			{
				mMainMenuButtonFocus = 0;
				mUIFocusTimer = 0.2f;
			}


			//Move from focusing on Options to focusing on Quit Game ~Adam
			else if( ( (Input.GetAxis ("Vertical") < 0f || InputManager.ActiveDevice.DPadDown.IsPressed) && mUIFocusTimer<= 0f && mMainMenuButtonFocus == 7))
			{
				mMainMenuButtonFocus = 6;
				mUIFocusTimer = 0.2f;
			}
			//Move from focusing on Quit Game to focusing on Options ~Adam
			else if((Input.GetAxis ("Vertical") > 0f || InputManager.ActiveDevice.DPadUp.IsPressed) && mUIFocusTimer<= 0f && mMainMenuButtonFocus == 1)
			{
				mMainMenuButtonFocus = 1;
				mUIFocusTimer = 0.2f;
			}

			//Move from focusing on "Insert Coin" to focusing on the CoOp Start ~Adam
			else if( ( (Input.GetAxis ("Vertical") < 0f || InputManager.ActiveDevice.DPadDown.IsPressed) && mUIFocusTimer<= 0f && mMainMenuButtonFocus == 0) && !Application.isMobilePlatform )
			{
				mMainMenuButtonFocus = 6;
				mUIFocusTimer = 0.2f;
			}
			//Move from focusing on CoOpStart to focusing on the "Insert Coin" ~Adam
			else if((Input.GetAxis ("Vertical") > 0f || InputManager.ActiveDevice.DPadUp.IsPressed) && mUIFocusTimer<= 0f && mMainMenuButtonFocus == 6)
			{
				mMainMenuButtonFocus = 0;
				mUIFocusTimer = 0.2f;
			}


			//Move from focusing on "Insert Coin" to focusing on score reset ~Adam
			/*else if((Input.GetAxis ("Horizontal") < 0f || InputManager.ActiveDevice.DPadLeft.IsPressed) && mUIFocusTimer<= 0f && mMainMenuButtonFocus == 0)
			{
				mMainMenuButtonFocus = 2;
				mUIFocusTimer = 0.2f;
			}*/

			//Move from score reset to insert coin ~Adam
			/*else if((Input.GetAxis ("Horizontal") > 0f && mUIFocusTimer <= 0f || InputManager.ActiveDevice.DPadRight.IsPressed) && mMainMenuButtonFocus == 2 && mGameStarter.isActiveAndEnabled == true)
			{
				mMainMenuButtonFocus = 0;
				mUIFocusTimer = 0.2f;
			}*/

			//Toggle between canceling/confirming the high score reset ~Adam
			/*else if((Input.GetAxis ("Horizontal") != 0f || InputManager.ActiveDevice.DPadRight.IsPressed || InputManager.ActiveDevice.DPadLeft.IsPressed) && mUIFocusTimer<= 0f && (mMainMenuButtonFocus == 5 || mMainMenuButtonFocus == 4))
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
				
			}*/
		}
	}

}
