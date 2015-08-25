using UnityEngine;
using System.Collections;

public class BlobWeakPoint : BossWeakPoint 
{
	public GameObject mBossBody;
	public SpriteRenderer mMainBodySprite;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public override void TakeDamage()
	{
		

		mBossBody.GetComponent<BlobBoss>().mhealth --;
		//For flashing when hit ~Adam
		if(mMainBodySprite != null)
		{
			mMainBodySprite.color = Color.Lerp (mMainBodySprite.color, Color.red, 1f);
		}
	
		

		
	}
}
