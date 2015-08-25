using UnityEngine;
using System.Collections;

public class DuoHornScript : BossWeakPoint {
	
	public int health;
	
	public enum HornList{
		
		LeftHorn,
		RightHorn
	}
	
	public HornList hornNumber;
	
	//For Flashing when hit ~Adam
	public SpriteRenderer mHornSprite;
	
	public GameObject mDeathEffect;
	public Transform mExplosionPoint;

	public override void Start ()
	{
		mBossCentral.mTotalHealth += health;
		mBossCentral.mCurrentHealth += health;
	}

	public override void Update()
	{
		//For flashing when hit ~Adam
		if(mHornSprite != null)
		{
			mHornSprite.color = Color.Lerp (mHornSprite.color, Color.white,0.1f);
		}
	}
	
	
	public override void TakeDamage(){
		
		health --;
		base.TakeDamage ();

		//For flashing when hit ~Adam
		if(mHornSprite != null)
		{
			mHornSprite.color = Color.Lerp (mHornSprite.color, Color.red,1f);
		}
		
		if (health <= 0) {
			
			BlowUpHorn();
		}
	}
	
	public void BlowUpHorn(){
		
		if (hornNumber == HornList.LeftHorn) {
			
			GetComponentInParent<DuoBossReak> ().leftHornAlive = false;
		} else {
			
			GetComponentInParent<DuoBossReak> ().rightHornAlive = false;
		}
		if(mDeathEffect != null)
		{
			if(mExplosionPoint !=null)
			{
				Instantiate(mDeathEffect, mExplosionPoint.position, Quaternion.identity);
			}
			else
			{
				Instantiate(mDeathEffect, transform.position, Quaternion.identity);
			}
		}
		Destroy (gameObject);
	}
}
