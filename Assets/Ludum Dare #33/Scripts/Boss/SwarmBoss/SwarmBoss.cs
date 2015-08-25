using UnityEngine;
using System.Collections;

public class SwarmBoss : BossGenericScript 
{


	// Update is called once per frame
	public override void Update () 
	{
//		if(FindObjectOfType<FakeEnemy>() == null)
//		{
//			mDying = true;
//		}
		base.Update ();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player Bullet")
		{
			Destroy(this.gameObject);
		}
	}
}
