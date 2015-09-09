using UnityEngine;
using System.Collections;

public class RepairStation : MonoBehaviour 
{
	bool mServicedP1 = false;
	bool mServicedP2 = false;
	public float mDriftSpeed = 9.2f;
	[SerializeField] private Animator mAnimator;
	[SerializeField] private GameObject mReadyGetter;

	//For keeping player shields from running out while they choose what to repair ~Adam
	ScoreManager mScoreMan;
	public float mP1ShieldTime = 0f;
	public float mP2ShieldTime = 0f;

	// Use this for initialization
	void Start () 
	{
		mScoreMan = FindObjectOfType<ScoreManager>();
		//Find the shields to keep sustained ~Adam
		if(mScoreMan.mPlayerAvatar!=null)
		{
			mP1ShieldTime = mScoreMan.mPlayerAvatar.GetComponent<PlayerShipController>().mShieldTimer;
		}
		if(FindObjectOfType<PlayerTwoShipController>()!=null)
		{
			mP2ShieldTime = mScoreMan.mPlayer2Avatar.GetComponent<PlayerShipController>().mShieldTimer;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{


		transform.Translate (Vector3.down* mDriftSpeed*Time.deltaTime);
		//Activate the GetReady object once the repair station goes off-screen ~Adam
		if(transform.position.y < -46f)
		{
			if(mReadyGetter != null)
			{
				mReadyGetter.SetActive(true);
			}
			Destroy (this.gameObject);
		}


		//Freeze the timer on the player shields ~Adam
		if(FindObjectOfType<PlayerOneShipController>()!=null)
		{
			mScoreMan.mPlayerAvatar.GetComponent<PlayerShipController>().mShieldTimer = mP1ShieldTime;
		}
		if(FindObjectOfType<PlayerTwoShipController>()!=null)
		{
			mScoreMan.mPlayer2Avatar.GetComponent<PlayerShipController>().mShieldTimer = mP2ShieldTime;
		}
	}//END of Update()

	void OnTriggerEnter(Collider other)
	{
		//If Player 1 goes through ~Adam
		if(other.GetComponent<PlayerShipController>() != null && ! mServicedP1)
		{
			float xDist = other.transform.position.x-transform.position.x;
			Debug.Log ("Player 1 entered, " + xDist);
			//If the player went through the left (HP) door ~Adam
			if(xDist < -5f && xDist > -12f)
			{
				ScoreManager scoreMan = FindObjectOfType<ScoreManager>();
				//Play animation ~Adam
				mAnimator.Play ("HealthDoor");
				//Restore lives and make sure it doesn't go over the max life count ~Adam
				scoreMan.mLivesRemaining += scoreMan.mMaxLives/5;
				if(scoreMan.mLivesRemaining > scoreMan.mMaxLives)
				{
					scoreMan.mLivesRemaining = scoreMan.mMaxLives;
				}

				//For if we want restoring lives to fix a little bit of movement/firing
//				other.GetComponent<PlayerShipController>().mMoveUpgrade += 0.07f;
//				other.GetComponent<PlayerShipController>().mFireUpgrade += 0.1f;
//				if(other.GetComponent<PlayerShipController>().mFireUpgrade > 1.2f)
//				{
//					other.GetComponent<PlayerShipController>().mFireUpgrade = 1.2f;
//				}
//				if(other.GetComponent<PlayerShipController>().mMoveUpgrade > 1.2f)
//				{
//					other.GetComponent<PlayerShipController>().mMoveUpgrade = 1.2f;
//				}
				mServicedP1 = true;
			}
			//If the player went through the center (Move Speed) door ~Adam
			else if(xDist <= 5f && xDist >= -5f)
			{
				//Play animation ~Adam
				mAnimator.Play ("MovementDoor");
				//Upgrade Move speed ~Adam
				other.GetComponent<PlayerShipController>().mMoveUpgrade += 0.25f;
				if(other.GetComponent<PlayerShipController>().mMoveUpgrade > 1.2f)
				{
					other.GetComponent<PlayerShipController>().mMoveUpgrade = 1.2f;
				}
				mServicedP1 = true;
			}
			//If the player went through the right (Fire Speed) door ~Adam
			else if(xDist > 5f && xDist < 12f)
			{
				//Play animation ~Adam
				mAnimator.Play ("FireDoor");
				//Upgrade fire rate ~Adam
				other.GetComponent<PlayerShipController>().mFireUpgrade += 0.3f;
				if(other.GetComponent<PlayerShipController>().mFireUpgrade > 1.2f)
				{
					other.GetComponent<PlayerShipController>().mFireUpgrade = 1.2f;
				}
				mServicedP1 = true;
			}
		}
		//If Player 2 goes through ~Adam
		else if(other.GetComponent<PlayerTwoShipController>() != null && ! mServicedP2)
		{
			Debug.Log ("Player 2 entered");
			float xDist = other.transform.position.x-transform.position.x;
			//If the player went through the left (HP) door ~Adam
			if(xDist < -5f && xDist > -12f)
			{
				ScoreManager scoreMan = FindObjectOfType<ScoreManager>();
				//Play animation ~Adam
				mAnimator.Play ("HealthDoor");
				//Restore lives and make sure it doesn't go over the max life count ~Adam
				scoreMan.mLivesRemaining += 6;
				if(scoreMan.mLivesRemaining > scoreMan.mMaxLives)
				{
					scoreMan.mLivesRemaining = scoreMan.mMaxLives;
				}
				//For if we want restoring lives to fix a little bit of movement/firing
//				other.GetComponent<PlayerShipController>().mMoveUpgrade += 0.07f;
//				other.GetComponent<PlayerShipController>().mFireUpgrade += 0.1f;
//				if(other.GetComponent<PlayerShipController>().mFireUpgrade > 1.2f)
//				{
//					other.GetComponent<PlayerShipController>().mFireUpgrade = 1.2f;
//				}
//				if(other.GetComponent<PlayerShipController>().mMoveUpgrade > 1.2f)
//				{
//					other.GetComponent<PlayerShipController>().mMoveUpgrade = 1.2f;
//				}
				mServicedP2 = true;
			}
			//If the playe went through the center (Move Speed) door ~Adam
			else if(xDist <= 5f && xDist >= -5f)
			{
				//Play animation ~Adam
				mAnimator.Play ("MovementDoor");
				//Upgrade Move speed ~Adam
				other.GetComponent<PlayerTwoShipController>().mMoveUpgrade += 0.25f;
				if(other.GetComponent<PlayerShipController>().mMoveUpgrade > 1.2f)
				{
					other.GetComponent<PlayerShipController>().mMoveUpgrade = 1.2f;
				}
				mServicedP2 = true;
			}
			//If the playe went through the right (Fire Speed) door ~Adam
			else if(xDist > 5f && xDist < 12f)
			{
				//Play animation ~Adam
				mAnimator.Play ("FireDoor");
				//Upgrade fire rate ~Adam
				other.GetComponent<PlayerTwoShipController>().mFireUpgrade += 0.3f;
				if(other.GetComponent<PlayerShipController>().mFireUpgrade > 1.2f)
				{
					other.GetComponent<PlayerShipController>().mFireUpgrade = 1.2f;
				}
				mServicedP2 = true;
			}
		}
	}
}
