using UnityEngine;
using System.Collections;

public class EnemyBulletController : MonoBehaviour 
{
	
	public GameObject mPlayer = null;
	public GameObject mPlayerClone = null; //For co-op mode ~Adam
	public float mBulletSpeed = 20.0f;
	private float mSelfDestructTimer = 0.0f;
	private ScoreManager mScoreController;
	public bool mShootable;
	public bool mAimAtPlayer = false;
	public bool mFixedFireDir = false;
	public Vector3 mFireDir;

	public GameObject bulletExplosion;

	public void Start()
	{
		mPlayer = FindObjectOfType<PlayerShipController>().gameObject;
		mScoreController = FindObjectOfType<ScoreManager>();
		#region co-op mode stuff
		if(FindObjectOfType<PlayerTwoShipController>() != null)
		{
			mPlayerClone = FindObjectOfType<PlayerTwoShipController>().gameObject;
		}
		#endregion
		Vector2 bulletForce;

		//Used for firing in a particular pattern (i.e. rotational pattern on boss horns)~Adam
		if(mFixedFireDir)
		{
			bulletForce = mFireDir*mBulletSpeed;

			//transform.rotation = Quaternion.Euler(new Vector3(90f,0f,0f) + transform.rotation.eulerAngles);
		}
		//Used for aiming at the player ~Adam
		else if (mAimAtPlayer)
		{
			Vector3 directionToPlayer = Vector3.down;
			#region twin-stick clone stuff
			//Fire at the clone ship if it is both present and closer -Adam
			if(mPlayerClone != null && Vector3.Distance(transform.position,mPlayerClone.transform.position) <= Vector3.Distance(transform.position,mPlayer.transform.position) )
			{
				directionToPlayer = mPlayerClone.transform.position-transform.position;
				bulletForce = Vector3.Normalize(directionToPlayer)*mBulletSpeed;
				transform.LookAt(mPlayerClone.transform.position);
				transform.rotation = Quaternion.Euler(new Vector3(90f,0f,0f) + transform.rotation.eulerAngles);
			}
			#endregion
			else
			{
				//fire at the player
				directionToPlayer = mPlayer.transform.position-transform.position;
				bulletForce = Vector3.Normalize(directionToPlayer)*mBulletSpeed;
				transform.LookAt(mPlayer.transform.position);
				transform.rotation = Quaternion.Euler(new Vector3(90f,0f,0f) + transform.rotation.eulerAngles);
			}
		}
		//Just fire up and down ~Adam
		else
		{
			#region twin-stick clone stuff
			if(mPlayerClone != null && Vector3.Distance(transform.position,mPlayerClone.transform.position) <= Vector3.Distance(transform.position,mPlayer.transform.position) )
			{
				if (mPlayerClone.transform.position.y > transform.position.y)
				{
					bulletForce = new Vector2(0.0f,mBulletSpeed);
				}
				else
				{
					bulletForce = new Vector2(0.0f,mBulletSpeed * -1.0f);
				}
			}
			#endregion
			else
			{
				//Fire up/down
				if (mPlayer.transform.position.y > transform.position.y)
				{
					bulletForce = new Vector2(0.0f,mBulletSpeed);
				}
				else
				{
					bulletForce = new Vector2(0.0f,mBulletSpeed * -1.0f);
				}

			}
		}


		GetComponent<Rigidbody>().velocity = bulletForce;
		mSelfDestructTimer = Time.time + 5.0f;
		
	}
	
	void Update()
	{
		//Self-destruct after a certain amount of time
		if(mSelfDestructTimer>0.0f)
		{
			if (mSelfDestructTimer < Time.time)
				Destroy(gameObject);
		}

		//Detect distance to player and slow down time if close but not quite hitting ~Adam
		if (Vector3.Distance(this.transform.position, mPlayer.transform.position) <= 2.5f)
		{
			if(FindObjectOfType<SlowTimeController>()!= null)
			{
				FindObjectOfType<SlowTimeController>().SlowDownTime(0.4f,1f);
			}
		}
		//Detect distance to player and kill the player and destroy self if close enough to "touch" ~Adam
		if (Vector3.Distance(this.transform.position, mPlayer.transform.position) <= 1.5f)
		{
			Debug.Log("The player was shot");
			mScoreController.LoseALife();
			Destroy(gameObject);
		}

		#region twin-stick clone stuff
		//Detect distance to player clone and kill the clone and destroy self if close enough to "touch" ~Adam
		if(mPlayerClone != null)
		{
			if (Vector3.Distance(this.transform.position, mPlayerClone.transform.position) <= 1.5f)
			{
				Debug.Log("The clone was shot");
				mScoreController.LosePlayerTwoLife();
				Destroy(gameObject);
			}
		}
		#endregion
	}//END of Update()

	void OnTriggerEnter (Collider other)
	{
		if(other.tag == "Player Bullet")
		{
			//Debug.Log("Hit a player bullet!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1");
			if(mShootable)
			{

				if(bulletExplosion != null)
				{

					Instantiate(bulletExplosion, transform.position, Quaternion.identity);
				}

				Destroy(other.gameObject);
				Destroy(this.gameObject);
			}
		}

	}//END of OnTriggerEnter()



}