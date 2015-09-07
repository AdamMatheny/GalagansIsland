﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss5Central : BossCentral 
{
	//For coming in slow ~Adam
	public float mEntrySpeed;

	//For ramming the player when under the death weapon threshhold ~Adam
	float mDefaultSpeed;
	public float mRammingSpeed;
	public float mRamInterval;
	public float mRamTimer = 0f;
	public bool mRamming = false;
	public Vector3 mRamPoint = Vector3.zero;
	public GameObject mRammingSphere;

	//For breaking the gem ~Adam
	public Animator mAnimator;
	public List<RuntimeAnimatorController> mAnimationStages = new List<RuntimeAnimatorController>();
	public List<int> mHealthStages = new List<int>();
	public GameObject mDamageEffect;
	public Transform mDamageEffectPoint;

	//For shooting the gem and eye projectiles in time with animations ~Adam
	[SerializeField] private List<Animator> mAnimatedParts = new List<Animator>();
	[SerializeField] private float mShootTimer = 5;
	[SerializeField] private float mShootInterval = 5;
	[SerializeField] private Transform[] mEyePositions;
	[SerializeField] private Transform mGemPosition;
	[SerializeField] private GameObject mEyeBullet;
	[SerializeField] private GameObject mGemBullet;

	//For ending the game ~Adam
	[SerializeField] private GameObject mGameHUD;
	[SerializeField] private GameObject mScreenFader;

	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
		mDefaultSpeed = mMoveSpeed;
		mMoveSpeed = mEntrySpeed;

	}//END of Start()
	
	// Update is called once per frame
	protected override void Update () 
	{
		base.Update ();
		mAnimatedParts.Remove (null);

		//Change number of sprite based on health ~Adam
		if(mHealthStages.Count >0 && mAnimationStages.Count > 0 && mCurrentHealth <= mHealthStages[0])
		{
			//Make a boom where the damage was ~Adam
			if(mCurrentHealth == mHealthStages[0] && mDamageEffect != null && mDamageEffectPoint != null)
			{
				Instantiate (mDamageEffect, mDamageEffectPoint.position, Quaternion.identity);
			}
			mAnimator.runtimeAnimatorController = mAnimationStages[0];
			mHealthStages.Remove (mHealthStages[0]);
			mAnimationStages.Remove(mAnimationStages[0]);
		}
		
		
		//Toggle Ramming when under the death weapon threshhold ~Adam
		if(!mDying && mFightStarted)
		{
			mMoveSpeed = mDefaultSpeed;

			if(mCurrentHealth > mDeathWeaponThreshhold)
			{
				mShootTimer -= Time.deltaTime;
				if(mShootTimer <= 0f)
				{
					foreach(Animator movingPart in mAnimatedParts)
					{
						movingPart.Play ("Shoot");
					}
					mShootTimer = mShootInterval;
				}
			}
			if(mCurrentHealth<= mDeathWeaponThreshhold)
			{
				if(GetComponent<BossRotator>() != null)
				{
					GetComponent<BossRotator>().enabled = true;
				}
				mRamTimer -= Time.deltaTime;
				if(mRamTimer <= 0f)
				{
					mRamming = true;
					mMoveSpeed = mRammingSpeed;
					//Turn off the shooting while ramming ~Adam
					foreach(GameObject weapon in mWeapons)
					{
						mDeathWeapon.SetActive (false);
					}
					mRammingSphere.SetActive (true);
					
					if(mRamTimer <= mRamInterval *-0.5f)
					{
						mRamming = false;
						mRamTimer = mRamInterval;
					}
				}
				else
				{
					mMoveSpeed = mDefaultSpeed;
					mRamPoint = mTargetedPlayer.transform.position;
					//Turn the shooting back on ~Adam
					foreach(GameObject weapon in mWeapons)
					{
						mDeathWeapon.SetActive (true);
					}
					mRammingSphere.SetActive (false);
				}
			}
		}
	}//END of Update()



	protected override void BossEntry()
	{
		if(mEntryTime<= 5f)
		{
			base.BossEntry ();
		}
		else
		{
			mEntryTime -= Time.deltaTime;
		}
	}//END of BossEntry()
	
	protected override void BossMovement()
	{
		base.BossMovement ();
		if(mRamming)
		{
			mMoveTarget = mRamPoint;
		}
		if(Vector3.Distance (transform.position, mRamPoint) < 2f)
		{
			mRamming = false;
			mRamTimer = mRamInterval;
		}
	}//END of BossMovement()
	
	protected override void BossDeath()
	{
		if(mDeathTimer <= 0f)
		{
			//Let the Kill Counter know to go to the next level
			LevelKillCounter killCounter = FindObjectOfType<LevelKillCounter>();
			killCounter.mKillCount = killCounter.mRequiredKills+1;
			
			//Award points for death ~Adam
			if(mScoreMan.mPlayerAvatar != null && mScoreMan.mPlayer2Avatar != null)
			{
				mScoreMan.AdjustScore (mScoreValue/2, true);
				mScoreMan.AdjustScore (mScoreValue/2, false);
			}
			else if(mScoreMan.mPlayerAvatar == null && mScoreMan.mPlayer2Avatar != null)
			{
				mScoreMan.AdjustScore (mScoreValue, false);
			}
			else
			{
				mScoreMan.AdjustScore (mScoreValue, true);
			}



			if(FindObjectOfType<PlayerOneShipController>() != null)
			{
				Destroy(FindObjectOfType<PlayerShipController>().gameObject);
			}
			if (FindObjectOfType<PlayerTwoShipController>() != null)
			{
				Destroy(FindObjectOfType<PlayerTwoShipController>().gameObject);
			}
			Destroy(FindObjectOfType<LevelKillCounter>().gameObject);
			//Destroy(FindObjectOfType<ScoreManager>().gameObject);
			Application.LoadLevel("Credits");

			Destroy(this.gameObject);
		}
		else
		{
			mDeathTimer -= Time.deltaTime;
			if(mDeathEffect != null)
			{
				mDeathEffect.SetActive (true);
			}
			//Turn off any remaining weapons and weakpoints ~Adam
			foreach(BossWeakPoint weakPoint in mWeakPoints)
			{
				weakPoint.gameObject.SetActive (false);
			}
			foreach(GameObject weapon in mWeapons)
			{
				weapon.SetActive (false);
			}
			mDeathWeapon.SetActive (false);

			//Special stuff for ending the game and going to credits ~Adam
			FindObjectOfType<ScoreManager>().enabled = false;
			FindObjectOfType<LevelKillCounter>().enabled = false;
			//	FindObjectOfType<PauseManager>().enabled = false;
			mGameHUD.SetActive(false);
			//Fade out the screen ~Adam
			mScreenFader.GetComponent<Renderer>().enabled = true;
			mScreenFader.GetComponent<Renderer>().material.color = Color.Lerp(mScreenFader.GetComponent<Renderer>().material.color, new Color(0,0,0,1f),0.01f);
			//Fade out the audio ~Adam
			if(FindObjectOfType<BGMVolumeController>() != null)
			{
				FindObjectOfType<BGMVolumeController>().GetComponent<AudioSource>().ignoreListenerVolume = false;
				FindObjectOfType<BGMVolumeController>().enabled = false;
			}
			AudioListener.volume -=  0.001f;
			if(mDeathTimer <= 2.8f)
			{
				AudioListener.volume -=  0.01f;
			}

		}


	}

	public void FireEyes()
	{
		for(int i = 0; i < mEyePositions.Length; i++)
		{
			Instantiate (mEyeBullet, mEyePositions[i].position, Quaternion.identity);
		}
	}

	public void FireGem()
	{
		Instantiate(mGemBullet, mGemPosition.position, Quaternion.identity);
	}
}