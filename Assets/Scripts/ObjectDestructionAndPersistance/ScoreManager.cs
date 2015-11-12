﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InControl;
using Assets.Scripts.Achievements;
//using XInputDotNetPure; // Required in C#

public class ScoreManager : MonoBehaviour 
{
	[SerializeField] private Texture2D mSideDisplayTex;

	public int mShieldLifeCount = 10;
	public int mShieldHits = 0;

	public int mScore = 0;
	public int mLivesRemaining = 24;
    public int mMaxLives = 24;
	public int mCurrentLevel; //Changed this variable name to be consistent with the rest of the naming schem ~Adam
	public int mOriginalLevel = 0;
	//For giving the player an extra life every certain number of points ~Adam
	int mExtraLifeScore = 1000;
	int mExtraLifeInteraval = 1000;

	//For spawning an triple-bullet power-up every certain number of points ~Adam
	int mPowerUpScore = 300;
	int mPowerUpInterval = 300;
	[SerializeField] private GameObject mTripleBulletEmblem;
	//For spawning a shield power-up every certain number of points ~Adam
	int mShieldScore = 600;
	int mShieldInterval = 300;
	[SerializeField] private GameObject mShieldEmblem;

	//For the UI of showing a meter depicting tim until next powerup
	[SerializeField] private Image mPowerUpMeter;
	[SerializeField] private GameObject mPowerUpMeterBack;
	public Text mPowerUpMeterScoreDisplay;

	public float mPlayerSafeTime = 0f;
	// Use this for initialization
	public GameObject mPlayerAvatar;
	public GameObject mPlayerDeathEffect;

	//For when we have two players ~Adam
	public GameObject mPlayer2Avatar;
	public GameObject mPlayer2DeathEffect;

	//For better GUI elements ~Adam
	[SerializeField] private GUIStyle mScoreManStyle;
	[SerializeField] private GUIStyle mHighScoreStyle;

	//List of level names ~Adam
	[SerializeField] private string[] mLevelNames;

	//For making the background scrolling persist between levels ~Adam
	public Vector3 mBackgroundPosition;
	public float mBackgroundOffset;

	//For using the new UI system so we can use an image for a font ~Adam
	public Text mLevelInfoText;
	public Text mHighScoreText;

    public Canvas mHighscoreCanvas;


	//For differentiating player 1 and player 2's scores ~Adam
	public int mP1Score = 0;
	public int mP2Score = 0;


	//For showing what power up is going to spawn next ~Adam
	[SerializeField] private Image mNextPowerUpImage;
	[SerializeField] private Sprite mShieldEmblemSprite;
	[SerializeField] private Sprite mTripleEmblemSprite;

	public bool mInCoOpMode = false;
	public int mP1Lives = 100;
	public int mP2Lives = 0;

	void StoreHighscore(int newHighscore)
	{
		int oldHighscore = PlayerPrefs.GetInt("highscore", 0);    
		if(newHighscore > oldHighscore)
			PlayerPrefs.SetInt("highscore", newHighscore);
	}



	void Start () 
	{
        if (Application.isMobilePlatform)
        {
            mLivesRemaining = 25;
            mMaxLives = 25;
        }
//		//Get rid of self if we're back on the title screen
//		if (Application.loadedLevel == 0)
//		{
//			Destroy(this.gameObject);
//		}

		//Delete self if there's already a score manager to prevent duplicates (this only seems to delete the new ones, which is what we want)
        foreach (var canv in gameObject.GetComponentsInChildren<Canvas>())
        {
            if (canv.transform.name == "ScoreCanvas")
            {
                mHighscoreCanvas = canv;
            }
        }
		if(mPlayer2Avatar == null && mPlayerAvatar.GetComponent<PlayerOneShipController>().mPlayerTwo.gameObject != null && mPlayerAvatar.GetComponent<PlayerOneShipController>().mPlayerTwo.gameObject.activeInHierarchy)
		{
			mPlayer2Avatar = mPlayerAvatar.GetComponent<PlayerOneShipController>().mPlayerTwo.gameObject;
		}
	}

