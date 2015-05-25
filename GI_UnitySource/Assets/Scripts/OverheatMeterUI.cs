using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OverheatMeterUI : MonoBehaviour 
{
	[SerializeField] private Image mOverHeatBar;
	[SerializeField] private Image mOverHeatOverlay;

	PlayerShipController mPlayer;

	// Use this for initialization
	void Start () 
	{

		//Find the player ship -Adam
		mPlayer = FindObjectOfType<PlayerShipController>();
	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		//Safety in case the player ship connection is lost -Adam
		if(mPlayer == null)
		{
			mPlayer = FindObjectOfType<PlayerShipController>();
		}

		else
		{
			//Make the bar move up and down
			mOverHeatBar.GetComponent<RectTransform>().localScale = new Vector3(1f, mPlayer.heatLevel/mPlayer.maxHeatLevel, 1f);

			//Display overlay when overheated
			if(mPlayer.isOverheated)
			{
				mOverHeatOverlay.enabled = true;
				GetComponent<Animator>().speed = 0f;
			}
			else
			{

				mOverHeatOverlay.enabled = false;
				if(mPlayer.heatLevel/mPlayer.maxHeatLevel > 0.9f)
				{
					GetComponent<Animator>().speed = 5f;
				}
				else
				{
					GetComponent<Animator>().speed = 1f;
				}
			}
		}
	}//END of Update()
}
