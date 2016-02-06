﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InControl;
using XInputDotNetPure; // Required in C#


public class PlayerOneShipController : PlayerShipController 
{
	
	
	float mFireCheckpointTimer = 0f;
	float mMoveCheckpointTimer = 0f;
	public int mCheckPointsRemaining = 3;

	// Use this for initialization
	protected override void Start () 
	{
		//Make sure we always have a reference to the score manager and set the current life percentage ~Adam
		if(FindObjectOfType<ScoreManager>() != null)
		{
			mScoreMan = FindObjectOfType<ScoreManager>();
			mPauseMan = mScoreMan.gameObject.GetComponent<PauseManager>();

		}
		
		//Adjust speed and scale for mobile ~Adam
		if (Application.isMobilePlatform)
		{
			mBaseMovementSpeed = 15.0f;
			transform.localScale = new Vector3(1.5f,1.5f,1.5f);
		}
		
		mShipCreationLevel = Application.loadedLevel;
		
		PlayerOneShipController[] otherPlayerShips = FindObjectsOfType<PlayerOneShipController>();
		//Debug.Log(otherPlayerShip.name);
		foreach(PlayerOneShipController othership in otherPlayerShips)
		{
			if(othership.mShipCreationLevel < this.mShipCreationLevel)
			{
				Debug.Log("Found another ship so destroying self.");
				Destroy(this.gameObject);
			}
		}
		if(mScoreMan != null && mScoreMan.mPlayerAvatar != null && mScoreMan.mPlayerAvatar != this.gameObject)
		{
			Destroy(this.gameObject);
		}
		
		//mLastFramePosition = transform.position;
		
	}//END of Start()
	
	
	//Persist between level loads/reloads ~Adam
	protected override void Awake()
	{
		base.Awake();
		
	}//END of Awake()
	
	
	
	// Update is called once per frame
	protected override void Update () 
	{
		
		base.Update();
		
		TakeCheckpointInput();
	}//END of Update()
	
	void LateUpdate () 
	{
		base.LateUpdate ();
	}//END of LateUpdate()
	
	//For flipping ships upside down in co-op mode ~Adam
	public override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter (collision);
	}


	public void StartSpin()
	{
		base.StartSpin ();
	}//END of StartSpin()
	
	
	
	public void SpinShip(float spinDir)
	{
		base.SpinShip(spinDir);
	}//END of SpinShip()
	
	//For getting hit by boss beams ~Adam
	void OnParticleCollision(GameObject other)
	{
		base.OnParticleCollision (other);
	}//END of OnParticleCollision()
	
	//For taking weapon/movement damage ~Adam
	public void TakeStatDamage()
	{
		base.TakeStatDamage ();
	}//END of TakeStatDamage()
	
	
	
	#region breaking down parts of the Update() function for parts that will be different between Player1 and Player2
	//managing input devices on co-op mode ~Adam
	protected override void ManageInputDevice()
	{
		base.ManageInputDevice();
	}//END of ManageInputDevice()
	
	//For Spinning the ship ~Adam
	protected override void SpinControl()
	{
		base.SpinControl();
	}//END of SpinControl()
	
	//Make the player drift toward the bottom of the screen ~Adam
	protected override void DriftDown()
	{
		base.DriftDown ();
		
	}//END of DriftDown()
	
	
	protected override void TakeDirectionalInput()
	{
		base.TakeDirectionalInput ();
	}//END of TakeDirectionalInput()
	
	protected override void SetMovementDirection(float horizontal, float vertical)
	{
		base.SetMovementDirection(horizontal, vertical);
	}//END of SetMovementDirection()
	
	protected override void TakeFiringInput()
	{
		base.TakeFiringInput ();
	}//END of TakeFiringInput()
	
	//Thruster control for hovering ~Adam
	protected override void TakeThrusterInput()
	{
		base.TakeThrusterInput ();
	}//END of TakeThrusterInput()

	void TakeCheckpointInput()
	{
		if(Time.timeScale > 0f 
		   && Application.loadedLevel != PlayerPrefs.GetInt ("CheckPointedLevel") 
		   && mCheckPointsRemaining > 0 && Application.loadedLevelName != "Credits")
		{
			//Hold the Right Bumper to spend Movement Speed to place a checkpoint ~Adam 
			if((mPlayerInputDevice.RightBumper.IsPressed || Input.GetKey(KeyCode.E)) && mMoveUpgrade > 0.6f)
			{
				Debug.Log ("Holding Rgiht Bumper");
				mMoveCheckpointTimer += Time.deltaTime;
				if(mMoveCheckpointTimer >= 3f)
				{
					mMoveUpgrade-= 0.3f;
					if(mMoveUpgrade < 0.6f)
					{
						mMoveUpgrade = 0.6f;
					}
					SaveCheckPointStats();
				}
			}
			//Hold the Right Bumper to spend Rate of Fire to place a checkpoint ~Adam 
			else if( (mPlayerInputDevice.LeftBumper.IsPressed || Input.GetKey(KeyCode.Q)) && mFireUpgrade > 0.6f)
			{
				Debug.Log ("Holding Left Bumper");
				mFireCheckpointTimer += Time.deltaTime;
				if(mFireCheckpointTimer >= 3f)
				{
					mFireUpgrade-= 0.3f;
					if(mFireUpgrade < 0.6f)
					{
						mFireUpgrade = 0.6f;
					}
					SaveCheckPointStats();
				}
			}
			//Reset timers when bumpers are released ~Adam
			if(mPlayerInputDevice.RightBumper.WasReleased || Input.GetKeyUp(KeyCode.E))
			{
				mMoveCheckpointTimer = 0f;
			}
			if(mPlayerInputDevice.LeftBumper.WasReleased || Input.GetKeyUp(KeyCode.Q))
			{
				mFireCheckpointTimer = 0f;
			}
		}
	}//END of TakeCheckpointInput()

	//Set all the stats for a Checkpoint being placed ~Adam
	void SaveCheckPointStats()
	{
		mCheckPointsRemaining -= 1;
		Debug.Log ("Saving Checkpoint stats at time, " + Time.time);
		if(Application.loadedLevelName != "Tutorial" && Application.loadedLevelName != "Credits")
		{
			PlayerPrefs.SetInt ("CheckPointedLevel", Application.loadedLevel);


			PlayerPrefs.SetInt("CheckPointedLivesRemaining", mScoreMan.mLivesRemaining);
			PlayerPrefs.SetInt("CheckPointedP1Lives", mScoreMan.mP1Lives);

			PlayerPrefs.SetFloat("CheckPointedP1FireUpgrade", mFireUpgrade);
			PlayerPrefs.SetFloat("CheckPointedP1MoveUpgrade", mMoveUpgrade);
			PlayerPrefs.SetFloat("CheckPointedP1ShieldTime", mShieldTimer);
			PlayerPrefs.SetFloat("CheckPointedP1SpreadTime", mThreeBulletTimer);
			PlayerPrefs.SetInt ("CheckPointedCheckPointsRemaining", mCheckPointsRemaining);
		}
		//Don't actually set checkpoints on the Tutorial or Credits ~Adam
		else
		{
			PlayerPrefs.SetInt ("CheckPointedLevel", 0);
		}
	}//END of SetCheckPointStats()

	#endregion
	
}//END of MonoBehavior