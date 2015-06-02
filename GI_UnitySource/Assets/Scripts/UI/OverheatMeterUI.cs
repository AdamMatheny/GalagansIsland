using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OverheatMeterUI : MonoBehaviour 
{
	[SerializeField] private Image mOverHeatBar;
	[SerializeField] private Image mOverHeatOverlay;
	[SerializeField] private Image mBlankBulb;
	[SerializeField] private Image mRedBulb;
	[SerializeField] private ParticleSystem mSteamUp;
	[SerializeField] private ParticleSystem mSteamDown;

	bool mCanPlaySteamNoise = true;

	PlayerShipController mPlayer;

	// Use this for initialization
	void Start () 
	{

		//Find the player ship -Adam
		mPlayer = FindObjectOfType<PlayerShipController>();
	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{

		if(mSteamUp == null)
		{
			mSteamUp = GameObject.Find("OverheatSteamUp").GetComponent<ParticleSystem>();
		}
		else
		{
			if(mPlayer.heatLevel/mPlayer.maxHeatLevel < 0.9f && !mSteamUp.GetComponent<AudioSource>().isPlaying)
			{
				mCanPlaySteamNoise = true;
			}
		}
		if(mSteamDown == null)
		{
			mSteamDown = GameObject.Find("OverheatSteamDown").GetComponent<ParticleSystem>();
		}

		//Safety in case the player ship connection is lost -Adam
		if(mPlayer == null)
		{
			mPlayer = FindObjectOfType<PlayerShipController>();
		}

		else if(GetComponent<Image>().canvas.isActiveAndEnabled) //Only do stuff when the canvas is actually turned on
		{
			//Make the bar move up and down
			mOverHeatBar.GetComponent<RectTransform>().localScale = new Vector3(1f, mPlayer.heatLevel/mPlayer.maxHeatLevel, 1f);

			//Display overlay when overheated
			if(mPlayer.isOverheated)
			{
				mOverHeatOverlay.enabled = true;
				GetComponent<Animator>().speed = 0f;
				if(mSteamUp != null && mSteamDown != null)
				{
					mSteamUp.Play();
					mSteamUp.GetComponentInChildren<ParticleSystem>().Play();
					mSteamDown.Stop();
				}
				mRedBulb.enabled = false;
				mBlankBulb.enabled = false;
			}
			else
			{
				if(mSteamUp != null & mSteamDown != null)
				{
					mSteamDown.Play();
					mSteamUp.Stop();
					mSteamUp.GetComponentInChildren<ParticleSystem>().Stop();
				}
				mOverHeatOverlay.enabled = false;
				if(mPlayer.heatLevel/mPlayer.maxHeatLevel > 0.9f)
				{
					mRedBulb.enabled = true;
					mBlankBulb.enabled = false;

					GetComponent<Animator>().speed = 5f;
					if(mSteamDown != null)
					{
						mSteamDown.startSpeed = 5f;
						mSteamDown.startLifetime = 0.5f;
						mSteamDown.emissionRate = 50f;
						if(mSteamUp != null && mCanPlaySteamNoise)
						{
							mSteamUp.GetComponent<AudioSource>().Play();
							mCanPlaySteamNoise = false;
						}
					}
				}
				else
				{
					mRedBulb.enabled = false;
					mBlankBulb.enabled = true;

					GetComponent<Animator>().speed = 1f;
					if(mSteamDown != null)
					{
						mSteamDown.startSpeed = 1f;
						mSteamDown.startLifetime = 2f;
						mSteamDown.emissionRate = 10f;
					}
				}
			}
		}
		//Hide the steam if the canvas it turned off or the ship is missing
		else
		{
			if(mSteamUp != null & mSteamDown != null)
			{
				mSteamUp.Stop();
				mSteamUp.GetComponentInChildren<ParticleSystem>().Stop();
				mSteamDown.Stop();
			}
		}
	}//END of Update()
}