	//Pesist between level loads/reloads ~adam
	void Awake()
	{


		DontDestroyOnLoad (transform.gameObject);
//		DontDestroyOnLoad (mPlayerAvatar.gameObject);
//		DontDestroyOnLoad (mPlayer2Avatar.gameObject);

		//Figure out how old this ScoreManager is ~Adam
		if(mOriginalLevel == 0)
		{
			mOriginalLevel = Application.loadedLevel;
		}
		ScoreManager[] otherScoreManagers = FindObjectsOfType<ScoreManager>();

		//Delete self if there's an older ScoreManager ~Adam
		foreach(ScoreManager otherScoreManager in otherScoreManagers)
		{
			if (otherScoreManager != null && otherScoreManager.mOriginalLevel < mOriginalLevel)
			{
				Destroy(this.gameObject);
			}
		}
		mPlayerAvatar = GameObject.FindGameObjectWithTag("Player").gameObject;

		if (mPlayerAvatar != null) {

			if (mPlayerAvatar.GetComponent<PlayerOneShipController> ().mPlayerTwo.gameObject != null && mPlayerAvatar.GetComponent<PlayerOneShipController> ().mPlayerTwo.gameObject.activeInHierarchy) {
				mPlayer2Avatar = mPlayerAvatar.GetComponent<PlayerOneShipController> ().mPlayerTwo.gameObject;
			}
		}
	}

