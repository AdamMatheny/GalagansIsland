using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Script for disabling the player's gun at the start of each level to let swarms form

public class GetReady : MonoBehaviour 
{
	[SerializeField] private float mReadyTimer = 5f;

	public PlayerOneShipController mPlayer1Ship;
	public PlayerTwoShipController mPlayer2Ship;
	[SerializeField] private Text mReadyText;
	[SerializeField] private string mStartText = "Get Ready!";
	[SerializeField] private string mFireText = "Fire Away!";

	// Use this for initialization
	void Start () 
	{
		//Set the text that displays at the start of the level ~Ada,
		mReadyText.text = mStartText;

		//Find the player ships ~Adam
		if(FindObjectOfType<PlayerOneShipController>() != null)
		{
			mPlayer1Ship = FindObjectOfType<PlayerOneShipController>();
		}
		if(FindObjectOfType<PlayerTwoShipController>() != null)
		{
			mPlayer2Ship = FindObjectOfType<PlayerTwoShipController>();
		}
	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		mReadyTimer -= Time.deltaTime;
		if(mReadyTimer > 1f)
		{
			//Count Down ~Adam

			//Turn off the player 1 ship's gun if the ship is present, else, find the ship ~Adam
			if(mPlayer1Ship!=null)
			{
				mPlayer1Ship.mToggleFireOn = false;
				mPlayer1Ship.isOverheated = true;
			}
			else
			{
				if(FindObjectOfType<PlayerOneShipController>() != null)
				{
					mPlayer1Ship = FindObjectOfType<PlayerOneShipController>();
				}
			}
			//Turn off the player 2 ship's gun if the ship is present, else, find the ship ~Adam
			if(mPlayer2Ship!=null)
			{
				mPlayer2Ship.mToggleFireOn = false;
				mPlayer2Ship.isOverheated = true;
			}
			else
			{
				if(FindObjectOfType<PlayerTwoShipController>() != null)
				{
					mPlayer2Ship = FindObjectOfType<PlayerTwoShipController>();
				}
			}
		}
		//Let the player fire and change the text message ~Adam
		else if(mReadyTimer > 0f)
		{
			mReadyText.text = mFireText;
		}
		//Delete self ~Adam
		else
		{
			Destroy(this.gameObject);
		}
	}//END of Update()
}
