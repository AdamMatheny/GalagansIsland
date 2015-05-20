using UnityEngine;
using System.Collections;

public class LevelKillCounter : MonoBehaviour 
{
	[SerializeField] private bool mAllowLevelSkip = false;
	[SerializeField] private int mRequiredKills = 30;
	[SerializeField] private int mKillCount = 0;
	float mLevelCompleteTimer = 5f;

	bool mLevelComplete = false;

	[SerializeField] private Texture2D mLevelCompleteTex;

	//For spawning new waves after a certain number of enemies have been killed.
	//Make sure that these two arrays are the same length!
	[SerializeField] private EnemyShipSpawner[] mShipWaves;
	[SerializeField] private EnemyShipSpawner[] mInactiveWaves;

	[SerializeField] private int[] mShipWaveRequiredKills;

	[SerializeField] private GUIStyle mLevelCompleteStyle;

	[SerializeField] private bool mRemainingEnemy;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
//		Debug.Log(Time.time);
		if(mLevelComplete)
		{
			EnemyShipAI[] leftoverEnemies;
			leftoverEnemies = FindObjectsOfType<EnemyShipAI>();
			foreach(EnemyShipAI enemyToDelete in leftoverEnemies)
			{
				Destroy(enemyToDelete.gameObject);
			}
		}

		mRemainingEnemy = (FindObjectOfType<EnemyShipAI>() != null);

		if(mKillCount >= mRequiredKills && !mRemainingEnemy)
		{
			if(mRemainingEnemy == false)
			{
				mLevelComplete = true;
			}
		}
		if(mLevelComplete)
		{
			mLevelCompleteTimer -= Time.deltaTime;
		}

		if (mLevelCompleteTimer <= 0f)
		{
			Debug.Log("Loading next level");
			if(Application.loadedLevel == Application.levelCount-3)
			{
				Destroy(FindObjectOfType<PlayerShipController>().gameObject);
				//Destroy(FindObjectOfType<ScoreManager>().gameObject);

				Application.LoadLevel("Credits");
				Destroy(this.gameObject);
			}
			else
			{
				FindObjectOfType<PlayerShipController>().mShipStolen = false;
				Application.LoadLevel(Application.loadedLevel+1);
			}
		}

		if(mShipWaves.Length>0 && mShipWaves.Length == mShipWaveRequiredKills.Length && mShipWaves.Length == mInactiveWaves.Length)
		{
			for (int i = 0; i < mShipWaves.Length; i++)
			{
				if(mShipWaves[i] != null && mKillCount >= mShipWaveRequiredKills[i] && mShipWaves[i].enabled == false)
				{
					mShipWaves[i].enabled=true;
				}

				if(mInactiveWaves[i] != null && mKillCount >= mShipWaveRequiredKills[i] && mInactiveWaves[i].enabled == true)
				{
					mInactiveWaves[i].enabled=false;
				}

			}
		}
		if(Input.GetKeyDown("i") && mAllowLevelSkip)
		{
			mLevelComplete = true;
		}
	}//END of Update()

	void OnGUI()
	{
		mLevelCompleteStyle.fontSize = Mathf.RoundToInt(Screen.width*0.01f);

		if(mLevelComplete)
		{
			GUI.Box(new Rect(Screen.width*0.425f, Screen.height*0.4f, Screen.width*0.15f, Screen.height*0.05f), "", mLevelCompleteStyle);
			//GUI.DrawTexture(new Rect(Screen.width*0.4f, Screen.height*0.4f, Screen.width*0.2f, Screen.height*0.2f), mLevelCompleteTex);
		}
	}

	public void UpdateKillCounter()
	{
		mKillCount++;
	}
}
