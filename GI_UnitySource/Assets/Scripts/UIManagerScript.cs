using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour 
{

    public Animator startButton;
    public Animator quitButton;
    public Animator dialog;
    public Image Lives1Img;
    public Image Lives2Img;
    public Image Lives3Img;
   
	float mGameStartTimer = 2.5f;
	bool mStartingGame = false;

	void Update()
	{
		if (mStartingGame)
		{
			mGameStartTimer-=Time.deltaTime;

			if(mGameStartTimer < Time.time)
			{
				Time.timeScale = 1f;
				Application.LoadLevel(1);
			}
		}
	}

	public void StartGame()
    {
		if(!startButton.GetBool("isHidden"))
	   {
			startButton.SetBool("isHidden", true);
			quitButton.SetBool("isHidden", true);
			dialog.SetBool("isHidden", true);
		
			Time.timeScale = 0.25f;
			EnemyShipAI[] startScreenShips = FindObjectsOfType<EnemyShipAI>();

			foreach(EnemyShipAI startShip in startScreenShips)
			{
				startShip.EnemyShipDie();
			}

			Destroy(FindObjectOfType<PlayerShipController>().gameObject);
			Destroy(FindObjectOfType<LevelKillCounter>().gameObject);
			Destroy(FindObjectOfType<ScoreManager>().gameObject);
			mGameStartTimer+= Time.time;
			mStartingGame = true;
		}
    }

    public void OpenSettings()
    {
		if(!dialog.GetBool("isHidden"))
		{
	        startButton.SetBool("isHidden", true);
	        quitButton.SetBool("isHidden", true);
	        dialog.enabled = true;
	        dialog.SetBool("isHidden", false);
    	}
	}

    public void CloseSettings()
    {
        startButton.SetBool("isHidden", false);
        quitButton.SetBool("isHidden", false);
        dialog.SetBool("isHidden", true);
    }

    public void ShowExtraLives(int numLives)
    {
        switch (numLives)
        {
            case 0:
                Lives1Img.enabled = false;
                Lives2Img.enabled = false;
                Lives3Img.enabled = false;
                break;
            case 1:
                Lives1Img.enabled = true;
                Lives2Img.enabled = false;
                Lives3Img.enabled = false;
                break;
            case 2:
                Lives1Img.enabled = true;
                Lives2Img.enabled = true;
                Lives3Img.enabled = false;
                break;
            case 3:
                Lives1Img.enabled = true;
                Lives2Img.enabled = true;
                Lives3Img.enabled = true;
                break;
        }
    }

}
