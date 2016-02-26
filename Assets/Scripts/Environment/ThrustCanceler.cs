using UnityEngine;
using System.Collections;

public class ThrustCanceler : MonoBehaviour 
{

	[SerializeField] private float mActivationDelay = 30f;
	bool mThrustCancelOn = false;
	[SerializeField] private PlayerOneShipController mP1Ship;
	[SerializeField] private PlayerTwoShipController mP2Ship;
	[SerializeField] private GameObject mGraphicEffect;

	[SerializeField] LevelKillCounter mKillCounter;

	// Use this for initialization
	void Start () 
	{
		StartCoroutine(StartActivationDelay());
	}



	// Update is called once per frame
	void Update () 
	{
		if(mKillCounter.mLevelComplete)
		{
			if(mP1Ship != null)
			{
				mP1Ship.EnableHover();
			}
			if(mP2Ship != null)
			{
				mP2Ship.EnableHover();
			}
		}
	}
	IEnumerator StartActivationDelay()
	{
		yield return new WaitForSeconds(mActivationDelay);
		mP1Ship = FindObjectOfType<PlayerOneShipController>();
		mP2Ship = FindObjectOfType<PlayerTwoShipController>();
		GetComponent<BoxCollider>().enabled = true;
		mGraphicEffect.SetActive(true);
	}


	void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<PlayerShipController>() != null)
		{
			GetComponent<BoxCollider>().enabled = false;
			mGraphicEffect.SetActive(false);

			if(mP1Ship != null)
			{
				mP1Ship.DisableHover(false, 60f);
			}
			if(mP2Ship != null)
			{
				mP2Ship.DisableHover(false, 60f);
			}
		}
	}


}


