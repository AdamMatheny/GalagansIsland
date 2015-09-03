using UnityEngine;
using System.Collections;

//This is for parts of the boss that are meant to hurt the player by just touching them ~Adam

public class BossWeaponCollision : MonoBehaviour 
{
	ScoreManager mScoreMan;

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
		if(other.GetComponent<PlayerShipController>()!= null)
		{
			mScoreMan.HitAPlayer(other.gameObject);
		}
	}
}
