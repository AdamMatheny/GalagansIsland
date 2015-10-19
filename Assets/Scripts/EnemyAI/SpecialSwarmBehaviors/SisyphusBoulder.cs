using UnityEngine;
using System.Collections;

public class SisyphusBoulder : MonoBehaviour 
{
	public bool mClimbing = true;

	public float mClimbSpeed = 5f;
	public float mFallSpeed = 15f;


	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Push the boulder up to the top ~Adam
		if(mClimbing)
		{
			//transform.position += Vector3.up*mClimbSpeed;
			transform.position = Vector3.Lerp (transform.position, new Vector3(0f,24f,-2f), 0.001f*mClimbSpeed);
			if(transform.position.y < 20f)
			{
				transform.RotateAround (transform.position, Vector3.forward, mClimbSpeed*0.5f);
			}
			if(transform.position.y >= 22f)
			{
				mClimbing = false;
			}
		}
		//Boulder falls back down ~Adam
		else
		{
			//transform.position += Vector3.up*mClimbSpeed;
			transform.position = Vector3.Lerp (transform.position, new Vector3(0f,-53f,-2f), 0.001f*mFallSpeed);
			//transform.RotateAround (transform.position, Vector3.forward, mFallSpeed*-0.1f);
			if(transform.position.y <= -52.5f)
			{
				mClimbing = true;
			}
		}

	}
}
