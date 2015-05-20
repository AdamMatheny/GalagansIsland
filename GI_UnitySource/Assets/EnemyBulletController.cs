using UnityEngine;
using System.Collections;

public class EnemyBulletController : MonoBehaviour 
{
	
	public GameObject mPlayer = null;
	public float mBulletSpeed = 20.0f;
	private float mSelfDestructTimer = 0.0f;
	private ScoreManager mScoreController;
	public bool mShootable;
	public bool mAimAtPlayer = false;
	public bool mFixedFireDir = false;
	public Vector3 mFireDir;

	public void Start()
	{
		mPlayer = FindObjectOfType<PlayerShipController>().gameObject;
		mScoreController = FindObjectOfType<ScoreManager>();

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
			Vector3 directionToPlayer = mPlayer.transform.position-transform.position;
			bulletForce = Vector3.Normalize(directionToPlayer)*mBulletSpeed;
			transform.LookAt(mPlayer.transform.position);
			transform.rotation = Quaternion.Euler(new Vector3(90f,0f,0f) + transform.rotation.eulerAngles);
		}
		//Just fire up and down ~Adam
		else
		{
			if (mPlayer.transform.position.y > transform.position.y)
			{
				bulletForce = new Vector2(0.0f,mBulletSpeed);
			}
			else
			{
				bulletForce = new Vector2(0.0f,mBulletSpeed * -1.0f);
			}
		}


		GetComponent<Rigidbody>().velocity = bulletForce;
		mSelfDestructTimer = Time.time + 5.0f;
		
	}
	
	void Update()
	{
		if(mSelfDestructTimer>0.0f)
		{
			if (mSelfDestructTimer < Time.time)
				Destroy(gameObject);
		}
		if (Vector3.Distance(this.transform.position, mPlayer.transform.position) <= 1f)
		{
			Debug.Log("The player was shot");
			mScoreController.LoseALife();
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if(other.tag == "Player Bullet")
		{
			//Debug.Log("Hit a player bullet!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1");
			if(mShootable)
			{
				Destroy(other.gameObject);
				Destroy(this.gameObject);
			}
		}
	}


}