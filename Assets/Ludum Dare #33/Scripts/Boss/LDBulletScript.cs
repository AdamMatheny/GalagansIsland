﻿using UnityEngine;
using System.Collections;

public class LDBulletScript : EnemyBulletController 
{
	public int mHitDamage = 1;

	public void OnCollisionEnter(Collision other)
	{
		
		if (other.gameObject.tag == "Player") 
		{
			if(other.gameObject.GetComponent<HeroShipAI>().mInvincibleTimer <= 0f)
			{
				other.gameObject.GetComponent<HeroShipAI>().HitHeroShip(mHitDamage);
			}
			Destroy(gameObject);
		}

		else if (other.gameObject.name == "ShipCore") 
		{
			if(other.transform.parent.gameObject.GetComponent<HeroShipAI>().mInvincibleTimer <= 0f)
			{
				other.transform.parent.gameObject.GetComponent<HeroShipAI>().HitHeroShip(mHitDamage);
			}
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		
		if (other.gameObject.tag == "Player") 
		{
			if(other.gameObject.GetComponent<HeroShipAI>().mInvincibleTimer <= 0f)
			{
				other.gameObject.GetComponent<HeroShipAI>().HitHeroShip(mHitDamage);
			}
			Destroy(gameObject);
		}


		else if (other.gameObject.name == "ShipCore") 
		{
			if(other.transform.parent.gameObject.GetComponent<HeroShipAI>().mInvincibleTimer <= 0f)
			{
				other.transform.parent.gameObject.GetComponent<HeroShipAI>().HitHeroShip(mHitDamage);
			}
		}
	}
}
