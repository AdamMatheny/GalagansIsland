using UnityEngine;
using System.Collections;

public class LaserFistPowerup : MonoBehaviour 
{
	BoxCollider mDeathBox;

	Vector3 mLaserCenterFull = new Vector3(0f,4.8f,0f);
	Vector3 mLaserSizeFull = new Vector3(1f,10f, 3f);

	Vector3 mLaserCenterStart = new Vector3(0f,0f,0f);
	Vector3 mLaserSizeStart = new Vector3(0.1f,0.1f, 3f);

	public float maxTime;
	public float time;

	public GameObject bigBoom;


	float mLaserFistTimer = 0f;

	// Use this for initialization
	void Start () 
	{
		mDeathBox = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (bigBoom != null) {

			if (time > 0) {
				
				time -= Time.deltaTime;
			} else {
				
				time = maxTime;
				bigBoom.SetActive(true);
			}
		}

		mLaserFistTimer += Time.deltaTime;

		if(GetComponent<Animator>().GetBool("Expanding"))
		{
			mDeathBox.center = Vector3.Lerp(mDeathBox.center, mLaserCenterFull, 0.1f);
			mDeathBox.size = Vector3.Lerp(mDeathBox.size, mLaserSizeFull, 0.1f);
		}
		else
		{
			mDeathBox.center = Vector3.Lerp(mDeathBox.center, mLaserCenterStart, 0.1f);
			mDeathBox.size = Vector3.Lerp(mDeathBox.size, mLaserSizeStart, 0.1f);

		}

		if(mLaserFistTimer >= 5f)
		{
			StartDeathBoxShrinkage();
		}
	}

	public void StartDeathBoxExpansion()
	{
		GetComponent<Animator>().SetBool("Expanding", true);
	}//END StartDeathBoxExpansion()

	public void StartDeathBoxShrinkage()
	{
		GetComponent<Animator>().SetBool("Expanding", false);
	}//END StartDeathBoxExpansion()

	public void StopLaserFist()
	{
		mLaserFistTimer = 0f;
		this.gameObject.SetActive(false);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<EnemyShipAI>() != null)
		{
			other.GetComponent<EnemyShipAI>().EnemyShipDie();
		}
		if(other.GetComponent<EnemyBulletController>() != null)
		{
			if(other.GetComponent<EnemyBulletController>().mShootable)
			{
				Destroy(other.gameObject);
			}
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(other.GetComponent<EnemyShipAI>() != null)
		{
			other.GetComponent<EnemyShipAI>().EnemyShipDie();
		}
	}
}
