﻿using UnityEngine;
using System.Collections;

public class DuoBossCollider : BossWeakPoint {
	
	public int health;

	public GameObject mDeathEffect;
	public Transform mExplosionPoint;
	
	public override void Update()
	{

	}

	public override void TakeDamage(){
		
		health --;
		
		if (health <= 0) {
			
			BlowUpHorn();
		}
	}

	private IEnumerator WaitDeath(){

		yield return new WaitForSeconds (3);
		Destroy (gameObject);
	}
	
	public void BlowUpHorn(){

		if(mDeathEffect != null)
		{
			if(mExplosionPoint !=null)
			{
				GameObject deathEffect = Instantiate(mDeathEffect, mExplosionPoint.position, Quaternion.identity) as GameObject;
				deathEffect.SetActive(true);
			}
			else
			{
				GameObject deathEffect = Instantiate(mDeathEffect, transform.position, Quaternion.identity) as GameObject;
				deathEffect.SetActive(true);
			}
		}
		//Destroy (gameObject);
	}
}
