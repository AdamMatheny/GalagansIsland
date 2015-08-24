using UnityEngine;
using System.Collections;

public class BossEye : MonoBehaviour {

	public GameObject BuildUp;
	
	public GameObject mTarget;
	
	public GameObject bullet;

	public int health;
	
	public float timer;
	float timerTemp;
	
	public void Start(){
		
		mTarget = GameObject.FindGameObjectWithTag ("Player");
		
		timerTemp = timer;
	}
	
	public void Update(){

		if (timerTemp < 1) {

			BuildUp.SetActive (true);
		} else {

			BuildUp.SetActive(false);
		}
		
		if (timerTemp > 0) {
			
			timerTemp -= Time.deltaTime;
		} else {
			
			timerTemp = timer;
			Instantiate(bullet, transform.position + new Vector3(0, 4), Quaternion.identity);
			Debug.Log("SHOOT!");
		}
		
		float horizontal = Input.GetAxis ("RightAnalogHorizontal");
		float vertical = Input.GetAxis ("RightAnalogVertical");
		
		transform.localPosition = new Vector2 (horizontal / 15, (vertical / 15) + .04f);
	}

	public void TakeDamage(){

		if (GetComponentInParent<Boss1> ().leftHornAlive == false) {

			if (GetComponentInParent<Boss1> ().rightHornAlive == false) {
				
				health --;
			}
		}

		if (health <= 0) {

			BlowUpEye();
		}
	}

	public void BlowUpEye(){

		Destroy (gameObject);
	}
}
