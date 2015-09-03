using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InControl;
using XInputDotNetPure; // Required in C#


public class PlayerOneShipController : PlayerShipController 
{
	
	

	
	// Use this for initialization
	protected override void Start () 
	{
		//Make sure we always have a reference to the score manager and set the current life percentage ~Adam
		if(FindObjectOfType<ScoreManager>() != null)
		{
			mScoreMan = FindObjectOfType<ScoreManager>();
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
		
		mLastFramePosition = transform.position;
		
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
		
		
	}//END of Update()
	
	void LateUpdate () 
	{
		base.LateUpdate ();
	}//END of LateUpdate()
	
	
	
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
	
	#endregion
	
}//END of MonoBehavior