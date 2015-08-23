using UnityEngine;
using System.Collections;

public class LDBulletScript : EnemyBulletController {
	
	public void OnCollisionEnter(Collision other){
		
		if (other.gameObject.tag == "Player") {
			
			Destroy(gameObject);
		}
	}
}