	// Update is called once per frame
	void Update () 
	{

		mCurrentLevel = Application.loadedLevel; //Wasn't affected in either Awake() or Start()

		#region Mateusz, why did you add this? It make the high score box and level info box go away. ~Adam
//        if (mCurrentLevel == 0)
//        {
//            mHighscoreCanvas.enabled = false;
//        }
		#endregion

		//We already had a method of switching between levels that gave us a lag time in which to actually play a player death animation ~Adam
//		if (mLivesRemaining <= 0) {
//
//			Application.LoadLevel("EndGame");
//			Destroy(shipDeath);
//		}



		mPlayerSafeTime-=Time.deltaTime;

		/*if(mScore < 0)
		{
			mScore = 0;
		}*/

//		//Grant an extra life every 1000 kills (assuming 1 point per kill) ~Adam
//		if(mScore >= mExtraLifeScore)
//		{
//			mLivesRemaining++;
//			mExtraLifeScore += mExtraLifeInteraval;
//		}

		//For showing the meter that says how close the player is to a power up
		if(mPowerUpMeterScoreDisplay != null && mPowerUpMeter != null && mPowerUpMeterBack != null)
		{

			if(mScore < 0)
			{

				mPowerUpMeter.rectTransform.localScale = new Vector3(0f, 1f, 1f);
				mPowerUpMeterScoreDisplay.text = "Loser. Try Shooting.";
			}
			else
			{

				mPowerUpMeterScoreDisplay.text = "Total Score: " + mScore;

				if(mPowerUpScore < mShieldScore)
				{
					float barAdjust = 1f*(mPowerUpInterval-(mPowerUpScore-mScore))/mPowerUpInterval;
					if(barAdjust < 0f)
					{
						barAdjust = 0f;
					}
					mPowerUpMeter.rectTransform.localScale = new Vector3(barAdjust, 1f,1f); 
					//mPowerUpMeter.rectTransform.rect = new Rect(mPowerUpMeter.rectTransform.rect.x, mPowerUpMeter.rectTransform.rect.y, barAdjust, mPowerUpMeter.rectTransform.rect.height);
				}
				else
				{
					float barAdjust = 1f*(mShieldInterval-(mShieldScore-mScore))/mShieldInterval;
					if(barAdjust < 0f)
					{
						barAdjust = 0f;
					}
					//Debug.Log(barAdjust);
					mPowerUpMeter.rectTransform.localScale = new Vector3(barAdjust, 1f,1f); 
				}
			}
		}
		//Spawn a triple bullet power up every 500 kills (assuming 1 point per kill) ~Adam
		if(mScore >= mPowerUpScore)
		{
			float spawnXPos = Random.Range(-16f,16f);
			float spawnyPos = Random.Range(-17f,23f);
			Instantiate(mTripleBulletEmblem, new Vector3(spawnXPos, spawnyPos, -2f), Quaternion.identity);
			mPowerUpMeterBack.GetComponent<Animator>().Play("PowerPointMeterFlash_Anim");
			mPowerUpScore += (mPowerUpInterval+mShieldInterval);
		}
		//Spawn a shield power up every 300 kills (assuming 1 point per kill) ~Adam
		if(mScore >= mShieldScore)
		{
			float spawnXPos = Random.Range(-16f,16f);
			float spawnyPos = Random.Range(-17f,23f);
			Instantiate(mShieldEmblem, new Vector3(spawnXPos, spawnyPos, -2f), Quaternion.identity);
			mPowerUpMeterBack.GetComponent<Animator>().Play("PowerPointMeterFlash_Anim");
			mShieldScore += (mShieldInterval+mPowerUpInterval);
		}

		//Make sure we have a reference to the player's ship ~Adam
		if (mPlayerAvatar == null && GameObject.FindGameObjectWithTag("Player") != null)
		{
			mPlayerAvatar = GameObject.FindGameObjectWithTag("Player").gameObject;
			
		}

		//Color the player while invincible
		if(mPlayerAvatar != null && mPlayerAvatar.GetComponent<PlayerShipController>() != null)
		{
			if(mPlayerSafeTime > 0)
			{
				mPlayerAvatar.GetComponent<PlayerShipController>().mMainShipHitSprite.SetActive(true);
				mPlayerAvatar.GetComponent<PlayerShipController>().mDamageParticles.SetActive(true);
				if(mPlayerAvatar.GetComponent<PlayerShipController>().mShipRecovered)
				{
					mPlayerAvatar.GetComponent<PlayerShipController>().mSecondShipHitSprite.SetActive(true);
				}
			}
			else
			{
				mPlayerAvatar.GetComponent<PlayerShipController>().mDamageParticles.SetActive(false);
				mPlayerAvatar.GetComponent<PlayerShipController>().mMainShipHitSprite.SetActive(false);
				mPlayerAvatar.GetComponent<PlayerShipController>().mSecondShipHitSprite.SetActive(false);
			}
		}
		if(mPlayer2Avatar != null)
		{
			if(mPlayerSafeTime > 0)
			{
				mPlayer2Avatar.GetComponent<PlayerShipController>().mMainShipHitSprite.SetActive(true);
				mPlayer2Avatar.GetComponent<PlayerShipController>().mDamageParticles.SetActive(true);
				if(mPlayer2Avatar.GetComponent<PlayerShipController>().mShipRecovered)
				{
					mPlayer2Avatar.GetComponent<PlayerShipController>().mSecondShipHitSprite.SetActive(true);
				}
			}
			else
			{
				mPlayer2Avatar.GetComponent<PlayerShipController>().mDamageParticles.SetActive(false);
				mPlayer2Avatar.GetComponent<PlayerShipController>().mMainShipHitSprite.SetActive(false);
				mPlayer2Avatar.GetComponent<PlayerShipController>().mSecondShipHitSprite.SetActive(false);
			}
		}

		switch(Application.loadedLevelName)
		{
		case "Level26_Boss":
			mLevelInfoText.text = " \nGame Over";
			break;
		case "Credits":
			mLevelInfoText.text = "Thank you for playing!";
			break;
		case "EndGame":
			mLevelInfoText.text = " \nGame Over";
			break;
		default:
			if(mLevelNames[Application.loadedLevel].Contains("Boss"))
			{
				mLevelInfoText.text =  "\n" + mLevelNames[Application.loadedLevel];
			}
			else
			{
				mLevelInfoText.text = "Level "+ (Application.loadedLevel-Application.loadedLevel/6) + ":\n" + mLevelNames[Application.loadedLevel];
			}
			break;
		}



		mHighScoreText.text = "High Score:\n" + PlayerPrefs.GetInt("highscore", 0);
		//mHighScoreText.text = "Return to Wayward Pines!";

		StoreHighscore (mScore);

		//Show what power up is spawning next ~Adam
		if(mNextPowerUpImage != null && mShieldEmblemSprite != null && mTripleEmblemSprite != null)
		{
			if(mShieldScore < mPowerUpScore)
			{
				mNextPowerUpImage.sprite = mShieldEmblemSprite;
			}
			else
			{
				mNextPowerUpImage.sprite = mTripleEmblemSprite;
			}
		}

		//If we're out of lives, wait a short bit for the player explosion to play, then clean up the objects that normally persist between levels
		//Then go to the EndGame scene and delete this game object ~Adam
		if(mLivesRemaining <= 0 && mPlayerSafeTime <= 0 && (mPlayer2Avatar == null || !mPlayer2Avatar.activeInHierarchy) && (mPlayerAvatar == null || !mPlayerAvatar.activeInHierarchy))
		{

			if(FindObjectOfType<LevelKillCounter>() != null)
			{
				Destroy(FindObjectOfType<LevelKillCounter>().gameObject);
			}
			mLevelInfoText.text = "\nGame Over";
			if(GameObject.Find("PowerMeterCanvas") != null)
			{
				GameObject.Find("PowerMeterCanvas").SetActive (false);
			}
			if(mPlayerAvatar != null)
			{
				Destroy(mPlayerAvatar.gameObject);
			}
			if(mPlayer2Avatar != null)
			{
				Destroy(mPlayer2Avatar.gameObject);
			}
			Application.LoadLevel("EndGame");
			this.enabled = false;
			//Destroy(this.gameObject);
			
		}

		//Let dead players borrow lives to repsawn in Co-Op mode ~Adam
		//Only allow the repsawning in Co-Op mode while a player isn't invincible and there are still lives leftover ~Adam
		if(mInCoOpMode && mPlayerSafeTime <= 0f && mLivesRemaining > 1)
		{
			//For player 1 coming back ~Adam
			if(!mPlayerAvatar.activeInHierarchy && mPlayer2Avatar.activeInHierarchy)
			{
				if( ( (InputManager.ActiveDevice.Action1.WasPressed || InputManager.ActiveDevice.Action4.WasPressed) 
				    && InputManager.ActiveDevice != mPlayer2Avatar.GetComponent<PlayerShipController>().mPlayerInputDevice) 
				   || Input.GetButtonDown("FireGun") )
				{
					if(mP2Lives >= 10)
					{
						mP2Lives -= 5;
						mP1Lives += 5;
						if(AchievementManager.instance != null)
						{
                        	AchievementManager.instance.PostAchievement("Mortgage");
						}
					}
					else if(mP2Lives > 5)
					{
						mP2Lives -= 3;
                        mP1Lives += 3;
						if(AchievementManager.instance != null)
						{
							AchievementManager.instance.PostAchievement("Mortgage");
						}
					}
					else
					{
						mP2Lives -= 1;
                        mP1Lives += 1;
						if(AchievementManager.instance != null)
						{
							AchievementManager.instance.PostAchievement("Mortgage");
						}
					}
					mPlayerAvatar.transform.position = mPlayer2Avatar.transform.position;
					mPlayerAvatar.SetActive(true);
					mPlayerAvatar.GetComponent<PlayerShipController>().Respawn ();
				}
			}

			//For player 2 coming back ~Adam
			if(mPlayerAvatar.activeInHierarchy && !mPlayer2Avatar.activeInHierarchy)
			{
				if(( (InputManager.ActiveDevice.Action1.WasPressed || InputManager.ActiveDevice.Action4.WasPressed) 
				    && InputManager.ActiveDevice != mPlayerAvatar.GetComponent<PlayerShipController>().mPlayerInputDevice) 
				   || Input.GetButtonDown("FireGunP2") )
				{
					if(mP1Lives >= 10)
					{
						mP1Lives -= 5;
                        mP2Lives += 5;
						if(AchievementManager.instance != null)
						{
							AchievementManager.instance.PostAchievement("Mortgage");
						}
					}
					else if(mP1Lives > 5)
					{
						mP1Lives -= 3;
                        mP2Lives += 3;
						if(AchievementManager.instance != null)
						{
							AchievementManager.instance.PostAchievement("Mortgage");
						}
					}
					else
					{
						mP1Lives -= 1;
                        mP2Lives += 1;
						if(AchievementManager.instance != null)
						{
							AchievementManager.instance.PostAchievement("Mortgage");
						}
					}
					mPlayer2Avatar.transform.position = mPlayerAvatar.transform.position;
					mPlayer2Avatar.SetActive(true);
					mPlayer2Avatar.GetComponent<PlayerShipController>().Respawn ();
				}
			}

		}
	}//END of Update()



