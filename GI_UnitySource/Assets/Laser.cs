using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

	public ScoreManager mScoreManager;


	void OnTriggerEnter(Collider other){

		//collided = true;

		if (other.gameObject.tag == "Ship") {

			Debug.Log ("HIT SHIP 1");
			//Application.LoadLevel(1);
		}

		if (other.gameObject.GetComponent<PlayerShipController> () != null) {

			Debug.Log ("HIT SHIP 2");
			mScoreManager.GetComponent<ScoreManager>().LoseALife();
		}
	}
}
