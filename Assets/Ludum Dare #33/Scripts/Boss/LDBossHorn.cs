using UnityEngine;
using System.Collections;

public class LDBossHorn : MonoBehaviour {

	public int health;

	public enum HornList{

		LeftHorn,
		RightHorn
	}

	public HornList hornNumber;

	public void TakeDamage(){

		health --;

		if (health <= 0) {

			BlowUpHorn();
		}
	}

	public void BlowUpHorn(){

		if (hornNumber == HornList.LeftHorn) {

			GetComponentInParent<Boss1> ().leftHornAlive = false;
		} else {

			GetComponentInParent<Boss1> ().rightHornAlive = false;
		}

		Destroy (gameObject);
	}
}
