using UnityEngine;
using System.Collections;

public class LDStinger : MonoBehaviour {

	public GameObject cameraShader;

	public int mHitDamage = 1;

	public void Start(){

		cameraShader = GameObject.FindGameObjectWithTag ("MainCamera");
	}

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
			cameraShader.GetComponent<CameraShader> ().shader1.enabled = true;
			cameraShader.GetComponent<CameraShader> ().shader2.enabled = true;

			Debug.Log (gameObject.name + " hit ship core");
			if(other.transform.parent.gameObject.GetComponent<HeroShipAI>().mInvincibleTimer <= 0f)
			{
				other.transform.parent.gameObject.GetComponent<HeroShipAI>().HitHeroShip(mHitDamage);
			}
			transform.GetChild(0).SetParent (null);
			Destroy(gameObject);
		}
	}

	public void OnTriggerExit(Collider other){

		if (other.gameObject.name == "ShipCore") {

			cameraShader.GetComponent<CameraShader> ().shader1.enabled = false;
			cameraShader.GetComponent<CameraShader> ().shader2.enabled = false;
		}
	}
}
