using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LifeIconManager : MonoBehaviour 
{
	ScoreManager mScoreManager;
	[SerializeField] private int mIconNumber;


	// Use this for initialization
	void Start () 
	{
		mScoreManager = FindObjectOfType<ScoreManager>();

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(mScoreManager != null)
		{
			if(mScoreManager.mLivesRemaining >= mIconNumber)
			{
				GetComponent<Image>().enabled = true;
			}
			else
			{
				GetComponent<Image>().enabled = false;
			}
		}
		else
		{
			mScoreManager = FindObjectOfType<ScoreManager>();
		}
	}
}
