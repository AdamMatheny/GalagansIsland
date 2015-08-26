using UnityEngine;
using System.Collections;

public class LDStinger : MonoBehaviour {

	public int mHitDamage = 1;

	public void OnTriggerEnter(Collider other)
	{
		
		//		if (other.gameObject.tag == "Player") 
		//		{
		//			if(other.gameObject.GetComponent<HeroShipAI>().mInvincibleTimer <= 0f)
		//			{
		//				other.gameObject.GetComponent<HeroShipAI>().HitHeroShip(mHitDamage);
		//			}
		//		}
		
		
		//else
		if (other.gameObject.name == "ShipCore") 
		{
			Debug.Log (gameObject.name + " hit ship core");
			if(other.transform.parent.gameObject.GetComponent<HeroShipAI>().mInvincibleTimer <= 0f)
			{
				other.transform.parent.gameObject.GetComponent<HeroShipAI>().HitHeroShip(mHitDamage);
			}
			transform.GetChild(0).SetParent (null);
			Destroy(gameObject);
		}
	}
}
