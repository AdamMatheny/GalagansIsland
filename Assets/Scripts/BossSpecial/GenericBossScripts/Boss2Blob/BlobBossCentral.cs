using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlobBossCentral : BossCentral 
{
	//For ramming the player with the death weapon ~Adam
	float mDefaultSpeed;
	public float mRammingSpeed;
	public float mRamInterval;
	public float mRamTimer = 0f;
	public bool mRamming = false;
	public Vector3 mRamPoint = Vector3.zero;

	public GameObject toothBreak;
	public Transform toothBreakTransform;

	//For knocking out teeth ~Adam
	public Animator mAnimator;
	public List<RuntimeAnimatorController> mAnimationStages = new List<RuntimeAnimatorController>();
	public List<int> mHealthStages = new List<int>();
	public GameObject mDamageEffect;
	public Transform mDamageEffectPoint;

	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
		mDefaultSpeed = mMoveSpeed;
	}//END of Start()
	
	// Update is called once per frame
	protected override void Update () 
	{
		base.Update ();

		//Change number of sprite based on health ~Adam
		if(mHealthStages.Count >0 && mAnimationStages.Count > 0 && mCurrentHealth <= mHealthStages[0])
		{
			//Make a boom where the damage was ~Adam
			if(mCurrentHealth == mHealthStages[0] && mDamageEffect != null && mDamageEffectPoint != null)
			{
				Instantiate (mDamageEffect, mDamageEffectPoint.position, Quaternion.identity);
			}

			if(toothBreak != null){

				Instantiate(toothBreak, toothBreakTransform.position, Quaternion.identity);
			}

			mAnimator.runtimeAnimatorController = mAnimationStages[0];
			mHealthStages.Remove (mHealthStages[0]);
			mAnimationStages.Remove(mAnimationStages[0]);
		}


		//Toggle Ramming when under the death weapon threshhold ~Adam
		if(!mDying && mFightStarted && mCurrentHealth <= mDeathWeaponThreshhold)
		{
			mRamTimer -= Time.deltaTime;
			if(mRamTimer <= 0f)
			{
				mRamming = true;
				mMoveSpeed = mRammingSpeed;
				//Turn off the shooting while ramming ~Adam
				foreach(GameObject weapon in mWeapons)
				{
					weapon.SetActive (false);
				}
				mDeathWeapon.SetActive (true);

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
					weapon.SetActive (true);
				}
				mDeathWeapon.SetActive (false);
			}
		}
	}//END of Update()
	
	protected override void BossEntry()
	{
		base.BossEntry ();
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
		base.BossDeath ();
	}
}