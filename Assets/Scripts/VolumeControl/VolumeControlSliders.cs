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
		 * 2: Back
		 */
	[SerializeField] private Text mSFXText;
	[SerializeField] private Slider mSFXSliderBar;
	[SerializeField] private Image mSFXSliderFill;
	[SerializeField] private Text mBGMText;
	[SerializeField] private Slider mBGMSliderBar;
	[SerializeField] private Image mBGMSliderFill;
	[SerializeField] private Text mBackButton;
	[SerializeField] private Color mNormalColor;
	[SerializeField] private Color mFocusColor;
	public float mUIFocusTimer = 0.2f;

	// Use this for initialization
	void Start () 
	{
		mSFXSliderBar.value = PlayerPrefs.GetFloat("SFXVolume");
		mBGMSliderBar.value = PlayerPrefs.GetFloat("BGMVolume");
	}
	
	// Update is called once per frame
	void Update () 
	{
		mSliderPanel.SetActive(mMenuOpen);

		if(mMenuOpen)
		{
			Debug.Log("volume menu is open");

			if(mUIFocusTimer > 0f)
			{
				if(Time.timeScale != 0)
				{
					mUIFocusTimer -= Time.deltaTime;
				}
				else
				{
					mUIFocusTimer -= 0.01f;
				}
			}
			if(!Application.isMobilePlatform)
			{
				//Adjust bars values to match the volume settings ~Adam
				mSFXSliderBar.value = PlayerPrefs.GetFloat("SFXVolume");
				mBGMSliderBar.value = PlayerPrefs.GetFloat("BGMVolume");

				Debug.Log("volume menu is not on mobile platform");

				
				//Control the "Back" button
				if(mUIFocusTimer <= 0f)
				{
					Debug.Log("volume menu is ready");

					if (Input.GetButtonDown("Thrusters") || Input.GetButtonDown("FireGun") || (InputManager.ActiveDevice.Action1.IsPressed))
					{
						if(mMenuFocus == 2)
						{
							CloseVolumeMenu();
						}
					}
					
					
					//Adjust volume up and down ~Adam
					else if(Input.GetAxisRaw ("Horizontal") > 0 || InputManager.ActiveDevice.DPadRight.WasPressed)
					{
						Debug.Log("pressing right");
						switch(mMenuFocus)
						{
						case 0:
							PlayerPrefs.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume")+0.1f);
							if(PlayerPrefs.GetFloat("SFXVolume") > 1.0f)
							{
								PlayerPrefs.SetFloat("SFXVolume", 1.0f);
							}
							mUIFocusTimer = 0.2f;
							break;
						case 1:
							PlayerPrefs.SetFloat("BGMVolume", PlayerPrefs.GetFloat("BGMVolume")+0.1f);
							if(PlayerPrefs.GetFloat("BGMVolume") > 1.0f)
							{
								PlayerPrefs.SetFloat("BGMVolume", 1.0f);
							}
							mUIFocusTimer = 0.2f;
							break;
						case 2:
							break;
						default:
							break;
						}
					}
					
					
					//Move from score quit game to insert coin ~Adam
					else if(Input.GetAxisRaw ("Horizontal") < 0 || InputManager.ActiveDevice.DPadLeft.WasPressed)
					{
						Debug.Log("pressing left");
						switch(mMenuFocus)
						{
						case 0:
							PlayerPrefs.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume")-0.1f);
							if((PlayerPrefs.GetFloat("SFXVolume") < 0.0f))
							{
								PlayerPrefs.SetFloat("SFXVolume", 0.0f);
							}
							mUIFocusTimer = 0.2f;
							break;
						case 1:
							PlayerPrefs.SetFloat("BGMVolume", PlayerPrefs.GetFloat("BGMVolume")-0.1f);
							if((PlayerPrefs.GetFloat("BGMVolume") < 0.0f))
							{
								PlayerPrefs.SetFloat("BGMVolume", 0.0f);
							}
							mUIFocusTimer = 0.2f;
							break;
						case 2:
							break;
						default:
							break;
						}
					}
					
					
					//Switch between options
					//Move from focusing on "Insert Coin" to focusing on the CoOp Start ~Adam
					if(Input.GetAxisRaw ("Vertical") < 0 || InputManager.ActiveDevice.DPadDown.WasPressed)
					{
						Debug.Log("pressing down");
						switch(mMenuFocus)
						{
						case 0:
							mMenuFocus = 1;
							mUIFocusTimer = 0.2f;
							break;
						case 1:
							mMenuFocus = 2;
							mUIFocusTimer = 0.2f;
							break;
						case 2:
							break;
						default:
							break;
						}				
						
					}
					//Move from focusing on CoOpStart to focusing on the "Insert Coin" ~Adam
					else if(Input.GetAxisRaw ("Vertical") > 0 || InputManager.ActiveDevice.DPadUp.WasPressed)
					{
						Debug.Log("pressing up");
						switch(mMenuFocus)
						{
						case 0:
							break;
						case 1:
							mMenuFocus = 0;
							mUIFocusTimer = 0.2f;
							break;
						case 2:
							mMenuFocus = 1;
							mUIFocusTimer = 0.2f;
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
						break;
					case 1:
						mSFXText.color = mNormalColor;
						mSFXSliderFill.color = mNormalColor;
						mBGMText.color = mFocusColor;
						mBGMSliderFill.color = mFocusColor;
						mBackButton.color = mNormalColor;
						break;
					case 2:
						mSFXText.color = mNormalColor;
						mSFXSliderFill.color = mNormalColor;
						mBGMText.color = mNormalColor;
						mBGMSliderFill.color = mNormalColor;
						mBackButton.color = mFocusColor;
						break;
					default:
						break;
					}
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
			FindObjectOfType<MainMenuGUIFocusController>().mUIFocusTimer+=0.2f;
		}
		else
		{
			FindObjectOfType<PauseManager>().mUIFocusTimer+=0.2f;
		}

		mMenuOpen = false;
	}
}
