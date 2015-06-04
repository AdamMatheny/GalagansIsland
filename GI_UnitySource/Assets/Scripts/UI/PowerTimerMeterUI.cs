﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerTimerMeterUI : MonoBehaviour 
{
	[SerializeField] private Image mPowerTimerBar;
	[SerializeField] private Image mPowerTimerBulb;
	[SerializeField] private Image mPowerTimerSwirl;

	public float swirlStartY;

	PlayerShipController mPlayer;
	
	// Use this for initialization
	void Start () 
	{
		swirlStartY = mPowerTimerSwirl.transform.position.y;

		//Find the player ship -Adam
		mPlayer = FindObjectOfType<PlayerShipController>();
	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		//Safety in case the player ship connection is lost -Adam
		if(mPlayer == null)
		{
			mPlayer = FindObjectOfType<PlayerShipController>();
		}
		
		else
		{


			if(mPlayer.mThreeBullet)
			{
				//Make the bar move up and down
				mPowerTimerBar.enabled = true;
				mPowerTimerBar.GetComponent<RectTransform>().localScale = new Vector3(1f, mPlayer.mThreeBulletTimer/30f, 1f);
				mPowerTimerSwirl.transform.position = new Vector3(mPowerTimerSwirl.transform.position.x, swirlStartY + (mPowerTimerBar.GetComponent<RectTransform>().localScale.y * 24), mPowerTimerSwirl.transform.position.z);
				mPowerTimerBulb.enabled = true;
			}
			else
			{
				mPowerTimerBar.enabled = false;
				mPowerTimerBulb.enabled = false;

			}
		}
	}//END of Update()
}