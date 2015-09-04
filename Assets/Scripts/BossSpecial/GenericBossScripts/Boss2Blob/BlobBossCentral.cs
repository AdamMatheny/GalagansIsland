using UnityEngine;
using System.Collections;

public class BlobBossCentral : BossCentral 
{
	//For ramming the player with the death weapon ~Adam
	float mDefaultSpeed;
	public float mRammingSpeed;
	public float mRamInterval;
	public float mRamTimer = 0f;
	public bool mRamming = false;

	//For knocking out teeth ~Adam
	public Animator mAnimator;
	public RuntimeAnimatorController[] mAnimationStages;


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

		//Change number of teeth based on health ~Adam
		//8 teeth
		if(mCurrentHealth >= 108)
		{
			mAnimator.runtimeAnimatorController = mAnimationStages[0];
		}
		//6 teeth ~Adam
		else if(mCurrentHealth >= 72)
		{
			mAnimator.runtimeAnimatorController = mAnimationStages[1];
		}
		//4 teeth ~Adam
		else if(mCurrentHealth >= 38)
		{
			mAnimator.runtimeAnimatorController = mAnimationStages[2];
		}
		//2 teeth ~Adam
		else if(mCurrentHealth >= 12)
		{
			mAnimator.runtimeAnimatorController = mAnimationStages[3];
		}
		//No teeth ~Adam
		else
		{
			mAnimator.runtimeAnimatorController = mAnimationStages[4];
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
				}
			}
			else
			{
				mMoveSpeed = mDefaultSpeed;
				//Turn the shooting back on ~Adam
				foreach(GameObject weapon in mWeapons)
				{
					weapon.SetActive (false);
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
			mMoveTarget = mTargetedPlayer.transform.position + Vector3.up*8f;
		}
		if(Vector3.Distance (transform.position, mTargetedPlayer.transform.position) < 9f)
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