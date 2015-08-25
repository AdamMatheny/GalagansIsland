using UnityEngine;
using System.Collections;

public class SwarmBoss : BossGenericScript 
{
	public float mAutoDieTimer = 60f;

	// Update is called once per frame
	public override void Update () 
	{
		mAutoDieTimer -= Time.deltaTime;

		if(mAutoDieTimer <= 0f)
		{
			foreach(FakeEnemy faker in FindObjectsOfType<FakeEnemy>())
			{
				Destroy (faker.gameObject);
			}
			foreach(EnemyShipAI enemy in FindObjectsOfType<EnemyShipAI>())
			{
				Destroy (enemy.gameObject);
			}
		}

		if(FindObjectOfType<FakeEnemy>() == null && FindObjectOfType<EnemyShipAI>() == null)
		{
			mDying = true;
		}
		base.Update ();
	}


}
