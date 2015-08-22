using UnityEngine;
using System.Collections;

public class HeroShipAI : MonoBehaviour 
{
	public Transform mTarget;
	public GameObject mHeroBullet;
	public float mSpeed = 16f;
	public Vector3 mMoveDir = Vector3.zero;
	
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

		//Try to get under whatever is shooting at us ~Adam
		mMoveDir *= mSpeed * 0.01f;

		//Move the ship ~Adam
		transform.Translate(mMoveDir);
	}

	void OnTriggerEnter(Collider other)
	{

		if(other.gameObject != this.gameObject)
		{
			Debug.Log ("Enter "+ other.gameObject.name);
			mDodgeTimer = 1f;
			mMoveDir = Vector3.Normalize (transform.position-other.transform.position);
		}
	}
	void OnTriggerStay(Collider other)
	{
		if(other.gameObject != this.gameObject)
		{
			Debug.Log ("Stay "+ other.gameObject.name);
			mDodgeTimer = 1f;
			mMoveDir = Vector3.Normalize (transform.position-other.transform.position);
		}
	}
}
