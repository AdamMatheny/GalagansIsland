using UnityEngine;
using System.Collections;

public class PlayerStateController : MonoBehaviour 
{

	public GameObject panel;

	public enum playerStates
	{
		idle = 0,
		left,
		right,
		up,
		down,
		diagUL,
		diagUR,
		diagDL,
		diagDR,
		dash,
		falling,
		kill,
		resurrect,
		firingWeapon,
		_stateCount
	}
		
	public static float[] stateDelayTimer = new float[(int)playerStates._stateCount];
	
	public delegate void playerStateHandler(PlayerStateController.playerStates newState);
	public static event playerStateHandler onStateChange;
    public bool keyboard = false;
    private bool lastInput = false;
    private Animator playerAnimator = null;

	//Used for the TapCheck() function
	private int tapCount = 0;
	private float lastTap = 0;
	public float leeway = 0.4f;
	public int maxtaps = 2;

	void LateUpdate () 
	{
		//if(!GameStates.gameActive)
		//	return;
		
		if (Input.GetKeyDown (KeyCode.P)) 
		{
			
			if(Time.timeScale != 1)
			{
				
				unPause();
			}
			else 
			{
				Pause();
			}
		}
		
		// Detect the current input of the Horizontal axis, then broadcast a state update for the player as appropriate
		playerAnimator = GetComponent<Animator>();
		
		
		
		#region KEYBOARD CONTROLS
		if (keyboard)
		{

			float horizontal = Input.GetAxis("Horizontal");
			float vertical = Input.GetAxis("Vertical");

			//Taking in diretional Input
			if (horizontal != 0.0f || vertical != 0.0f)
			{
				//Moving Left and Right
				if (horizontal < 0.0f && vertical == 0.0f)
				{
					if (onStateChange != null)
					{
						onStateChange(PlayerStateController.playerStates.left);
					}
					
				}
				else if (horizontal > 0.0f && vertical == 0.0f)
				{
					if (onStateChange != null)
					{
						onStateChange(PlayerStateController.playerStates.right);
					}
				}
				//Moving Up and Down
				else if (vertical < 0.0f && horizontal == 0.0f)
				{
					if (onStateChange != null)
					{
						onStateChange(PlayerStateController.playerStates.down);
					}
					
				}
				else if (vertical > 0.0f && horizontal == 0.0f)
				{
					if (onStateChange != null)
					{
						onStateChange(PlayerStateController.playerStates.up);
					}
				}
				//Moving Diagonally Up
				else if (vertical > 0.0f && horizontal > 0.0f)
				{
					if (onStateChange != null)
					{
						onStateChange(PlayerStateController.playerStates.diagUR);
					}
				}
				else if (vertical > 0.0f && horizontal < 0.0f)
				{
					if (onStateChange != null)
					{
						onStateChange(PlayerStateController.playerStates.diagUL);
					}
				}
				//Moving Diagonally Down
				else if (vertical < 0.0f && horizontal > 0.0f)
				{
					if (onStateChange != null)
					{
						onStateChange(PlayerStateController.playerStates.diagDR);
					}
				}
				else if (vertical < 0.0f && horizontal < 0.0f)
				{
					if (onStateChange != null)
					{
						onStateChange(PlayerStateController.playerStates.diagDL);
					}
				}

			}


			//*****We don't have a "DASH", whis is this in here?*****
			else
			{
				//Prepare for Dash movement
				lastInput = false;
				//Do idle movement
				if (onStateChange != null)
					onStateChange(PlayerStateController.playerStates.idle);
			}
			
			float firing = Input.GetAxis("Fire1");
			if (firing > 0.0f)
			{
				if (onStateChange != null)
				{
					//if (playerAnimator.GetBool("DashF"))
					//{
					//    playerAnimator.SetBool("DashF", false);
					//}
					onStateChange(PlayerStateController.playerStates.firingWeapon);
					Debug.Log("FIRE");
				}
			}
		}//End of keyboard controls
		#endregion
	}//END of LateUpdate()

	void Pause()
	{

		Time.timeScale = 0;
		panel.SetActive (true);
	}
	void unPause()
	{

		Time.timeScale = 1;
		panel.SetActive (false);
	}



    int TapCheck(string axis)
    {
        if(Input.GetAxis(axis) != 0)
        {
            if(!lastInput)
            {
                lastTap = Time.time;
                lastInput = true;
                tapCount++;
                if (tapCount == 1)
                {
                    //Debug.Log("Tap" + axis);
                    return 1;
                }
                if (tapCount == 2)
                {
                    //Debug.Log("DoubleTap" + axis);
                    return 2;
                }
                if (tapCount >= maxtaps)
                    tapCount = 0;
            }
            else
            {
                if(Time.time - lastTap > leeway)
                {
                    tapCount = 0;
                }
            }
        }
        return 0;
    }//END of TapChec()
}
