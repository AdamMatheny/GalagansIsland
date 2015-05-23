using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour 
{
	[SerializeField] private Texture2D mSideDisplayTex;

	public int mShieldLifeCount = 10;
	public int mShieldHits = 0;

	public int mScore = 0;
	public int mLivesRemaining = 100;
	public int mCurrentLevel; //Changed this variable name to be consistent with the rest of the naming schem ~Adam
	public int mOriginalLevel = 0;
	//For giving the player an extra life every certain number of points ~Adam
	int mExtraLifeScore = 1000;
	int mExtraLifeInteraval = 1000;

	//For spawning an triple-bullet power-up every certain number of points ~Adam
	int mPowerUpScore = 500;
	int mPowerUpInterval = 500;
	[SerializeField] private GameObject mTripleBulletEmblem;
	//For spawning a shield power-up every certain number of points ~Adam
	int mShieldScore = 300;
	int mShieldInterval = 300;
	[SerializeField] private GameObject mShieldEmblem;

	//For the UI of showing a meter depicting tim until next powerup
	[SerializeField] private Image mPowerUpMeter;
	[SerializeField] private GameObject mPowerUpMeterBack;

	public float mPlayerSafeTime = 0f;
	// Use this for initialization
	[SerializeField] private GameObject mPlayerAvatar;
	public GameObject mPlayerDeathEffect;


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


	void StoreHighscore(int newHighscore)
	{
		int oldHighscore = PlayerPrefs.GetInt("highscore", 0);    
		if(newHighscore > oldHighscore)
			PlayerPrefs.SetInt("highscore", newHighscore);
	}



	void Start () 
	{

//		//Get rid of self if we're back on the title screen
//		if (Application.loadedLevel == 0)
//		{
//			Destroy(this.gameObject);
//		}

		//Delete self if there's already a score manager to prevent duplicates (this only seems to delete the new ones, which is what we want)

	}

	//Pesist between level loads/reloads ~adam
	void Awake()
	{
		DontDestroyOnLoad (transform.gameObject);
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

	}

	// Update is called once per frame
	void Update () 
	{

		mCurrentLevel = Application.loadedLevel; //Wasn't affected in either Awake() or Start()

		//We already had a method of switching between levels that gave us a lag time in which to actually play a player death animation ~Adam
//		if (mLivesRemaining <= 0) {
//
//			Application.LoadLevel("EndGame");
//			Destroy(shipDeath);
//		}

		//If we're out of lives, wait a short bit for the player explosion to play, then clean up the objects that normally persist between levels
		//Then go to the EndGame scene and delete this game object ~Adam
		if(mLivesRemaining <= 0 && mPlayerSafeTime <= 0)
		{

			//Destroy(FindObjectOfType<PlayerShipController>().gameObject); //Looks like for some reason I had this line in there that would have prevented the EndGame scene from loading... Oops...~Adam
			Destroy(FindObjectOfType<LevelKillCounter>().gameObject);
			Application.LoadLevel("EndGame");
			mLevelInfoText.text = "Lives: " + mLivesRemaining + "\nScore: " + mScore + "\nGame Over";

			this.enabled = false;
			//Destroy(this.gameObject);
			
		}

		mPlayerSafeTime-=Time.deltaTime;

		if(mScore < 0)
		{
			mScore = 0;
		}

//		//Grant an extra life every 1000 kills (assuming 1 point per kill) ~Adam
//		if(mScore >= mExtraLifeScore)
//		{
//			mLivesRemaining++;
//			mExtraLifeScore += mExtraLifeInteraval;
//		}

		if(mPowerUpScore < mShieldScore)
		{
			float barAdjust = 710.9f * (mPowerUpInterval-(mPowerUpScore-mScore))/mPowerUpInterval;
			//Debug.Log (mPowerUpMeter.rectTransform.sizeDelta);
			mPowerUpMeter.rectTransform.sizeDelta = new Vector2( barAdjust, mPowerUpMeter.rectTransform.sizeDelta.y); 
			//mPowerUpMeter.rectTransform.rect = new Rect(mPowerUpMeter.rectTransform.rect.x, mPowerUpMeter.rectTransform.rect.y, barAdjust, mPowerUpMeter.rectTransform.rect.height);
		}
		else
		{
			float barAdjust = 710.9f * (mShieldInterval-(mShieldScore-mScore))/mShieldInterval;
		//	Debug.Log (mPowerUpMeter.rectTransform.sizeDelta);
			mPowerUpMeter.rectTransform.sizeDelta = new Vector2( barAdjust, mPowerUpMeter.rectTransform.sizeDelta.y); 
		}
		//Spawn a triple bullet power up every 500 kills (assuming 1 point per kill) ~Adam
		if(mScore >= mPowerUpScore)
		{
			float spawnXPos = Random.Range(-18f,18f);
			float spawnyPos = Random.Range(-17f,23f);
			Instantiate(mTripleBulletEmblem, new Vector3(spawnXPos, spawnyPos, -2f), Quaternion.identity);
			mPowerUpMeterBack.GetComponent<Animator>().Play("PowerPointMeterFlash_Anim");
			mPowerUpScore += mPowerUpInterval;
		}
		//Spawn a shield power up every 300 kills (assuming 1 point per kill) ~Adam
		if(mScore >= mShieldScore)
		{
			float spawnXPos = Random.Range(-18f,18f);
			float spawnyPos = Random.Range(-17f,23f);
			Instantiate(mShieldEmblem, new Vector3(spawnXPos, spawnyPos, -2f), Quaternion.identity);
			mPowerUpMeterBack.GetComponent<Animator>().Play("PowerPointMeterFlash_Anim");
			mShieldScore += mShieldInterval;
		}

		//Make sure we have a reference to the player's ship ~Adam
		if (mPlayerAvatar == null)
		{
			mPlayerAvatar = GameObject.FindGameObjectWithTag("Player").gameObject;
			
		}

		//Color the player while invincible
		if(mPlayerAvatar != null)
		{
			if(mPlayerSafeTime > 0)
			{
				mPlayerAvatar.GetComponent<PlayerShipController>().mMainShipHitSprite.SetActive(true);
				if(mPlayerAvatar.GetComponent<PlayerShipController>().mShipRecovered)
				{
					mPlayerAvatar.GetComponent<PlayerShipController>().mSecondShipHitSprite.SetActive(true);
				}
			}
			else
			{
				mPlayerAvatar.GetComponent<PlayerShipController>().mMainShipHitSprite.SetActive(false);
				mPlayerAvatar.GetComponent<PlayerShipController>().mSecondShipHitSprite.SetActive(false);
			}
		}


		mLevelInfoText.text = "Lives: " + mLivesRemaining + "\nScore: " + mScore + "\n" + mLevelNames[Application.loadedLevel];
		mHighScoreText.text = "High Score: " + PlayerPrefs.GetInt("highscore", 0);

		StoreHighscore (mScore);

	}//END of Update()

//	void OnGUI()
//	{
//		mScoreManStyle.fontSize = Mathf.RoundToInt(Screen.width*0.0095f);
//		mHighScoreStyle.fontSize = Mathf.RoundToInt(Screen.width*0.008f);
//		GUI.DrawTexture(new Rect(0f, Screen.height*0.01f, Screen.width*0.21f, Screen.height*0.1f), mSideDisplayTex);
//		GUI.Label(new Rect(Screen.width * 0.042f, Screen.height*0.03f, Screen.width*0.1f, Screen.height*0.1f), "Lives: " + mLivesRemaining + "\nScore: " + mScore + "\n" + mLevelNames[Application.loadedLevel], mScoreManStyle);
//		GUI.Box(new Rect(Screen.width * 0.0625f, Screen.height*0.11f, Screen.width*0.1f, Screen.height*0.05f), "High Score: " + PlayerPrefs.GetInt("highscore", 0), mHighScoreStyle);
//
//
//	}

	//Used for adding/subtracting points
	public void AdjustScore(int points)
	{
		mScore += points;
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

				GameObject playerDeathParticles;
				playerDeathParticles = Instantiate(mPlayerDeathEffect, mPlayerAvatar.transform.position, Quaternion.identity) as GameObject;

				if(mPlayerAvatar.GetComponent<PlayerShipController>().mShipRecovered)
				{
					mPlayerAvatar.GetComponent<PlayerShipController>().mShipRecovered = false;
					mPlayerAvatar.GetComponent<PlayerShipController>().StartSpin();
					Camera.main.GetComponent<CameraShaker>().ShakeCameraDeath();
				}
				else
				{
					mScore -= 10;
					mLivesRemaining--;
					mPlayerAvatar.GetComponent<PlayerShipController>().StartSpin();
					Camera.main.GetComponent<CameraShaker>().ShakeCameraDeath();
				}
			}
			else
			{
				//mScore -= 10;
				mPlayerAvatar.GetComponent<PlayerShipController>().StartSpin();
				Camera.main.GetComponent<CameraShaker>().ShakeCameraDeath();
			}

			//If that wasn't the last life, go invulnerable, otherwise go back to the title screen
			if(mLivesRemaining <= 0)
			{
				Destroy(mPlayerAvatar.gameObject);
				mPlayerSafeTime = 3f;

			}
			else
			{
				mPlayerSafeTime = 2f;
				//Application.LoadLevel(Application.loadedLevel);
			}

		}
	}//END of LoseALife()


}
