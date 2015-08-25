using UnityEngine;
using System.Collections;

public class LDBossEntrance : MonoBehaviour 
{
	public GameObject mBoss;
	float mTimer = 1f;
	public AudioClip mBossMusic;

	// Use this for initialization
	void Start () 
	{
		transform.SetParent (null);
		mBoss.SetActive (false); 
		Camera.main.GetComponentInChildren<AudioSource>().clip = mBossMusic;
		Camera.main.GetComponentInChildren<AudioSource>().Play();
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
