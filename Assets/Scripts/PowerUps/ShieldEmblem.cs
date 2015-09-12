using UnityEngine;
using System.Collections;

public class ShieldEmblem : MonoBehaviour 
{
	
	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Translate(new Vector3(0f,-5f*Time.deltaTime,0f));

	}
	
	void OnTriggerEnter(Collider other)
	{

		if (other.tag == "SecondShip") 
		{

			other.GetComponentInParent<PlayerShipController> ().mShielded = true;
			other.GetComponentInParent<PlayerShipController> ().mShieldTimer = 30f;
			Destroy(this.gameObject);
		}

		if(other.GetComponent<PlayerShipController>() != null)
		{
			other.GetComponent<PlayerShipController>().mShielded = true;
			other.GetComponent<PlayerShipController>().mShieldTimer = 30f; 

			if(FindObjectOfType<LevelKillCounter>() != null)
			{
				if(other.GetComponent<PlayerOneShipController>() != null)
				{
					FindObjectOfType<LevelKillCounter>().mP1ShieldTime = 30f;
				}
				else if (other.GetComponent<PlayerTwoShipController>() != null)
				{
					FindObjectOfType<LevelKillCounter>().mP2ShieldTime = 30f;
				}
			}

			if(FindObjectOfType<GetReady>() != null)
			{
				if(other.GetComponent<PlayerOneShipController>() != null)
				{
					FindObjectOfType<GetReady>().mP1ShieldTime = 30f;
				}
				else if (other.GetComponent<PlayerTwoShipController>() != null)
				{
					FindObjectOfType<GetReady>().mP2ShieldTime = 30f;
				}
			}

			Destroy(this.gameObject);
		}

	}
}