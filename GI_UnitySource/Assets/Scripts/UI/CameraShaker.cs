﻿using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Pluging for game pad rumble
using InControl;

public class CameraShaker : MonoBehaviour 
{

	float strength = 2.5f;
	float duration = .25f;

	public float mRedShakeStrength = 2.8f;
	public float mRedShakeDuration = 1f;

	public float mTealShakeStrength = 2.5f;
	public float mTealShakeDuration = .75f;
	
	public float mGreenShakeStrength = 2.75f;
	public float mGreenShakeDuration = .8f;

	public float mPurpleShakeStrength = 2.6f;
	public float mPurpleShakeDuration = 1.25f;

	public float mDeathShakeStrength = 2.5f;
	public float mDeathShakeDuration = .5f;

	public float mEnemyShakeStrength = 2.5f; //was 5.0f Changed to 2.5f for Florida Con
	public float mEnemyShakeDuration = .1f;

	public float mShootShakeStrength = .2f;
	public float mShootShakeDuration = .1f;

	float mShakeTime = 0f;
	Vector3 mStartingPosition;
	// Use this for initialization
	void Start () 
	{
		mStartingPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (mShakeTime > 0 && Time.timeScale != 0f)
		{
			transform.position = mStartingPosition+(Random.insideUnitSphere * strength);
			mShakeTime -= Time.deltaTime;
#if !UNITY_ANDROID
			GamePad.SetVibration(0, strength, strength);
#endif
			//InputManager.ActiveDevice.Vibrate(strength);
		}
		else
		{
#if !UNITY_ANDROID
			GamePad.SetVibration(0, 0, 0);
#endif
			//InputManager.ActiveDevice.Vibrate(0f);
			mShakeTime = 0f;
			transform.position = mStartingPosition;
		}
	}

	public void ShakeCamera()
	{
		strength = mShootShakeStrength;
		mShakeTime = mShootShakeDuration;
	}
	public void ShakeCameraEnemy(){

        if (Random.Range(0, 8) < 1) //12.5% chance to shake camera
        {
            mShakeTime = mEnemyShakeDuration;
            strength = mEnemyShakeStrength;
        }
	}
	public void ShakeCameraDeath(){

		mShakeTime = mDeathShakeDuration;
		strength = mDeathShakeStrength;
	}
	public void ShakeCameraPurple(){

		mShakeTime = mPurpleShakeDuration;
		strength = mPurpleShakeStrength;
	}
	public void ShakeCameraTeal(){
		
		mShakeTime = mTealShakeDuration;
		strength = mTealShakeStrength;
	}
	public void ShakeCameraRed(){
		
		mShakeTime = mRedShakeDuration;
		strength = mRedShakeStrength;
	}
	public void ShakeCameraGreen(){
		
		mShakeTime = mGreenShakeDuration;
		strength = mGreenShakeStrength;
	}
}
