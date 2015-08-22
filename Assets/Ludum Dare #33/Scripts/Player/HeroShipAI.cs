using UnityEngine;
using System.Collections;

public class HeroShipAI : MonoBehaviour 
{
	public Transform mTarget;
	public GameObject mHeroBullet;

	public float mSpeed = 16f;
	public Vector3 mMoveDir = Vector3.zero;

	public float mShootTimerDefault = 0.1f;
	public float mShootTimer;
	public Transform mBulletSpawnPoint;

	public float mDodgeTimer = 0f;


	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(mDodgeTimer <= 0f)
		{
			mMoveDir = Vector3.Normalize ((mTarget.position+(Vector3.down*20f))-transform.position);

		}
		else
		{
			mDodgeTimer -= Time.deltaTime;
		}

		mShootTimer -= Time.deltaTime;
		if(mShootTimer <= 0f)
		{
			FireHeroBullet ();
			mShootTimer = mShootTimerDefault;
		}

		//Try to get under whatever is shooting at us ~Adam
		mMoveDir *= mSpeed * 0.01f;

		//Move the ship ~Adam
		transform.Translate(mMoveDir);
	}

	void OnTriggerEnter(Collider other)
	{

		if(other.gameObject != this.gameObject)
		{
			mDodgeTimer = 1f;
			mMoveDir = Vector3.Normalize (transform.position-other.transform.position);
		}
	}
	void OnTriggerStay(Collider other)
	{
		if(other.gameObject != this.gameObject)
		{
			mDodgeTimer = 1f;
			mMoveDir = Vector3.Normalize (transform.position-other.transform.position);
		}
	}

	void FireHeroBullet()
	{
		Instantiate (mHeroBullet, mBulletSpawnPoint.position, mBulletSpawnPoint.rotation* Quaternion.Euler (0f,0f,Random.Range(-3.0f,3.0f)));
	}
}
