using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class IndieGameTransition : MonoBehaviour 
{
	[SerializeField] private GameObject mPlayerShip;
	[SerializeField] private GameObject mScoreManager;
	[SerializeField] private GameObject mGameHUD;
	[SerializeField] private GameObject mAsteroidspawn;
	[SerializeField] private GameObject mIcicleStorm;

	public Image mStartScreenImage;
	public List<Sprite> mGameImages = new List<Sprite>();

	// Use this for initialization
	void Start () 
	{
		//Find and hide yet preserve the objects we need for going back to the main game ~Adam
		mPlayerShip = GameObject.Find("Ship");
		mScoreManager = GameObject.Find("ScoreManager");
		mGameHUD = GameObject.Find("Game_HUD");
		mAsteroidspawn = GameObject.Find("Asteroidspawn");
		mIcicleStorm = GameObject.Find("Icicle storm");

		mPlayerShip.SetActive (false);
		mScoreManager.SetActive (false);
		mGameHUD.SetActive (false);
		mAsteroidspawn.SetActive (false);
		mIcicleStorm.SetActive (false);

		mStartScreenImage.sprite = mGameImages[PlayerPrefs.GetInt ("IndieGameKey")-1];

		if(PlayerPrefs.GetInt("GoingToGame") == 0)
		{
			Debug.Log("Going to mini game");
			GetComponent<Animator>().Play("TransitionAnimation");
		}
		else
		{
			Debug.Log("Returning to main game");
			GetComponent<Animator>().Play("ReturnAnimation");
		}
	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		
	}//END of Update()

	public void GoToIndieGame()
	{
		//Make sure these objects persist ~Adam
		mPlayerShip.SetActive (true);
		mScoreManager.SetActive (true);
		mGameHUD.SetActive (true);
		mAsteroidspawn.SetActive (true);
		mIcicleStorm.SetActive (true);

		switch (PlayerPrefs.GetInt ("IndieGameKey"))
		{
		case 1:
			Application.LoadLevel ("TowerOfElementsScene");
			break;
		default:
			Application.LoadLevel ("TowerOfElementsScene");
			break;
		}
	}//END of GoToIndieGame()

	public void ReturnToMainGame()
	{
		//Make sure these objects persist ~Adam
		mPlayerShip.SetActive (true);
		mScoreManager.SetActive (true);
		mGameHUD.SetActive (true);
		mAsteroidspawn.SetActive (true);
		mIcicleStorm.SetActive (true);


		Application.LoadLevel(PlayerPrefs.GetInt ("MainLevelLeft"));
	}//END of ReturnToMainGame()
}
