using UnityEngine;
using System.Collections;

public class BlobBoss : BossGenericScript 
{
	
	public int mhealth = 120;
	
	public SpriteRenderer spriter;
	public Animator mAnimator;

	public RuntimeAnimatorController[] mAnimationStages;


	
	public override void Start ()
	{
		spriter = GetComponent<SpriteRenderer> ();
		base.Start ();
	}
	
	public override void Update ()
	{
		//For flashing when hit ~Adam
		if(spriter!= null)
		{
			spriter.color = Color.Lerp (spriter.color, Color.white,0.1f);
			
		}
		//Change number of teeth based on health ~Adam
		//6 teeth
		if(mhealth >= 90)
		{
			mAnimator.runtimeAnimatorController = mAnimationStages[0];
		}
		//4 teeth ~Adam
		else if(mhealth >= 50)
		{
			mAnimator.runtimeAnimatorController = mAnimationStages[1];
		}
		//2 teeth ~Adam
		else if(mhealth >= 11)
		{
			mAnimator.runtimeAnimatorController = mAnimationStages[2];
		}
		//No teeth ~Adam
		else
		{
			mAnimator.runtimeAnimatorController = mAnimationStages[3];
		}

		base.Update ();
	}


}