	//Used for adding/subtracting points
	public void AdjustScore(int points, bool mPlayer1Kill)
	{
		if(mScore < 0)
		{
			mP1Score = 0;
			mP2Score = 0;
			mScore = 0;
		}
		mScore += points;
		if(mPlayer1Kill)
		{
			mP1Score += points;
		}
		else
		{
			mP2Score += points;
		}
	}

	public void HalfScore()
	{
		mScore /= 2;
	}

	public void DoubleScore()
	{
		mScore *= 2;
	}
	public void LoseALife()
	{
		if(mPlayerSafeTime<=0f)
		{



			//Lose a life if the player isn't shielded ~Adam
			if(!mPlayerAvatar.GetComponent<PlayerShipController>().mShielded)
			{
				Camera.main.GetComponent<CameraShaker> ().RumbleController(.1f, .2f);

				if(mP1Lives == 1)
				{
					GameObject playerDeathParticles;
					playerDeathParticles = Instantiate(mPlayerDeathEffect, mPlayerAvatar.transform.position, Quaternion.identity) as GameObject;
				}
				if(mPlayerAvatar.GetComponent<PlayerShipController>().mShipRecovered)
				{
					mPlayerAvatar.GetComponent<PlayerShipController>().mShipRecovered = false;
					mPlayerAvatar.GetComponent<PlayerShipController>().StartSpin();
					Camera.main.GetComponent<CameraShaker>().ShakeCameraDeath();


				}
				else
				{
					mScore -= 10;
					mP1Score -= 10;
					if(mP1Score < 0)
					{
						mP2Score += mP1Score;
						mP1Score = 0;
					}
					if(mScore <-1)
					{
						mScore = -1;
						mP1Score = 0;
						mP2Score = 0;
					}
					mLivesRemaining--;
					mP1Lives--;
					mPlayerAvatar.GetComponent<PlayerShipController>().StartSpin();
					mPlayerAvatar.GetComponent<PlayerShipController>().TakeStatDamage();
					Camera.main.GetComponent<CameraShaker>().ShakeCameraDeath();


				}
			}
			else
			{
				//mScore -= 10;
				mPlayerAvatar.GetComponent<PlayerShipController>().StartSpin();
				Camera.main.GetComponent<CameraShaker>().ShakeCameraDeath();
				if(AchievementManager.instance != null)
				{
					AchievementManager.instance.IDontCare.IncreseValue();
				}
			}

			//If that wasn't the last life, go invulnerable, otherwise go back to the title screen
			if(mP1Lives <= 0)
			{

				Camera.main.GetComponent<CameraShaker> ().RumbleController(.6f, 3.15f);
				//Destroy(mPlayerAvatar.gameObject);
				mPlayerAvatar.gameObject.SetActive (false);
				//mPlayerAvatar.gameObject.SetActive(false);
				mPlayerSafeTime = 3f;

			}
			else
			{
				mPlayerSafeTime = 2f;
				//Application.LoadLevel(Application.loadedLevel);
			}

		}
	}//END of LoseALife()

