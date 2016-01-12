using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using InControl;

public class VolumeControlSliders : MonoBehaviour 
{
	//For showing/hiding the panel ~Adam
	public bool mMenuOpen = false;
	[SerializeField] private GameObject mSliderPanel;

	//For what part of the menu is being highlighted ~Adam
	public int mMenuFocus = 0;
		/* 0: SFX
		 * 1: BGM
		 * 2: Rumble
		 * 3: Tutorial
		 * 4: Back
		 */
	[SerializeField] private Text mSFXText;
	[SerializeField] private Slider mSFXSliderBar;
	[SerializeField] private Image mSFXSliderFill;
	[SerializeField] private Text mBGMText;
	[SerializeField] private Slider mBGMSliderBar;
	[SerializeField] private Image mBGMSliderFill;
	[SerializeField] private Text mBackButton;

	[SerializeField] private Text mRumbleSelect;
	[SerializeField] private Text mRumbleOn;
	[SerializeField] private Text mRumbleOff;


	[SerializeField] private Text mTutorialText;
	[SerializeField] private Image mTutorialBox;

	[SerializeField] private Color mNormalColor;
	[SerializeField] private Color mFocusColor;
	public float mUIFocusTimer = 0.2f;

	// Use this for initialization
	void Start () 
	{
		mSFXSliderBar.value = PlayerPrefs.GetFloat("SFXVolume");
		mBGMSliderBar.value = PlayerPrefs.GetFloat("BGMVolume");

		if(PlayerPrefs.GetInt("FirstTimeVolumeSetup")==0)
		{
			PlayerPrefs.SetFloat("BGMVolume", 0.4f);
			PlayerPrefs.SetFloat("SFXVolume", 0.4f);
			PlayerPrefs.SetInt("FirstTimeVolumeSetup", 1);
			PlayerPrefs.SetInt("RumbleOn", 0);

		}

		//Disable Rumble on Mac ~Adam
		if(Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXWebPlayer
		   || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXDashboardPlayer)
		{
			PlayerPrefs.SetInt("RumbleOn", 1);
			mRumbleSelect.gameObject.SetActive(false);
			mRumbleOn.gameObject.SetActive(false);
			mRumbleOff.gameObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		mSliderPanel.SetActive(mMenuOpen);

		if(mMenuOpen)
		{

			if(mUIFocusTimer > 0f)
			{
				if(Input.GetAxis ("Vertical") == 0 && Input.GetAxis ("VerticalP2") == 0)
				{
					//mUIFocusTimer -= Time.deltaTime;
					if(Input.GetAxis ("Horizontal") == 0 && Input.GetAxis ("HorizontalP2") == 0)
					{
						mUIFocusTimer = -1f;
					}

					else
					{
						mUIFocusTimer -= 0.03f;
					}
				}
//				if(Time.timeScale != 0)
//				{
//					mUIFocusTimer -= Time.deltaTime;
//				}
//				else
//				{
//					mUIFocusTimer -= 0.01f;
//				}
			}
			if(!Application.isMobilePlatform)
			{
				//Adjust bars values to match the volume settings ~Adam
				mSFXSliderBar.value = PlayerPrefs.GetFloat("SFXVolume");
				mBGMSliderBar.value = PlayerPrefs.GetFloat("BGMVolume");


				
				if(mUIFocusTimer <= 0f)
				{
					//Control the "Back" button

					if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Thrusters") || Input.GetButtonDown("FireGun") || (InputManager.ActiveDevice.Action1.WasPressed))
					{
						if(mMenuFocus == 4)
						{
							CloseVolumeMenu();
						}
						if(mMenuFocus == 3)
						{
							if(PlayerPrefs.GetInt ("PlayedTutorial") == 0)
							{
								PlayerPrefs.SetInt ("PlayedTutorial", 1);
							}
							else
							{
								PlayerPrefs.SetInt ("PlayedTutorial", 0);
								ResetFocusTimer();

							}
						}

					}
					
					
					//Adjust volume up and down (Right Input)~Adam
					else if(Input.GetAxisRaw ("Horizontal") > 0 || Input.GetAxisRaw ("HorizontalP2") > 0 || InputManager.ActiveDevice.DPadRight.WasPressed)
					{
						switch(mMenuFocus)
						{
						case 0:
							PlayerPrefs.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume")+0.1f);
							if(PlayerPrefs.GetFloat("SFXVolume") > 1.0f)
							{
								PlayerPrefs.SetFloat("SFXVolume", 1.0f);
							}
							ResetFocusTimer();
							break;
						case 1:
							PlayerPrefs.SetFloat("BGMVolume", PlayerPrefs.GetFloat("BGMVolume")+0.1f);
							if(PlayerPrefs.GetFloat("BGMVolume") > 1.0f)
							{
								PlayerPrefs.SetFloat("BGMVolume", 1.0f);
							}
							ResetFocusTimer();
							break;
						case 2:
							PlayerPrefs.SetInt("RumbleOn", 1);
							break;
						case 3:
							break;
						case 4:
							break;
						default:
							break;
						}
					}
					
					
					//Adjust volume up and down (Left Input)~Adam
					else if(Input.GetAxisRaw ("Horizontal") < 0 || Input.GetAxisRaw ("HorizontalP2") < 0 || InputManager.ActiveDevice.DPadLeft.WasPressed)
					{
						switch(mMenuFocus)
						{
						case 0:
							PlayerPrefs.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume")-0.1f);
							if((PlayerPrefs.GetFloat("SFXVolume") < 0.0f))
							{
								PlayerPrefs.SetFloat("SFXVolume", 0.0f);
							}
							ResetFocusTimer();
							break;
						case 1:
							PlayerPrefs.SetFloat("BGMVolume", PlayerPrefs.GetFloat("BGMVolume")-0.1f);
							if((PlayerPrefs.GetFloat("BGMVolume") < 0.0f))
							{
								PlayerPrefs.SetFloat("BGMVolume", 0.0f);
							}
							ResetFocusTimer();
							break;
						case 2:
							PlayerPrefs.SetInt("RumbleOn", 0);
							break;
						case 3:
							break;
						default:
							break;
						}
					}
					
					
					//Switch between options (Down input) ~Adam
					if(Input.GetAxisRaw ("Vertical") < 0 || Input.GetAxisRaw ("VerticalP2") < 0 || InputManager.ActiveDevice.DPadDown.WasPressed)
					{
						switch(mMenuFocus)
						{
						case 0:
							mMenuFocus = 1;
							ResetFocusTimer();
							break;
						case 1:
							//Skip the rumble option on Mac~Adam
							if(Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXWebPlayer
							   || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXDashboardPlayer)
							{
								mMenuFocus = 4;
							}
							else
							{
								mMenuFocus = 2;
							}
							ResetFocusTimer();
							break;
						case 2:
							mMenuFocus = 3;
							ResetFocusTimer();
							break;
						case 3:
							mMenuFocus = 4;
							ResetFocusTimer();
							break;
						case 4:
							break;
						default:
							break;
						}				
						
					}
					//Switch between options (Up input) ~Adam
					else if(Input.GetAxisRaw ("Vertical") > 0 || Input.GetAxisRaw ("VerticalP2") > 0 || InputManager.ActiveDevice.DPadUp.WasPressed)
					{
						switch(mMenuFocus)
						{
						case 0:
							break;
						case 1:
							mMenuFocus = 0;
							ResetFocusTimer();
							break;
						case 2:
							mMenuFocus = 1;
							ResetFocusTimer();
							break;
						case 3:
							//Skip the rumble option on Mac~Adam
							if(Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXWebPlayer
							   || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXDashboardPlayer)
							{
								mMenuFocus = 1;
							}
							else
							{
								mMenuFocus = 2;
							}
							ResetFocusTimer();
							break;
						case 4:
							mMenuFocus = 3;
							ResetFocusTimer();
							break;
						default:
							break;
						}	
					}
					
					
					//Control menu colors
					switch(mMenuFocus)
					{
					case 0:
						mSFXText.color = mFocusColor;
						mSFXSliderFill.color = mFocusColor;
						mBGMText.color = mNormalColor;
						mBGMSliderFill.color = mNormalColor;
						mBackButton.color = mNormalColor;
						mRumbleSelect.color = mNormalColor;
						mTutorialText.color = mNormalColor;
						break;
					case 1:
						mSFXText.color = mNormalColor;
						mSFXSliderFill.color = mNormalColor;
						mBGMText.color = mFocusColor;
						mBGMSliderFill.color = mFocusColor;
						mBackButton.color = mNormalColor;
						mRumbleSelect.color = mNormalColor;
						mTutorialText.color = mNormalColor;
						break;
					case 2:
						mSFXText.color = mNormalColor;
						mSFXSliderFill.color = mNormalColor;
						mBGMText.color = mNormalColor;
						mBGMSliderFill.color = mNormalColor;
						mBackButton.color = mNormalColor;
						mRumbleSelect.color = mFocusColor;
						mTutorialText.color = mNormalColor;
						break;
					case 3:
						mSFXText.color = mNormalColor;
						mSFXSliderFill.color = mNormalColor;
						mBGMText.color = mNormalColor;
						mBGMSliderFill.color = mNormalColor;
						mBackButton.color = mNormalColor;
						mRumbleSelect.color = mNormalColor;
						mTutorialText.color = mFocusColor;
						break;
					case 4:
						mSFXText.color = mNormalColor;
						mSFXSliderFill.color = mNormalColor;
						mBGMText.color = mNormalColor;
						mBGMSliderFill.color = mNormalColor;
						mBackButton.color = mFocusColor;
						mRumbleSelect.color = mNormalColor;
						mTutorialText.color = mNormalColor;
						break;
					default:
						break;
					}
				}
				if(PlayerPrefs.GetInt("RumbleOn") == 1)
				{
					mRumbleOff.color = mFocusColor;
					mRumbleOn.color = mNormalColor;
				}
				else
				{
					mRumbleOff.color = mNormalColor;
					mRumbleOn.color = mFocusColor;
				}
				if(PlayerPrefs.GetInt ("PlayedTutorial") == 0)
				{
					mTutorialBox.enabled = true;
				}
				else
				{
					mTutorialBox.enabled = false;
				}

			}
			else
			{
				PlayerPrefs.SetFloat("SFXVolume",mSFXSliderBar.value);
				PlayerPrefs.SetFloat("BGMVolume",mBGMSliderBar.value);
			}

		}
	}//END of Update()

	public void CloseVolumeMenu()
	{
		mUIFocusTimer+=0.2f;
		mMenuFocus = 0;

		if(Application.loadedLevel == 0)
		{
			FindObjectOfType<MainMenuGUIFocusController>().mUIFocusTimer=0.2f;
			FindObjectOfType<MainMenuGUIFocusController>().mStartupTimer=0.2f;
		}
		else
		{
			FindObjectOfType<PauseManager>().mUIFocusTimer=0.2f;
			FindObjectOfType<PauseManager>().mStartupTimer=0.2f;
		}

		mMenuOpen = false;
	}


	void ResetFocusTimer()
	{
		mUIFocusTimer = 0.25f;
	}
}
