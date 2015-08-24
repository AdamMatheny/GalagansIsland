﻿using UnityEngine;
using System.Collections;

public class BossEye : MonoBehaviour 
{
	public BossGenericScript mBossBody;

	public GameObject BuildUp;
	
	public GameObject mTarget;
	
	public GameObject bullet;

	public int health;
	
	public float timer;
	float timerTemp;

	public SpriteRenderer mMainBodySprite;

	public void Start(){
		
		mTarget = GameObject.FindGameObjectWithTag ("Player");
		
		timerTemp = timer;
	}
	
	public void Update()
	{
		//For flashing when hit ~Adam
		if(mMainBodySprite != null)
		{
			mMainBodySprite.color = Color.Lerp (mMainBodySprite.color, Color.white,0.1f);
		}

		if(mTarget == null)
		{
			mTarget = GameObject.FindGameObjectWithTag ("Player");
		}

		if (timerTemp < 1) {

			BuildUp.SetActive (true);
		} else {

			BuildUp.SetActive(false);
		}
		
		if (timerTemp > 0) {
			
			timerTemp -= Time.deltaTime;
		} else {
			
			timerTemp = timer;
			Instantiate(bullet, transform.position + new Vector3(0, 4), Quaternion.identity);
			Debug.Log("SHOOT!");
		}
		
		float horizontal = Input.GetAxis ("RightAnalogHorizontal");
		float vertical = Input.GetAxis ("RightAnalogVertical");
		
		transform.localPosition = new Vector2 (horizontal / 15, (vertical / 15) + .04f);
	}

	public void TakeDamage(){

		if (GetComponentInParent<Boss1> ().leftHornAlive == false) {

			if (GetComponentInParent<Boss1> ().rightHornAlive == false) {
				
				health --;
				//For flashing when hit ~Adam
				if(mMainBodySprite != null)
				{
					mMainBodySprite.color = Color.Lerp (mMainBodySprite.color, Color.red, 1f);
				}
			}
		}

		if (health <= 0) {

			BlowUpEye();
		}



	}

	public void BlowUpEye()
	{
		mBossBody.mDying = true;
		Destroy (gameObject);

	}
}
