using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DuoBossReak : BossGenericScript 
{
	
	public bool leftHornAlive = true;
	public bool rightHornAlive = true;
	
	//public SpriteRenderer spriter;
	
	public GameObject tail;
	public GameObject head;
	public GameObject stomach;
	public GameObject mBile;
	public GameObject mFlame;



	public override void Start ()
	{
		//spriter = GetComponent<SpriteRenderer> ();
		base.Start ();
	}
	
	public override void Update ()
	{
		if (!rightHornAlive) 
		{
			
			tail.SetActive(false);
		}

		if(!leftHornAlive)
		{
				
			head.SetActive(false);
		}
		if(!leftHornAlive && !rightHornAlive)
		{
					
			stomach.SetActive(false);
			mBile.SetActive(true);
		}

		if(mDying)
		{
			mFlame.SetActive (false);
		}
		base.Update ();

	}

}
