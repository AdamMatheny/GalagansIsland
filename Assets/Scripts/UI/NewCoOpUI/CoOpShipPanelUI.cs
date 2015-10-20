using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CoOpShipPanelUI : MonoBehaviour 
{
	public bool mP2UI = false;

	ScoreManager mScoreMan;
	
	[SerializeField] private PlayerOneShipController mP1Ship;
	[SerializeField] private PlayerTwoShipController mP2Ship;

	[SerializeField] private float mHealthValue = 0f;
	[SerializeField] private float mShieldValue = 0f;
	[SerializeField] private float mOverheatValue = 0f;
	[SerializeField] private float mTripleTimerValue = 0f;

	public Image mHealthBar;
	//public Image mShieldBar;
	public Image mShieldCircleBar;
	public Color mShieldFillColor;
	public Color mShieldPulseColor;
	float mShieldPulseTime;
	public Text mShieldedLabel;

	public Image mOverheatBar;
	public Image mTripleTimerBar;

	public Text mScoreText;

	//UI Ship pieces ~Adam
	public GameObject mShipHull;
	public GameObject mShipLeftWing;
	public GameObject mShipRightWing;
	public GameObject mShipLeftClaw;
	public GameObject mShipRightClaw;
	public GameObject mShipLeftGun;
	public GameObject mShipRightGun;

	//For playing the overheat whistle noise
	public bool mCanPlaySteamNoise = true;

	// Use this for initialization
	void Start () 
	{
		//Find the score manager and the player ships ~Adam
		if(FindObjectOfType<ScoreManager>() != null)
		{
			mScoreMan = FindObjectOfType<ScoreManager>();
		}
		if(FindObjectOfType<PlayerOneShipController>() != null)
		{
			mP1Ship = FindObjectOfType<PlayerOneShipController>();
		}
		if(FindObjectOfType<PlayerTwoShipController>() != null)
		{
			mP2Ship = FindObjectOfType<PlayerTwoShipController>();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(mScoreMan != null)
		{
//			mHealthValue = mScoreMan.mLivesRemaining/(mScoreMan.mMaxLives+.00001f);

			if(!mP2UI)
			{
				mHealthValue = mScoreMan.mP1Lives/(mScoreMan.mMaxLives+.00001f);
			}
			else
			{
				mHealthValue = mScoreMan.mP2Lives/(mScoreMan.mMaxLives+.00001f);
			}

			//Flash the ship parts ~Adam
			if(mScoreMan.mPlayerSafeTime >0f)
			{
				mShipHull.GetComponent<Animator>().SetInteger("UIFlashState", 1);
				mShipLeftWing.GetComponent<Animator>().SetInteger("UIFlashState", 1);
				mShipRightWing.GetComponent<Animator>().SetInteger("UIFlashState", 1);
				mShipLeftClaw.GetComponent<Animator>().SetInteger("UIFlashState", 1);
				mShipRightClaw.GetComponent<Animator>().SetInteger("UIFlashState", 1);
			}
			else
			{
				mShipHull.GetComponent<Animator>().SetInteger("UIFlashState", 0);
				mShipLeftWing.GetComponent<Animator>().SetInteger("UIFlashState", 0);
				mShipRightWing.GetComponent<Animator>().SetInteger("UIFlashState", 0);
				mShipLeftClaw.GetComponent<Animator>().SetInteger("UIFlashState", 0);
				mShipRightClaw.GetComponent<Animator>().SetInteger("UIFlashState", 0);
			}
		}

//		if(mHealthValue<0.8f)
//		{
//			mShipRightClaw.GetComponent<Animator>().SetInteger("UIFlashState", 2);
//		}
//		if(mHealthValue<0.6f)
//		{
//			mShipLeftClaw.GetComponent<Animator>().SetInteger("UIFlashState", 2);
//		}
//		if(mHealthValue<0.4f)
//		{
//			mShipRightWing.GetComponent<Animator>().SetInteger("UIFlashState", 2);
//		}
//		if(mHealthValue<0.2f)
//		{
//			mShipLeftWing.GetComponent<Animator>().SetInteger("UIFlashState", 2);
//		}
		if(mHealthValue < 0f)
		{
			mHealthValue = 0f;
		}


		if(!mP2UI && mP1Ship != null)
		{
			//Adjust the meters ~Adam
			mOverheatValue = mP1Ship.heatLevel/mP1Ship.maxHeatLevel;
			if(mOverheatValue < 0f)
			{
				mOverheatValue = 0f;
			}

			mShieldValue = mP1Ship.mShieldTimer/30f;
			if(mShieldValue < 0f)
			{
				mShieldValue = 0f;
			}

			mTripleTimerValue = mP1Ship.mThreeBulletTimer/30f;
			if(mTripleTimerValue < 0f)
			{
				mTripleTimerValue = 0f;
			}

			//Adjust Claw/Wing graphics based on damage/upgrade status ~Adam
			if(mP1Ship.mFireUpgrade < 0.8f)
			{
				mShipRightClaw.GetComponent<Animator>().SetInteger("UIFlashState", 2);
			}
			if(mP1Ship.mFireUpgrade < 0.6f)
			{
				mShipLeftClaw.GetComponent<Animator>().SetInteger("UIFlashState", 2);
			}
			if(mP1Ship.mMoveUpgrade < 0.8f)
			{
				mShipRightWing.GetComponent<Animator>().SetInteger("UIFlashState", 2);
			}
			if(mP1Ship.mMoveUpgrade < 0.6f)
			{
				mShipLeftWing.GetComponent<Animator>().SetInteger("UIFlashState", 2);
			}

			//Flash the ship gun parts ~Adam
			if(mScoreMan != null)
			{
				if(mP1Ship.mThreeBullet)
				{
					if(mScoreMan.mPlayerSafeTime >0f)
					{
						mShipLeftGun.GetComponent<Animator>().SetInteger("UIFlashState", 1);
						mShipRightGun.GetComponent<Animator>().SetInteger("UIFlashState", 1);
					}
					else
					{
						mShipLeftGun.GetComponent<Animator>().SetInteger("UIFlashState", 0);
						mShipRightGun.GetComponent<Animator>().SetInteger("UIFlashState", 0);
					}
				}
				else
				{
					mShipLeftGun.GetComponent<Animator>().SetInteger("UIFlashState", 2);
					mShipRightGun.GetComponent<Animator>().SetInteger("UIFlashState", 2);
				}
			}
			//Show this player's individual score ~Adam
			mScoreText.text = "P1 Score: " + mScoreMan.mP1Score;
		}
		else if(mP2UI && mP2Ship != null)
		{
			//Adjust the meters ~Adam
			mOverheatValue = mP2Ship.heatLevel/mP2Ship.maxHeatLevel;
			if(mOverheatValue < 0f)
			{
				mOverheatValue = 0f;
			}

			mShieldValue = mP2Ship.mShieldTimer/30f;
			if(mShieldValue < 0f)
			{
				mShieldValue = 0f;
			}


			mTripleTimerValue = mP2Ship.mThreeBulletTimer/30f;
			if(mTripleTimerValue < 0f)
			{
				mTripleTimerValue = 0f;
			}


			//Adjust Claw/Wing graphics based on damage/upgrade status ~Adam
			if(mP2Ship.mFireUpgrade < 0.9f)
			{
				mShipRightClaw.GetComponent<Animator>().SetInteger("UIFlashState", 2);
			}
			if(mP2Ship.mFireUpgrade < 0.8f)
			{
				mShipLeftClaw.GetComponent<Animator>().SetInteger("UIFlashState", 2);
			}
			if(mP2Ship.mMoveUpgrade < 0.9f)
			{
				mShipRightWing.GetComponent<Animator>().SetInteger("UIFlashState", 2);
			}
			if(mP2Ship.mMoveUpgrade < 0.8f)
			{
				mShipLeftWing.GetComponent<Animator>().SetInteger("UIFlashState", 2);
			}

			//Flash the ship gun parts ~Adam
			if(mScoreMan != null)
			{
				if(mP2Ship.mThreeBullet)
				{
					if(mScoreMan.mPlayerSafeTime >0f)
					{
						mShipLeftGun.GetComponent<Animator>().SetInteger("UIFlashState", 1);
						mShipRightGun.GetComponent<Animator>().SetInteger("UIFlashState", 1);
					}
					else
					{
						mShipLeftGun.GetComponent<Animator>().SetInteger("UIFlashState", 0);
						mShipRightGun.GetComponent<Animator>().SetInteger("UIFlashState", 0);
					}
				}
				else
				{
					mShipLeftGun.GetComponent<Animator>().SetInteger("UIFlashState", 2);
					mShipRightGun.GetComponent<Animator>().SetInteger("UIFlashState", 2);
				}
			}

			//Show this player's individual score ~Adam
			mScoreText.text = "P2 Score: " + mScoreMan.mP2Score;
		}

		//Control the overheat whistle noise
		if(mOverheatValue  < 0.9f && GetComponent<AudioSource>().isPlaying)
		{
			mCanPlaySteamNoise = true;
		}
		else if (mOverheatValue > 0.9f && mCanPlaySteamNoise)
		{
			GetComponent<AudioSource>().Play();
			mCanPlaySteamNoise = false;
		}


		//Find ships if they're null
		else if (!mP2UI && mP1Ship == null)
		{
			if(FindObjectOfType<PlayerOneShipController>() != null)
			{
				mP1Ship = FindObjectOfType<PlayerOneShipController>();
			}
		}
		else if (mP2UI && mP2Ship == null)
		{
			if(FindObjectOfType<PlayerTwoShipController>() != null)
			{
				mP2Ship = FindObjectOfType<PlayerTwoShipController>();
			}
		}

		//Set the bar sizes ~Adam
		mHealthBar.rectTransform.localScale = new Vector3(mHealthValue, 1f,1f);
		//mShieldBar.rectTransform.localScale = new Vector3(mShieldValue, 1f,1f);
		mShieldCircleBar.fillAmount = mShieldValue;
		if(mShieldValue > 0f)
		{
			mShieldedLabel.gameObject.SetActive (true);
			mShieldPulseTime += Time.deltaTime/Time.timeScale;
			if(mShieldPulseTime < 0.5f)
			{
				mShieldCircleBar.color = Color.Lerp(mShieldCircleBar.color, mShieldFillColor, 0.1f);
			}
			else if(mShieldPulseTime < 1f)
			{
				if(mShieldValue < 5f/30f)
				{
					mShieldCircleBar.color = Color.Lerp(mShieldCircleBar.color, Color.red, 0.1f);
				}
				else
				{
					mShieldCircleBar.color = Color.Lerp(mShieldCircleBar.color, mShieldPulseColor, 0.1f);
				}
			}
			else
			{
				mShieldPulseTime = 0f;
			}
		}
		else
		{
			mShieldedLabel.gameObject.SetActive (false);
		}
		mOverheatBar.rectTransform.localScale = new Vector3(mOverheatValue, 1f,1f);
		mTripleTimerBar.rectTransform.localScale = new Vector3(mTripleTimerValue, 1f,1f);
	}
}
