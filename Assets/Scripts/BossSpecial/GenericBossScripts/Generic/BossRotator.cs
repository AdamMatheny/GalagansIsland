﻿using UnityEngine;
using System.Collections;

//Use this for parts of the boss that spin around, like the horn beams on the skull boss ~Adam

public class BossRotator : MonoBehaviour 
{
	public BossCentral mBossCentral;
	public Vector3 mTargetRotation = Vector3.zero;
	public Vector3 newDir;
	public float mRotateSpeed = 10f;
	Vector3 mTargetPlayerPosition;

	public bool mSecondaryRotator = false;
	public GameObject mReferenceRotator;
	// Use this for initialization
	void Start () 
	{
		
	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		//Spin in tune with another rotator ~Adam
		if(mReferenceRotator != null && mSecondaryRotator)
		{
			transform.rotation = mReferenceRotator.transform.rotation;
		}
		//Spin on its own towards the player ~Adam
		else
		{
			if(mBossCentral.mTargetedPlayer != null)
			{
				mTargetPlayerPosition = mBossCentral.mTargetedPlayer.transform.position;
				mTargetRotation = new Vector3(mTargetPlayerPosition.x-transform.position.x,mTargetPlayerPosition.y-transform.position.y,0f);
				//mTargetRotation += Vector3.up*3f;
			}

			Vector3.Normalize (mTargetRotation);
			mTargetRotation = new Vector3(0f, 0f, Vector3.Angle(mTargetRotation, Vector3.down));
			if(mTargetPlayerPosition.x < transform.position.x)
			{
				mTargetRotation *= -1f;
			}

			transform.rotation =Quaternion.Lerp (transform.rotation, Quaternion.Euler (mTargetRotation), 0.001f*mRotateSpeed * Time.timeScale);
		}
	}//END of Update()



}