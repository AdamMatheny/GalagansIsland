using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PlayerStateListener : MonoBehaviour
{         
	public float playerWalkSpeed = 3f;
	public float playerJumpForceVertical = 500f;
	public float playerJumpForceHorizontal = 250f;
	public GameObject playerRespawnPoint = null;
	public GameObject bulletPrefab = null;
	private Animator playerAnimator = null;
    public Transform bulletSpawnTransform;
	private PlayerStateController.playerStates previousState = PlayerStateController.playerStates.idle;
	private PlayerStateController.playerStates currentState = PlayerStateController.playerStates.idle;
    private bool playerHasLanded = true;

	//Used for the duplicating of the ship via Grabber Enemies
	public bool mShipStolen = false;
	public bool mShipRecovered = false;

	public GameObject mMainShip;
	public GameObject mSecondShip;

    public void SetWalkSpeed(float value)
    {
        playerWalkSpeed = value;
    }

	void OnEnable()
    {
		PlayerStateController.onStateChange += onStateChange;
    }
	
    void OnDisable()
    {
		PlayerStateController.onStateChange -= onStateChange;
    }
	
	void Start()
	{
		PlayerStateListener otherPlayerShip = FindObjectOfType<PlayerStateListener>();
		if (otherPlayerShip != null && otherPlayerShip != this)
		{
			Destroy(this.gameObject);
		}


		playerAnimator = GetComponent<Animator>();
		
		// Setup any specific starting values here
		//PlayerStateController.stateDelayTimer[ (int)PlayerStateController.playerStates.jump] = 1.0f;
		PlayerStateController.stateDelayTimer[ (int)PlayerStateController.playerStates.firingWeapon] = 1.0f;
	}
    
	//Pesist between level loads/reloads
	void Awake()
	{
		DontDestroyOnLoad (transform.gameObject);
	}



	void Update()
	{
		playerWalkSpeed = 6f + (6f/25f*Application.loadedLevel);
		if(Application.loadedLevel == 0)
		{
			Destroy(this.gameObject);
		}
		if (mShipRecovered)
		{
			mSecondShip.GetComponent<Renderer>().enabled = true;
		}
		else
		{
			mSecondShip.GetComponent<Renderer>().enabled = false;
		}
	}

    void LateUpdate()
    {
		onStateCycle();
		//Keep ship within screen bounds
		if (transform.position.x < -18 && mShipRecovered)
		{
			transform.position = new Vector3(-18f, transform.position.y, transform.position.z);
		}
		else if(transform.position.x < -20f)
		{
			transform.position = new Vector3(-20f, transform.position.y, transform.position.z);
		}
		if (transform.position.x > 20f)
		{
			transform.position = new Vector3(20f, transform.position.y, transform.position.z);
		}
		if(transform.position.y < -33f)
		{
			transform.position = new Vector3(transform.position.x, -33f, transform.position.z);
		}
		if (transform.position.y > 23f)
		{
			transform.position = new Vector3(transform.position.x, 23, transform.position.z);
		}

    }
    
	public void hitDeathTrigger()
	{
		onStateChange(PlayerStateController.playerStates.kill);
	}
	
    // Every cycle of the engine, process the current state.
    void onStateCycle()
    {
		// Grab the current localScale of the object so we have 
		// access to it in the following code
		//Vector3 localScale = transform.localScale;

		//transform.localEulerAngles = Vector3.zero;
		
		switch(currentState)
		{
			case PlayerStateController.playerStates.idle:
			break;
        
			case PlayerStateController.playerStates.left:
				transform.Translate(new Vector3((playerWalkSpeed * -1.0f) * Time.deltaTime, 0.0f, 0.0f));
			break;
             
			case PlayerStateController.playerStates.right:
				transform.Translate(new Vector3(playerWalkSpeed * Time.deltaTime, 0.0f, 0.0f));
			break;
             
			case PlayerStateController.playerStates.up:
				transform.Translate(new Vector3(0.0f, playerWalkSpeed * Time.deltaTime, 0.0f));
			break;
             
			case PlayerStateController.playerStates.down:
				transform.Translate(new Vector3(0.0f, (playerWalkSpeed * -1.0f) * Time.deltaTime, 0.0f));
			break;

		case PlayerStateController.playerStates.diagDL:
			transform.Translate(new Vector3(Mathf.Sqrt(2f*(playerWalkSpeed * playerWalkSpeed))* -1f* Time.deltaTime, Mathf.Sqrt(2f*(playerWalkSpeed * playerWalkSpeed)) * -1f* Time.deltaTime, 0.0f));
			break;

		case PlayerStateController.playerStates.diagDR:
			transform.Translate(new Vector3(Mathf.Sqrt(2f*(playerWalkSpeed * playerWalkSpeed))* Time.deltaTime, Mathf.Sqrt(2f*(playerWalkSpeed * playerWalkSpeed)) * -1f* Time.deltaTime, 0.0f));
			break;

		case PlayerStateController.playerStates.diagUL:
			transform.Translate(new Vector3(Mathf.Sqrt(2f*(playerWalkSpeed * playerWalkSpeed))* -1f* Time.deltaTime, Mathf.Sqrt(2f*(playerWalkSpeed * playerWalkSpeed)) * Time.deltaTime, 0.0f));
			break;

		case PlayerStateController.playerStates.diagUR:
			//Mathf.Sqrt(2f*(playerWalkSpeed * playerWalkSpeed))
			transform.Translate(new Vector3(Mathf.Sqrt(2f*(playerWalkSpeed * playerWalkSpeed))* Time.deltaTime, Mathf.Sqrt(2f*(playerWalkSpeed * playerWalkSpeed)) * Time.deltaTime, 0.0f));
			break;


			case PlayerStateController.playerStates.falling:
			break;              

			case PlayerStateController.playerStates.kill:
				onStateChange(PlayerStateController.playerStates.resurrect);
			break;         

			case PlayerStateController.playerStates.resurrect:
				onStateChange(PlayerStateController.playerStates.idle);
			break;
			
			case PlayerStateController.playerStates.firingWeapon:
			break;
		}
	}

    // onStateChange is called whenever we make a change to the player's state 
	// from anywhere within the game's code.
	public void onStateChange(PlayerStateController.playerStates newState)
	{
		// If the current state and the new state are the same, abort - no need 
		// to change to the state we're already in.
		if(newState == currentState)
			return;
		
		// Verify there are no special conditions that would cause this state to abort
		if(checkIfAbortOnStateCondition(newState))
			return;

         
		// Check if the current state is allowed to transition into this state. If it's not, abort.
		if(!checkForValidStatePair(newState))
			return;
         
		// Having reached here, we now know that this state change is allowed. 
		// So let's perform the necessary actions depending on what the new state is.
		switch(newState)
		{
			case PlayerStateController.playerStates.idle:
			break;
         
			case PlayerStateController.playerStates.left:
			break;
			case PlayerStateController.playerStates.right:
			break;

			case PlayerStateController.playerStates.up:
			break;
			case PlayerStateController.playerStates.down:
			break;

			case PlayerStateController.playerStates.diagDL:
			break;
			case PlayerStateController.playerStates.diagDR:
			break;

			case PlayerStateController.playerStates.diagUL:
			break;
			case PlayerStateController.playerStates.diagUR:
			break;


            case PlayerStateController.playerStates.dash:
                //playerAnimator.SetBool("DashF", true);
                playerWalkSpeed = 3.0f;
            break;
              
		case PlayerStateController.playerStates.firingWeapon:
			//Tell animator we're shooting
			//playerAnimator.SetBool("Shoot", true);
			
			// Make the bullet object
			GameObject newBullet = Instantiate(bulletPrefab, transform.position+new Vector3(0,1.82f,0), Quaternion.identity) as GameObject;
			
			if (mShipRecovered)
			{
				GameObject secondBullet;
				secondBullet = Instantiate(bulletPrefab, mSecondShip.transform.position+new Vector3(0,1.82f,0), Quaternion.identity) as GameObject;
				secondBullet.name = "SECONDBULLET";
			}
			
			
			
			//set the state of the player back to the current state
			onStateChange(currentState);
			
			//set a delay timer on the weapon firing again
			PlayerStateController.stateDelayTimer[(int)PlayerStateController.playerStates.firingWeapon] = Time.time + 0.25f-(0.2f/25f*Application.loadedLevel);
			break;

			//WE DON'T USE ANY OF THESE PLAYER STATES!!!!!
//			case PlayerStateController.playerStates.jump:                   
//				if(playerHasLanded)
//				{
//                    //playerAnimator.SetBool("JumpF", true);
//					// Use the jumpDirection variable to specify if the player should be jumping left, right or vertical
//					float jumpDirection = 0.0f;
//					if(currentState == PlayerStateController.playerStates.left)
//						jumpDirection = -1.0f;
//					else if(currentState == PlayerStateController.playerStates.right)
//						jumpDirection = 1.0f;
//					else
//						jumpDirection = 0.0f;
//					             
//					// Apply the actual jump force
//					rigidbody2D.AddForce(new Vector2(jumpDirection * playerJumpForceHorizontal, playerJumpForceVertical));
//									
//					playerHasLanded = false;
//    				PlayerStateController.stateDelayTimer[ (int)PlayerStateController.playerStates.jump] = 0f;
//				}
//			break;
//
//              
//			case PlayerStateController.playerStates.landing:
//                //playerAnimator.SetBool("JumpF", false);
//               // playerAnimator.SetBool("DashF", false);
//               // playerAnimator.SetBool("Falling", false);
//                playerWalkSpeed = 1.0f;
//				playerHasLanded = true;
//				PlayerStateController.stateDelayTimer[(int)PlayerStateController.playerStates.jump]= Time.time + 0.1f;
//			break;
//              
//			case PlayerStateController.playerStates.falling:
//                //playerAnimator.SetBool("Falling", true);
//				PlayerStateController.stateDelayTimer[ (int)PlayerStateController.playerStates.jump] = 0.0f;
//			break;              
//              
//			case PlayerStateController.playerStates.kill:
//			break;         
//
//			case PlayerStateController.playerStates.resurrect:
//				transform.position = playerRespawnPoint.transform.position;
//				transform.rotation = Quaternion.identity;
//				rigidbody2D.velocity = Vector2.zero;
//			break;
			

		}
         
		// Store the current state as the previous state
		previousState = currentState;
		
		// And finally, assign the new state to the player object
		currentState = newState;
	}    
    
	// Compare the desired new state against the current, and see if we are 
	// allowed to change to the new state. This is a powerful system that ensures 
	// we only allow the actions to occur that we want to occur.
	bool checkForValidStatePair(PlayerStateController.playerStates newState)
	{
		bool returnVal = false;

		// Compare the current against the new desired state.
		switch(currentState)
		{
			case PlayerStateController.playerStates.idle:
				// Any state can take over from idle.
				returnVal = true;
			break;
         
			case PlayerStateController.playerStates.left:
				// Any state can take over from the player moving left.
				returnVal = true;
			break;
              
			case PlayerStateController.playerStates.right:         
				// Any state can take over from the player moving right.
				returnVal = true;              
			break;

            case PlayerStateController.playerStates.dash:
                returnVal = true;
            break;
              
			case PlayerStateController.playerStates.up:
					returnVal = true;
			break;
              
			case PlayerStateController.playerStates.down:
					returnVal = true;
			break;             

			case PlayerStateController.playerStates.diagDL:
				returnVal = true;
			break;             
			case PlayerStateController.playerStates.diagDR:
				returnVal = true;
			break;             
			case PlayerStateController.playerStates.diagUL:
				returnVal = true;
			break;             
			case PlayerStateController.playerStates.diagUR:
				returnVal = true;
			break;             


			case PlayerStateController.playerStates.firingWeapon:
				returnVal = true;
			break;

			//*****WHY DO WE HAVE SO MANY STATES WE DIDN'T USE? DID WE GET THIS SCRIPT FROM A TUTORIAL?*****
//			case PlayerStateController.playerStates.falling:    
//				// The only states that can take over from falling are landing or kill
//				if(
//					newState == PlayerStateController.playerStates.landing
//					|| newState == PlayerStateController.playerStates.kill
//					|| newState == PlayerStateController.playerStates.firingWeapon
//				  )
//					returnVal = true;
//				else
//					returnVal = false;
//				break;              
//              
//			case PlayerStateController.playerStates.kill:         
//				// The only state that can take over from kill is resurrect
//				if(newState == PlayerStateController.playerStates.resurrect)
//					returnVal = true;
//				else
//					returnVal = false;
//			break;              
//              
//			case PlayerStateController.playerStates. resurrect :
//				// The only state that can take over from Resurrect is Idle
//				if(newState == PlayerStateController.playerStates.idle)
//					returnVal = true;
//				else
//					returnVal = false;                          
//			break;
			
		}          
		return returnVal;
	}
	
	// checkIfAbortOnStateCondition allows us to do additional state verification, to see
	// if there is any reason this state should not be allowed to begin.
	bool checkIfAbortOnStateCondition(PlayerStateController.playerStates newState)
	{
		bool returnVal = false;
		
		switch(newState)
		{
			case PlayerStateController.playerStates.idle:
			break;
			
			case PlayerStateController.playerStates.left:
			break;
			case PlayerStateController.playerStates.right:
			break;

			case PlayerStateController.playerStates.up:
			break;
			case PlayerStateController.playerStates.down:
			break;

			case PlayerStateController.playerStates.diagDL:
			break;
			case PlayerStateController.playerStates.diagDR:
			break;
			case PlayerStateController.playerStates.diagUL:
			break;
			case PlayerStateController.playerStates.diagUR:
			break;


			case PlayerStateController.playerStates.firingWeapon:		
				if(PlayerStateController.stateDelayTimer[ (int)PlayerStateController.playerStates.firingWeapon] > Time.time)
				{
					returnVal = true;
				}
			break;

		//MORE STATES WE DON'T USE
//			case PlayerStateController.playerStates.dash:
//            break;
//			
//			case PlayerStateController.playerStates.falling:
//			break;
//			
//			case PlayerStateController.playerStates.kill:
//			break;
//			
//			case PlayerStateController.playerStates.resurrect:
//			break;
			
		}
		
		// Value of true means 'Abort'. Value of false means 'Continue'.
		return returnVal;
	}

//    public void hitByCrusher()
//    {
//        onStateChange(PlayerStateController.playerStates.kill);
//    }
}
