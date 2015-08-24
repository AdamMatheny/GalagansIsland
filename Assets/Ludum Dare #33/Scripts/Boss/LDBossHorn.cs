using UnityEngine;
using System.Collections;

public class LDBossHorn : MonoBehaviour {

	public int health;

	public enum HornList{

		LeftHorn,
		RightHorn
	}

	public HornList hornNumber;

	//For Flashing when hit ~Adam
	public SpriteRenderer mHornSprite;

	public GameObject mDeathEffect;

	void Update()
	{
		//For flashing when hit ~Adam
		if(mHornSprite != null)
		{
			mHornSprite.color = Color.Lerp (mHornSprite.color, Color.white,0.1f);
		}
	}


	public void TakeDamage(){

		health --;

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

			GetComponentInParent<Boss1> ().leftHornAlive = false;
		} else {

			GetComponentInParent<Boss1> ().rightHornAlive = false;
		}
		if(mDeathEffect != null)
		{
			Instantiate(mDeathEffect, transform.position, Quaternion.identity);
		}
		Destroy (gameObject);
	}
}
