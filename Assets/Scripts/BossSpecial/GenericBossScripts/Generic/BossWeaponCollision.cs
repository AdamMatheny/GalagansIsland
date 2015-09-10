using UnityEngine;
using System.Collections;

//This is for parts of the boss that are meant to hurt the player by just touching them ~Adam

public class BossWeaponCollision : MonoBehaviour 
{
	ScoreManager mScoreMan;
	[SerializeField] private int mDamage = 3;
	// Use this for initialization
	void Start () 
	{
		mScoreMan = FindObjectOfType<ScoreManager>();
	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		
	}//END of Update()


	void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<PlayerShipController>()!= null && mScoreMan.mPlayerSafeTime <= 0f)
		{
			for (int i = 0; i < mDamage-1;i++)
			{
				mScoreMan.HitAPlayer(other.gameObject);
				mScoreMan.mPlayerSafeTime = -1f;
			}
			mScoreMan.HitAPlayer(other.gameObject);
			//Debug.Log("Visual Hindrance!");
		}
	}
}
