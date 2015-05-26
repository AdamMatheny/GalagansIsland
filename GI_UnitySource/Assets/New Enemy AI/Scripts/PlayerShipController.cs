using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerShipController : MonoBehaviour 
{

	public GameObject particleShoot;

	public bool cheats = false;
	//For if we ever animate the ship ~Adam
	[SerializeField] private Animator mMainShipAnimator;
	[SerializeField] private Animator mSecondShipAnimator;
	
	public float bulletShootSpeed = .4f;
	
	//For firing bullets on a set time interval ~Adam
	float mBulletFireTime = 0f;
	//The prefab we're using for the player bullets ~Adam
	public GameObject mBulletPrefab = null;
	
	public float mBaseMovementSpeed = 6f;
	public float mMovementSpeed = 0f;
	
	public Vector3 mMoveDir = new Vector3(0f,-1f,0f);
	
	//private float mTopMovementSpeed = 6f;
	
	//Variables for editing drop speeds ~Adam
	[SerializeField] private float mMaxDropSpeed = 0.4f;
	[SerializeField] private float mDropSpeed = 0;
	[SerializeField] private float mDropAccelRate = 0.05f;
	[SerializeField] private float mDropDeccelRate = 0.01f;
	
	//Used for the duplicating of the ship via Grabber Enemies ~Adam
	public bool mShipStolen = false;
	public bool mShipRecovered = false;
	
	//The game objects that are our ship sprites ~Adam
	public GameObject mMainShip;
	public GameObject mSecondShip;
	//Where our bullets spawn from `Adam
	//indexes:
	//0: Main ship, main bullet
	//1: Second ship, main bullet
	//2: Main ship, left bullet
	//3: Main ship, right bullet
	//4: Second ship, left bullet
	//5: Second ship, right bullet
	[SerializeField] private Transform[] mBulletSpawns;
	

	//For Overheating
	public bool overHeatProcess = true;
	public bool isOverheated = false;
	public float heatLevel = 0f;
	public float mBaseHeatMax = 60f;
	public float maxHeatLevel;
	[SerializeField] private Texture2D mOverheatTimerTex;
	[SerializeField] private Texture2D mOverheatWarningTex;

	//For when the player has 3 bullets  ~Adam
	public bool mThreeBullet = true;
	public float mThreeBulletTimer = 0f;
	public GameObject mSideBullet;
	// public GameObject leftBullet;
	// public GameObject rightBullet;
	[SerializeField] private Texture2D mBulletTimerTex;
	[SerializeField] private Texture2D mBulletTimerTexVert;
	[SerializeField] private Texture2D mBulletTimerTubeTex;
	
	//For when the ship has a shieldPowerUp ~Adam
	public bool mShielded = false;
	[SerializeField] private SpriteRenderer mMainShipShieldSprite;
	[SerializeField] private SpriteRenderer mSecondShipShieldSprite;
	[SerializeField] private Texture2D mShieldTimerTex;
	public float mShieldTimer = 0f;

	[SerializeField] private Texture2D mSideMetersTex;


	//For deleting duplicate ships when we change levels ~Adam
	public int mShipCreationLevel;
	public bool mToggleFireOn = true;
	
	//For tracking where the ship was last frame so we can see how much/in what direction its moving ~Adam
	public Vector3 mLastFramePosition;
	public Vector3 mLastFrameDifference = Vector3.zero;
	float mLastNonZeroHorizontalDifference;
	bool mDriftDown = true;
	
	//For spinning the ship around when the player gets hit ~Adam
	float mSpinning = 0f;
	float mSpinTimer = 0f;
	float mSpinTimerDefault = 0.5f;
	
	//For Super Screen-Wiper powerup ~Adam
	public GameObject mLaserFist;
	public GameObject mBigBlast;



	//For making the ship flash when hit
	public GameObject mMainShipHitSprite;
	public GameObject mSecondShipHitSprite;

	// Use this for initialization
	void Start () 
	{
		mShipCreationLevel = Application.loadedLevel;
		
		PlayerShipController[] otherPlayerShips = FindObjectsOfType<PlayerShipController>();
		//Debug.Log(otherPlayerShip.name);
		foreach(PlayerShipController othership in otherPlayerShips)
		{
			if(othership.mShipCreationLevel < this.mShipCreationLevel)
			{
				Debug.Log("Found another ship so destroying self.");
				Destroy(this.gameObject);
			}
		}
		
		mLastFramePosition = transform.position;
		
	}//END of Start()
	
	
	//Pesist between level loads/reloads ~Adam
	void Awake()
	{
		DontDestroyOnLoad (transform.gameObject);
	}//END of Awake()
	
	
	// Update is called once per frame
	void Update () 
	{
		maxHeatLevel = mBaseHeatMax +  mBaseHeatMax * Application.loadedLevel/26f;

		mMovementSpeed *= Time.deltaTime / Time.timeScale;
		
		if (cheats) {
			
			if(Input.GetKeyDown(KeyCode.Q))
			{
				Application.LoadLevel(Application.loadedLevel + 1);
				mShipStolen = false;
			}
			
			if(Input.GetKeyDown(KeyCode.R))
			{
				Application.LoadLevel(Application.loadedLevel - 1);
				mShipStolen = false;
			}
		}
		
		//Spin the ships when hit
		if(mSpinning != 0f)
		{
			mSpinTimer -= Time.deltaTime;
			SpinShip(mSpinning);
			
			if (mSpinTimer <= 0f)
			{
				mSpinning = 0f;
				mMainShip.transform.rotation = Quaternion.identity;
				mSecondShip.transform.rotation = Quaternion.identity;
			}
		}
		
		
		//Toggle shield sprites ~Adam
		if(mShielded)
		{
			mMainShipShieldSprite.GetComponent<Animator>().SetInteger ("ShieldState", 1);
			mSecondShipShieldSprite.GetComponent<Animator>().SetInteger ("ShieldState", 1);

			mMainShipShieldSprite.enabled = true;
			mMainShipShieldSprite.GetComponent<Light>().enabled = true;
			if(mShipRecovered)
			{
				mSecondShipShieldSprite.enabled = true;
				mSecondShipShieldSprite.GetComponent<Light>().enabled = true;
			}
			//Decrease Shield time ~Adam
			mShieldTimer -= Time.deltaTime;
			if(mShieldTimer <= 0f)
			{
				mShielded = false;
			}
			if(mShieldTimer < 2f)
			{
				mMainShipShieldSprite.GetComponent<Animator>().SetInteger ("ShieldState", 0);
				mSecondShipShieldSprite.GetComponent<Animator>().SetInteger ("ShieldState", 0);

			}
			if(mShieldTimer < 5f)
			{
//				mMainShipShieldSprite.GetComponent<Animator>().speed = 0.5f;
//				mMainShipShieldSprite.GetComponent<Animator>().Play("ShieldSprite_Flicker");
//				mSecondShipShieldSprite.GetComponent<Animator>().speed = 0.5f;
//				mSecondShipShieldSprite.GetComponent<Animator>().Play("ShieldSprite_Flicker");
				mMainShipShieldSprite.GetComponent<Renderer>().material.color = Color.Lerp (mMainShipShieldSprite.GetComponent<Renderer>().material.color, Color.red,0.1f);
				mSecondShipShieldSprite.GetComponent<Renderer>().material.color = Color.Lerp (mSecondShipShieldSprite.GetComponent<Renderer>().material.color, Color.red,0.1f);
			}
			else
			{
				mMainShipShieldSprite.GetComponent<Renderer>().material.color = Color.white;
				mSecondShipShieldSprite.GetComponent<Renderer>().material.color = Color.white;
//				mMainShipShieldSprite.GetComponent<Animator>().speed = 0f;
//				mMainShipShieldSprite.GetComponent<Animator>().Play("ShieldSprite_Solid");
//				mMainShipShieldSprite.GetComponent<Animator>().StopPlayback();
//				mSecondShipShieldSprite.GetComponent<Animator>().speed = 0f;
//				mSecondShipShieldSprite.GetComponent<Animator>().Play("ShieldSprite_Solid");
//				mSecondShipShieldSprite.GetComponent<Animator>().StopPlayback();
			}

		}
		else
		{
			mMainShipShieldSprite.enabled = false;
			mMainShipShieldSprite.GetComponent<Light>().enabled = false;
			mSecondShipShieldSprite.enabled = false;
			mSecondShipShieldSprite.GetComponent<Light>().enabled = false;
			
		}
		
		//Increase movement speed as we progress through levels
		mMovementSpeed = mBaseMovementSpeed + (4f/25f*Application.loadedLevel);
		
		//Make the player drift toward the bottom of the screen
		// transform.position += new Vector3(0f,mDropSpeed*-1f, 0f);
		if(mMoveDir.y < 0f && mDriftDown)
		{
			foreach (ParticleSystem shipTrail in this.GetComponentsInChildren<ParticleSystem>())
			{
				shipTrail.enableEmission = false;
			}


			if(mDropSpeed < mMaxDropSpeed)
			{
				mDropSpeed += mDropAccelRate;
			}
			else
			{
				mDropSpeed = mMaxDropSpeed;
			}
			
		}
		else
		{
			foreach (ParticleSystem shipTrail in this.GetComponentsInChildren<ParticleSystem>())
			{
				shipTrail.enableEmission = true;
			}

			mDropSpeed -= mDropDeccelRate;
			
			if(mDropSpeed <= 0.01f)
			{
				mDropSpeed = 0.01f;
			}
		}
		
		//Make the player drift faster towards the bottom while firing ~Adam
		if(mToggleFireOn)
		{
			transform.position += new Vector3(0f,-0.00255f*Application.loadedLevel, 0f);
			//Decrease the timer on triple bullets while firing ~Adam
			mThreeBulletTimer -= Time.deltaTime;
		}
		

		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		
		
		//Delete the ship if we've returned to the title screen
		if(Application.loadedLevel == 0)
		{
			Destroy(this.gameObject);
		}
		

		
		
		
		
		//Keyboard Movement Controls
		//For making the ship drift down when not trying to go up
		if(vertical > 0f)
		{
			mDriftDown = false;
		}
		else
		{
			mDriftDown = true;
		}
		
		//Movement input for mouse/touch
		if(Input.GetMouseButton(0) && (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android))
		{
			Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
			//Debug.Log(screenPos + ", " + Input.mousePosition);
			Vector3 translationDirection = Vector3.Normalize(Input.mousePosition-screenPos);
			//Debug.Log(translationDirection*mMovementSpeed*Time.deltaTime);
			
			//For making the ship drift down when not trying to go up
			if(Input.mousePosition.y > screenPos.y-10f)
			{
				mDriftDown = false;
			}
			else
			{
				mDriftDown = true;
			}
			//transform.Translate(new Vector3(translationDirection.x, translationDirection.y, 0f)*mMovementSpeed*Time.deltaTime);
			//mMoveDir +=new Vector3(translationDirection.x, translationDirection.y, 0f)*0.5f*mMovementSpeed*Time.deltaTime;
			mMoveDir = Vector3.Lerp(mMoveDir, new Vector3(translationDirection.x, translationDirection.y, 0f)*2f*mMovementSpeed*Time.deltaTime, 0.08f);
			
		}
		
		// if (Input.GetKey (KeyCode.Mouse0)) {
		// transform.Translate(new Vector3(0.0f, (mMovementSpeed * Time.deltaTime) + .6f, 0.0f));
		// }
		
		//Taking in diretional Input from the keyboard
		else if (horizontal != 0.0f || vertical != 0.0f)
		{
			
			
			//Left
			if (horizontal < 0.0f && vertical == 0.0f)
			{
				//transform.Translate(new Vector3((mMovementSpeed * -1.0f) * Time.deltaTime, 0.0f, 0.0f));
				mMoveDir = Vector3.Lerp(mMoveDir, new Vector3((2f*mMovementSpeed * -1.0f) * Time.deltaTime, 0.0f, 0.0f), 0.08f);
			}
			//Right
			else if (horizontal > 0.0f && vertical == 0.0f)
			{
				//transform.Translate(new Vector3(mMovementSpeed * Time.deltaTime, 0.0f, 0.0f));
				mMoveDir = Vector3.Lerp(mMoveDir,new Vector3(2f*mMovementSpeed * Time.deltaTime, 0.0f, 0.0f), 0.08f);
			}
			//Down
			else if (vertical < 0.0f && horizontal == 0.0f)
			{ 
				//transform.Translate(new Vector3(0.0f, (mMovementSpeed * -1.0f) * Time.deltaTime, 0.0f));
				mMoveDir = Vector3.Lerp(mMoveDir, new Vector3(0.0f, (2f*mMovementSpeed * -1.0f) * Time.deltaTime, 0.0f), 0.08f);
			}
			//Up
			else if (vertical > 0.0f && horizontal == 0.0f)
			{
				//transform.Translate(new Vector3(0.0f, mMovementSpeed * Time.deltaTime, 0.0f));
				mMoveDir = Vector3.Lerp(mMoveDir, new Vector3(0.0f, 2f*mMovementSpeed * Time.deltaTime, 0.0f), 0.08f);
			}
			//Up+Right
			else if (vertical > 0.0f && horizontal > 0.0f)
			{
				//transform.Translate(Vector3.Normalize(new Vector3(1f,1f,0))*mMovementSpeed * Time.deltaTime );
				mMoveDir = Vector3.Lerp(mMoveDir, Vector3.Normalize(new Vector3(1f,1f,0))*2f*mMovementSpeed * Time.deltaTime , 0.08f);
			}
			//Up+Left
			else if (vertical > 0.0f && horizontal < 0.0f)
			{
				//transform.Translate(Vector3.Normalize(new Vector3(-1f,1f,0))*mMovementSpeed * Time.deltaTime );
				mMoveDir = Vector3.Lerp(mMoveDir, Vector3.Normalize(new Vector3(-1f,1f,0))*2f*mMovementSpeed * Time.deltaTime , 0.08f);
			}
			//Down+Right
			else if (vertical < 0.0f && horizontal > 0.0f)
			{
				//transform.Translate(Vector3.Normalize(new Vector3(1f,-1f,0))*mMovementSpeed * Time.deltaTime );
				mMoveDir = Vector3.Lerp(mMoveDir, Vector3.Normalize(new Vector3(1f,-1f,0))*2f*mMovementSpeed * Time.deltaTime, 0.08f);
			}
			//Down+Left
			else if (vertical < 0.0f && horizontal < 0.0f)
			{
				//transform.Translate(Vector3.Normalize(new Vector3(-1f,-1f,0))*mMovementSpeed * Time.deltaTime );
				mMoveDir = Vector3.Lerp(mMoveDir, Vector3.Normalize(new Vector3(-1f,-1f,0))*2f*mMovementSpeed * Time.deltaTime, 0.08f);
			}
		}
		//END of Keyboard Movement Controls
		
		//Toggle bullet firing
		//if(Input.getbu("Fire2"))
		if(Input.GetButtonDown("FireGun"))
		{
			ToggleFire();
		}
		
		if (isOverheated) 
		{
			mToggleFireOn = false;
			mMainShip.GetComponent<Renderer>().material.color = Color.Lerp(mMainShip.GetComponent<Renderer>().material.color,Color.red,0.05f);
			mSecondShip.GetComponent<Renderer>().material.color = Color.Lerp(mSecondShip.GetComponent<Renderer>().material.color,Color.red,0.05f);
		}
		else if (heatLevel/maxHeatLevel > 0.9f) 
		{
			mMainShip.GetComponent<Renderer>().material.color = Color.Lerp(mMainShip.GetComponent<Renderer>().material.color,Color.yellow,0.1f);
			mSecondShip.GetComponent<Renderer>().material.color = Color.Lerp(mSecondShip.GetComponent<Renderer>().material.color,Color.yellow,0.1f);
		}

		else
		{
			mMainShip.GetComponent<Renderer>().material.color = Color.Lerp(mMainShip.GetComponent<Renderer>().material.color,Color.white,0.1f);
			mSecondShip.GetComponent<Renderer>().material.color = Color.Lerp(mSecondShip.GetComponent<Renderer>().material.color,Color.white,0.1f);
		}

		//firing bullets
		if (mToggleFireOn) 
		{
			
			if(!isOverheated)
			{
				if(heatLevel < maxHeatLevel)
				{
					heatLevel += Time.deltaTime;
				}
				
				if(heatLevel >= maxHeatLevel)
				{
					
					heatLevel = maxHeatLevel;
					isOverheated = true;
				}

				//Firing Bullets
				if (Time.time > mBulletFireTime) 
				{
					
					// Make the bullet object
					GameObject newBullet = Instantiate (mBulletPrefab, mBulletSpawns[0].position, mMainShip.transform.rotation * Quaternion.Euler (0f,0f,Random.Range(-3.0f,3.0f))) as GameObject;
					//GameObject newBullet = Instantiate(mBulletPrefab, transform.position+new Vector3(0,2.5f,0), Quaternion.identity) as GameObject;
					if (mThreeBullet) 
					{

						if(!mShipRecovered)
						{
							Instantiate (mSideBullet, mBulletSpawns[2].position, mMainShip.transform.rotation * Quaternion.Euler (0f, 0f, 10f) * Quaternion.Euler (0f,0f,Random.Range(-5.0f,5.0f)));
						}
						else
						{
							Instantiate (mSideBullet, mBulletSpawns[2].position, mMainShip.transform.rotation * Quaternion.Euler (0f, 0f, 5f) * Quaternion.Euler (0f,0f,Random.Range(-10.0f,3.0f)));
						}
						Instantiate (mSideBullet, mBulletSpawns[3].position, mMainShip.transform.rotation * Quaternion.Euler (0f, 0f, -10f) * Quaternion.Euler (0f,0f,Random.Range(-5.0f,5.0f)));
						
						//Instantiate(leftBullet, transform.position+new Vector3(-2f,3f,0), Quaternion.identity);
						//Instantiate(rightBullet, transform.position+new Vector3(2,3f,0), Quaternion.identity);
					}
					//Instantiate(particleShoot, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
					GetComponent<AudioSource> ().Play ();
					Camera.main.GetComponent<CameraShaker> ().ShakeCamera ();
					if (mShipRecovered) 
					{
						//Instantiate(particleShoot, transform.position + new Vector3(-3.5f, 2, 0), Quaternion.identity);
						GameObject secondBullet;
						secondBullet = Instantiate (mBulletPrefab, mBulletSpawns[1].position, mSecondShip.transform.rotation * Quaternion.Euler (0f,0f,Random.Range(-3.0f,3.0f))) as GameObject;
						secondBullet.name = "SECONDBULLET";
						if (mThreeBullet) 
						{
							Instantiate (mSideBullet, mBulletSpawns[4].position, mSecondShip.transform.rotation * Quaternion.Euler (0f, 0f, 10f) * Quaternion.Euler (0f,0f,Random.Range(-5.0f,5.0f)));

							Instantiate (mSideBullet, mBulletSpawns[5].position, mSecondShip.transform.rotation * Quaternion.Euler (0f, 0f, -5f) * Quaternion.Euler (0f,0f,Random.Range(-3.0f,10.0f)));
						}
					}
					//Reset the timer to fire bullets.  The later the level, the smaller the time between shots
					if(mSpinning == 0)
					{
						if(Application.loadedLevelName != "Credits")
						{
							mBulletFireTime = Time.time + bulletShootSpeed - (0.15f / 25f * Application.loadedLevel);
						}
						else
						{
							mBulletFireTime = Time.time + (bulletShootSpeed - (0.15f / 25f * 21f));
						}
					}
					else
					{
							mBulletFireTime = Time.time + (bulletShootSpeed - (0.15f / 25f * Application.loadedLevel))/3f;
					}
				}
			}
		}
		else 
		{
			
			if(heatLevel > 0)
			{
				
				if(isOverheated)
				{
					heatLevel -= Time.deltaTime * maxHeatLevel/5f;
				}
				else
				{
					heatLevel -= Time.deltaTime * 3f;
				}
			}
		}
		
		if (heatLevel <= 0f) 
		{
			
			isOverheated = false;
		}
		
		if(Input.GetButton("Thrusters"))
		{
			mDropSpeed -= mDropDeccelRate*3f;
			if(mDropSpeed <= 0.01f)
			{
				mDropSpeed = 0.00f;
			}

			if(mMoveDir.y < -0.2f)
			{
				foreach (ParticleSystem shipTrail in this.GetComponentsInChildren<ParticleSystem>())
				{
					shipTrail.enableEmission = false;
				}
			}
			else
			{
				foreach (ParticleSystem shipTrail in this.GetComponentsInChildren<ParticleSystem>())
				{
					shipTrail.enableEmission = true;
				}
			}
		}
		
		//Move the ship by the mMoveDir vector if not paused
		if(Time.timeScale == 1)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
			{
					if(mMoveDir.y < 0f && !(vertical == 0.0f && !Input.GetMouseButton(0)))
				{
					mMoveDir = Vector3.Lerp(mMoveDir, mMoveDir+ new Vector3(0f,-mDropSpeed,0f), 0.08f);
				}
				transform.Translate(mMoveDir);
				
				if (vertical == 0.0f && !Input.GetMouseButton(0))
				{
					mDriftDown = true;
					mMoveDir = Vector3.Lerp(mMoveDir, new Vector3(mMoveDir.x,-mDropSpeed,mMoveDir.z), 0.2f);
					mMoveDir = Vector3.Lerp(mMoveDir, new Vector3(0f,-mDropSpeed,0f), 0.03f);

				}
			}
			else
			{
				if(mMoveDir.y < 0f && !(vertical == 0.0f))
				{
					mMoveDir = Vector3.Lerp(mMoveDir, mMoveDir+ new Vector3(0f,-mDropSpeed,0f), 0.08f);
				}
				transform.Translate(mMoveDir);
				
				if (vertical == 0.0f)
				{
					mDriftDown = true;
					mMoveDir = Vector3.Lerp(mMoveDir, new Vector3(mMoveDir.x,-mDropSpeed,mMoveDir.z), 0.2f);
					mMoveDir = Vector3.Lerp(mMoveDir, new Vector3(0f,-mDropSpeed,0f), 0.03f);
					
				}
			}
		}
		mMainShipAnimator.speed = Application.loadedLevel/5f+1f;
		mSecondShipAnimator.speed = Application.loadedLevel/5f+1f;
		if(mToggleFireOn)
		{
			mMainShipAnimator.SetBool("IsFiring", true);
			mSecondShipAnimator.SetBool("IsFiring", true);
		}
		else
		{
			mMainShipAnimator.SetBool("IsFiring", false);
			mSecondShipAnimator.SetBool("IsFiring", false);
		}
		
		
		//Control whether or not to render the second ship 
		if (mShipRecovered)
		{
			mSecondShip.GetComponent<SpriteRenderer>().enabled = true;
			foreach (ParticleSystem shipTrail in mSecondShip.GetComponentsInChildren<ParticleSystem>())
			{
				if(!(mMoveDir.y < 0f && mDriftDown))
				{
					shipTrail.enableEmission = true;
				}
			}
		}
		else
		{
			mSecondShip.GetComponent<SpriteRenderer>().enabled = false;
			foreach (ParticleSystem shipTrail in mSecondShip.GetComponentsInChildren<ParticleSystem>())
			{
				shipTrail.enableEmission = false;
			}
		}

	}//END of Update()
	
	void LateUpdate () 
	{
		//Keep ship within screen bounds
		if (transform.position.x < -17.5 && mShipRecovered)
		{
			transform.position = new Vector3(-17.5f, transform.position.y, transform.position.z);
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
		
		if(mThreeBulletTimer <= 0f)
		{
			mThreeBullet = false;
		}
	}//END of LateUpdate()
	
	public void ToggleFire()
	{
		mToggleFireOn = !mToggleFireOn;
	}//END of ToggleFire()
	
	void OnLevelWasLoaded(){
		Input.ResetInputAxes();

		//mToggleFireOn = false;
	}
	
	void OnGUI()
	{
		//For the spread fire timer that follows the ship -Adam
		if(mThreeBullet)
		{
			Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
			//Draw the triple bullet timer ~Adam
			//GUI.DrawTexture(new Rect(screenPos.x-Screen.width*0.016f, Screen.height-screenPos.y+Screen.height*0.022f, Screen.width*0.0008f*30+Screen.width*0.008f, Screen.height*0.012f),mBulletTimerTubeTex); 
			GUI.DrawTexture(new Rect(screenPos.x+Screen.width*0.016f, Screen.height-screenPos.y+Screen.height*0.0315f, Screen.width*0.006f, -1f*(Screen.height*0.045f+Screen.height*0.014f)),mBulletTimerTubeTex); 

			//GUI.DrawTexture(new Rect(screenPos.x-Screen.width*0.012f, Screen.height-screenPos.y+Screen.height*0.022f, Screen.width*0.0008f*mThreeBulletTimer, Screen.height*0.012f),mBulletTimerTex); 
			GUI.DrawTexture(new Rect(screenPos.x+Screen.width*0.016f, Screen.height-screenPos.y+Screen.height*0.025f, Screen.width*0.006f, -1f*(Screen.height*0.045f*mThreeBulletTimer/30f)),mBulletTimerTexVert); 

		}
//		elase if(mShielded && !mThreeBullet)
//		{
//			Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
//			//Draw the shield timer ~Adam
//			GUI.DrawTexture(new Rect(screenPos.x-Screen.width*0.016f, Screen.height-screenPos.y+Screen.height*0.022f, Screen.width*0.0008f*30+Screen.width*0.008f, Screen.height*0.012f),mBulletTimerTubeTex); 
//			GUI.DrawTexture(new Rect(screenPos.x-Screen.width*0.012f, Screen.height-screenPos.y+Screen.height*0.022f, Screen.width*0.0008f*mShieldTimer, Screen.height*0.012f),mShieldTimerTex); 
//		}
//		else if (mThreeBullet && mShielded)
//		{
//			Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
//			//Draw the triple bullet timer ~Adam
//			GUI.DrawTexture(new Rect(screenPos.x-Screen.width*0.016f, Screen.height-screenPos.y+Screen.height*0.015f, Screen.width*0.0008f*30+Screen.width*0.008f, Screen.height*0.012f),mBulletTimerTubeTex); 
//			GUI.DrawTexture(new Rect(screenPos.x-Screen.width*0.012f, Screen.height-screenPos.y+Screen.height*0.015f, Screen.width*0.0008f*mThreeBulletTimer, Screen.height*0.012f),mBulletTimerTex); 
//			//Draw the shield timer ~Adam
//			GUI.DrawTexture(new Rect(screenPos.x-Screen.width*0.016f, Screen.height-screenPos.y+Screen.height*0.027f, Screen.width*0.0008f*30+Screen.width*0.008f, Screen.height*0.012f),mBulletTimerTubeTex); 
//			GUI.DrawTexture(new Rect(screenPos.x-Screen.width*0.012f, Screen.height-screenPos.y+Screen.height*0.027f, Screen.width*0.0008f*mShieldTimer, Screen.height*0.012f),mShieldTimerTex); 
//		}


		//For drawing meters on the side of the screen
//		if(Application.loadedLevelName != "Credits")
//		{
//			GUI.DrawTexture(new Rect(Screen.width * .9f, Screen.height * 0.890f, Screen.width * .09f, Screen.height * .1f), mSideMetersTex);
//			GUI.DrawTexture(new Rect(Screen.width * .901f, Screen.height * 0.892f, Screen.width*.0542f*(heatLevel/90f), Screen.height*0.03f),mOverheatTimerTex);
//			if(isOverheated)
//			{
//				GUI.DrawTexture(new Rect(Screen.width * .901f, Screen.height * 0.892f, Screen.width*.0542f, Screen.height*0.03f),mOverheatWarningTex);
//			}
//			else
//			{
//				GUI.DrawTexture(new Rect(Screen.width * .901f+Screen.width*.0502f*(maxHeatLevel/90f), Screen.height * 0.892f, Screen.width*.004f, Screen.height*0.03f),mOverheatWarningTex);
//			}
//			if(mThreeBullet)
//			{
//				GUI.DrawTexture(new Rect(Screen.width * .901f, Screen.height * 0.9235f, Screen.width*.0542f*Mathf.Max(mThreeBulletTimer/30f,0f), Screen.height*0.03f),mBulletTimerTex); 
//			}
//			if(mShielded)
//			{
//				GUI.DrawTexture(new Rect(Screen.width * .901f, Screen.height * 0.955f, Screen.width*.0542f*(mShieldTimer/30f), Screen.height*0.03f),mShieldTimerTex); 
//			}
//		}

	}
	
	public void StartSpin()
	{
		mSpinTimer = mSpinTimerDefault;
		mSpinning = Random.Range(-1,1);
		if (mSpinning == 0f)
		{
			mSpinning += 0.1f;
		}
	}
	
	/*public void OnTriggerEnter(Collider other){

Debug.Log (other);

if (other.gameObject.GetComponent<Laser> () != null) {

Debug.Log("Collided with IT!");
}
}*/
	
	public void SpinShip(float spinDir)
	{
		if(spinDir > 0f)
		{
			mMainShip.transform.Rotate(Vector3.forward*Time.deltaTime*720f);
			mSecondShip.transform.Rotate(Vector3.forward*Time.deltaTime*-720f);
		}
		else if (spinDir < 0f)
		{
			mMainShip.transform.Rotate(Vector3.forward*Time.deltaTime*-720f);
			mSecondShip.transform.Rotate(Vector3.forward*Time.deltaTime*720f);
		}
		
	}
	
	//For getting hit by boss beams ~Adam
	void OnParticleCollision(GameObject other)
	{
		Debug.Log("The player was shot by a particle");
		FindObjectOfType<ScoreManager>().LoseALife();
	}
	
}//END of MonoBehavior