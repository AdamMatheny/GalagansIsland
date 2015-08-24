using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BossGenericScript : MonoBehaviour 
{

	//Put generic boss code here
	public List<Transform> mWeakPoints = new List<Transform>();

	public HeroShipAI mHero;

	//public int health;

	public Rigidbody2D rgb2d;

	//For Dying and spawning the next boss ~Adam
	public bool mDying = false;
	public float mDeathTimer = 5f;
	public GameObject mDeathEffect;
	public GameObject mNextBoss;

	public virtual void Start()
	{
	
		if(mHero == null)
		{
			mHero = FindObjectOfType<HeroShipAI>();
		}
		rgb2d = GetComponent<Rigidbody2D> ();
	}

	public virtual void Update()
	{
		if(mHero == null)
		{
			mHero = FindObjectOfType<HeroShipAI>();
		}
		else
		{
			mHero.mTarget = mWeakPoints[0];
		}

		if(mWeakPoints[0] == null)
		{
			mWeakPoints.Remove(null);
		}

		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		rgb2d.velocity = new Vector2 (horizontal * 10, vertical * 10);

		//Die and spawn the next boss ~Adam
		if(mDying == true)
		{
			mDeathTimer -= Time.deltaTime;
			if(mDeathEffect != null)
			{
				mDeathEffect.SetActive (true);
			}
			if(mDeathTimer <= 0f)
			{
				if(mNextBoss != null)
				{
					Instantiate (mNextBoss, new Vector3(0f,0f,-2f), Quaternion.identity);
				}
				Destroy (this.gameObject);
			}
		}
	}

	//public void TakeDamage(){

		//health --;
	//}
}