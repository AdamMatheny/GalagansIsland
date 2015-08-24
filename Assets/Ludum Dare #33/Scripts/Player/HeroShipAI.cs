using UnityEngine;
using System.Collections;

public class HeroShipAI : MonoBehaviour 
{
	public Transform mTarget;
	public int mHitsRemaining = 10;

	public GameObject mHeroBullet;
	public GameObject mDodgeObject;


	public float mSpeed = 16f;
	public Vector3 mMoveDir = Vector3.zero;

	public float mShootTimerDefault = 0.1f;
	public float mShootTimer = 2f;
	public Transform mBulletSpawnPoint;

	public float mDodgeTimer = 0f;

	[SerializeField] private GameObject mShipSprite;
	[SerializeField] private ParticleSystem mThrusters;

	public float mInvincibleTimer = 0f;
	[SerializeField] private ParticleSystem mHitEffect;

	public GameObject mDeathEffect;
	public GameObject mNextHeroShip;

	bool mHasEntered = false;

	// Use this for initialization
	void Start () 
	{
		//Find the Boss ~Adam
		if(mTarget == null)
		{
			if(FindObjectOfType<BossGenericScript>() != null)
			{
				mTarget = FindObjectOfType<BossGenericScript>().transform;
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Find the Boss ~Adam
		if(mTarget == null)
		{
			if(FindObjectOfType<BossGenericScript>() != null)
			{
				mTarget = FindObjectOfType<BossGenericScript>().transform;
			}
		}

		//Toggle hit effect sparks ~Adam
		if(mInvincibleTimer >= 0f)
		{
			mInvincibleTimer -= Time.deltaTime;
			if(mHitEffect.isStopped)
			{
				mHitEffect.Play();
			}
		}
		else if(mHitEffect.isPlaying)
		{
			mHitEffect.Stop();
		}


		//Dodge away ~Adam
		if(mDodgeTimer <= 0f)
		{
			mMoveDir = Vector3.Normalize ((mTarget.position+(Vector3.down*20f))-transform.position);

		}
		//Try to get under the target point ~Adam
		else
		{
			mMoveDir = Vector3.Normalize (transform.position-mDodgeObject.transform.position);
			mDodgeTimer -= Time.deltaTime;
		}

		//Shoot ~Adam
		if(mShootTimer <= 0f)
		{
			FireHeroBullet ();
			mShootTimer = mShootTimerDefault;
		}

		//Adjust for speed and don't move on the Z axis ~Adam
		mMoveDir *= mSpeed * 0.01f;
		mMoveDir = new Vector3(mMoveDir.x, mMoveDir.y, 0f);




		//Don't let the ship leave the bounds of the screen ~Adam
		if(!mHasEntered)
		{
			if(transform.position.y > -33f)
			{
				mHasEntered = true;
			}
		}
		else
		{
			//Count down the shoot timer ~Adam
			mShootTimer -= Time.deltaTime;

			//Keep ship within screen bounds
			if(transform.position.x < -20f)
			{
				transform.position = new Vector3(-20f, transform.position.y, transform.position.z);
				mMoveDir*=-1f;
				mDodgeTimer = 0f;
			}
			if (transform.position.x > 20f)
			{
				transform.position = new Vector3(20f, transform.position.y, transform.position.z);
				mMoveDir*=-1f;
				mDodgeTimer = 0f;
			}

			if(transform.position.y < -33f)
			{
				transform.position = new Vector3(transform.position.x, -33f, transform.position.z);
				mMoveDir*=-1f;
				mDodgeTimer = 0f;
			}
			if (transform.position.y > 23f)
			{
				transform.position = new Vector3(transform.position.x, 23, transform.position.z);
				mMoveDir*=-1f;
				mDodgeTimer = 0f;
			}
		}


		//Move the ship ~Adam
		transform.Translate(mMoveDir);

		//Animate the ship ~Adam
		if(mShipSprite.GetComponent<Animator>() != null)
		{
			//Always firing ~Adam
			mShipSprite.GetComponent<Animator>().SetBool ("IsFiring", true);
			//Ship flying left ~Adam
			if(mMoveDir.x <= -0.02f)
			{
				mShipSprite.GetComponent<Animator>().SetInteger ("Direction", -1);
			}
			//Ship flying right ~Adam
			else if(mMoveDir.x >= 0.02f)
			{
				mShipSprite.GetComponent<Animator>().SetInteger ("Direction", 1);
			}
			//Ship flying straight/hovering
			else
			{
				mShipSprite.GetComponent<Animator>().SetInteger ("Direction", 0);
			}
		}

		//Toggle thrusters ~Adam
		if(mThrusters != null)
		{
			if(mMoveDir.y >= -0.02f && mThrusters.isStopped)
			{
				mThrusters.Play ();
			}
			else if(mMoveDir.y < -0.02f)
			{
				mThrusters.Stop();
			}
		}


		//For debug testing hero ship damage
		if(Input.GetKeyDown(KeyCode.K))
		{
			HitHeroShip();
		}

	}//END of Update()

	void OnTriggerEnter(Collider other)
	{

		if(other.gameObject != this.gameObject && other.tag != "Player Bullet" && mInvincibleTimer <= 0f)
		{
			//Debug.Log ("enter "+other.gameObject.name);
			mDodgeTimer = 1f;
			mDodgeObject = other.gameObject;
			mMoveDir = Vector3.Normalize (transform.position-mDodgeObject.transform.position);
		}
	}//END of OnTriggerEnter()

	void OnTriggerStay(Collider other)
	{
		if(other.gameObject != this.gameObject && other.tag != "Player Bullet" && mInvincibleTimer <= 0f)
		{
			//Debug.Log ("Stay "+other.gameObject.name);
			if(other.gameObject.tag == "Enemy Bullet"){

				HitHeroShip();
			}
			mDodgeTimer = 1f;
			mDodgeObject = other.gameObject;
			mMoveDir = Vector3.Normalize (transform.position-mDodgeObject.transform.position);
		}
	}//END of OnTriggerStay()

	void FireHeroBullet()
	{
		Instantiate (mHeroBullet, mBulletSpawnPoint.position, mBulletSpawnPoint.rotation* Quaternion.Euler (0f,0f,Random.Range(-3.0f,3.0f)));
	}//END of FireHeroBullet()

	public void HitHeroShip()
	{
		if(mInvincibleTimer <= 0f)
		{
			mInvincibleTimer = 5f;
			mHitsRemaining --;
			if(GetComponent<AudioSource>() != null)
			{
				GetComponent<AudioSource>().Play();
			}

			//If this was the last hit, destroy self and spawn next ship
			if(mHitsRemaining <= 0)
			{
				if(mDeathEffect != null)
				{
					Instantiate (mDeathEffect, transform.position, Quaternion.identity);
				}
				if(mNextHeroShip != null)
				{
					Instantiate (mNextHeroShip, new Vector3(0f,-40f, -2f), Quaternion.identity);
				}
				Destroy(this.gameObject);
			}
		}
	}//END of HitHeroShip()
}
