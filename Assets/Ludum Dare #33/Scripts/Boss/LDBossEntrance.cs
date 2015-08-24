using UnityEngine;
using System.Collections;

public class LDBossEntrance : MonoBehaviour 
{
	public GameObject mBoss;
	float mTimer = 1f;
	// Use this for initialization
	void Start () 
	{
		transform.SetParent (null);
		mBoss.SetActive (false); 
	}
	
	// Update is called once per frame
	void Update () 
	{
		mTimer -= Time.deltaTime;
		if(mTimer <= 0f)
		{
			mBoss.SetActive (true);
		}
	}
}
