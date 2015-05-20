using UnityEngine;
using System.Collections;

public class RotateLeft : MonoBehaviour {
	
	[SerializeField] private GameObject spewwy;
	
	[SerializeField] private float timer;
	[SerializeField] private float timer2;
	
	public bool stopShooting = false;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		
		timer -= Time.deltaTime;
		if (timer <= 0) {
			
			stopShooting = true;
			
			timer2 -= Time.deltaTime;
			if(timer2 <= 0){
				
				stopShooting = false;
				timer2 = .5f;
				timer = 2;
			}
		}
		
		if (!stopShooting) {
			
			Instantiate (spewwy, transform.position, Quaternion.identity);
		}

		transform.Rotate (Vector3.forward * 7);
	}
}
