using UnityEngine;
using System.Collections;

public class SwarmBoss : BossGenericScript 
{
	public float mAutoDieTimer = 60f;

	//Set Starting health ~Adam
	public override void Start ()
	{
		base.Start ();
		mCurrentHealth = 100f;
		mTotalHealth = 100f;
	}

	// Update is called once per frame
	public override void Update () 
	{
		mAutoDieTimer -= Time.deltaTime;
		Time.timeScale = 1f;


		if(mAutoDieTimer <= 0f)
		{
			foreach(FakeEnemy faker in FindObjectsOfType<FakeEnemy>())
			{
				Destroy (faker.gameObject);
			}
			foreach(EnemyShipAI enemy in FindObjectsOfType<EnemyShipAI>())
			{
				enemy.EnemyShipDie();
			}
		}

		if(FindObjectOfType<FakeEnemy>() == null && FindObjectOfType<EnemyShipAI>() == null)
		{
			mDying = true;
		}
		//Figure out how much "health" this swarm has left ~Adam
		else
		{
			mCurrentHealth = 0;
			foreach(FakeEnemy faker in FindObjectsOfType<FakeEnemy>())
			{
				mCurrentHealth ++;
				
			}
			foreach(EnemyShipAI enemy in FindObjectsOfType<EnemyShipAI>())
			{
				mCurrentHealth ++;
//				if(mHero != null)
//				{
//					enemy.mPlayer = mHero.transform;
//				}
			}
		}
		base.Update ();
	}


}