	public void LosePlayerTwoLife()
	{
		if(mPlayerSafeTime<=0f)
		{
			
			
			
			//Lose a life if the player isn't shielded ~Adam
			if(!mPlayer2Avatar.GetComponent<PlayerShipController>().mShielded)
			{
				Camera.main.GetComponent<CameraShaker> ().RumbleController(.1f, .2f);

				if(mP2Lives == 1)
				{
					GameObject playerDeathParticles;
					playerDeathParticles = Instantiate(mPlayer2DeathEffect, mPlayer2Avatar.transform.position, Quaternion.identity) as GameObject;
				}
				if(mPlayer2Avatar.GetComponent<PlayerShipController>().mShipRecovered)
				{
					mPlayer2Avatar.GetComponent<PlayerShipController>().mShipRecovered = false;
					mPlayer2Avatar.GetComponent<PlayerShipController>().StartSpin();
					Camera.main.GetComponent<CameraShaker>().ShakeCameraDeath();


				}
				else
				{
					mScore -= 10;
					mP2Score -= 10;
					if(mP2Score < 0)
					{
						mP1Score += mP2Score;
						mP2Score = 0;
					}
					if(mScore <-1)
					{
						mScore = -1;
						mP1Score = 0;
						mP2Score = 0;
					}
					mLivesRemaining--;
					mP2Lives--;
					mPlayer2Avatar.GetComponent<PlayerShipController>().StartSpin();
					mPlayer2Avatar.GetComponent<PlayerShipController>().TakeStatDamage();
					Camera.main.GetComponent<CameraShaker>().ShakeCameraDeath();


				}
			}
			else
			{
				//mScore -= 10;
				mPlayer2Avatar.GetComponent<PlayerShipController>().StartSpin();
				Camera.main.GetComponent<CameraShaker>().ShakeCameraDeath();

			
			}
			
			//If that wasn't the last life, go invulnerable, otherwise go back to the title screen
			if(mP2Lives <= 0)
			{

				Camera.main.GetComponent<CameraShaker> ().RumbleController(3f, 2f);
				//Destroy(mPlayer2Avatar.gameObject);
				mPlayer2Avatar.gameObject.SetActive (false);
				//mPlayer2Avatar.gameObject.SetActive(false);
				mPlayerSafeTime = 3f;
				
			}
			else
			{
				mPlayerSafeTime = 2f;
				//Application.LoadLevel(Application.loadedLevel);
			}
			
		}
	}//END of LosePlayerTwoLife()

	public void HitAPlayer(GameObject playerHit)
	{
		if(playerHit == mPlayerAvatar)
		{
			LoseALife ();
		}
		else if(playerHit == mPlayer2Avatar)
		{
			LosePlayerTwoLife ();
		}
	}

	public void StartCoOpMode()
	{
		mInCoOpMode = true;
		mMaxLives = 50;
		mP1Lives = 50;
		mP2Lives = 50;
		mLivesRemaining = 100;
	}

}
