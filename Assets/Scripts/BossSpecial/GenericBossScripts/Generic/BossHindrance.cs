using UnityEngine;
using System.Collections;

public class BossHindrance : MonoBehaviour {

	public GameObject cameraShader;

	//private float HindranceTimer = .5f;

	// Use this for initialization
	void Start () {
	
		cameraShader = GameObject.FindGameObjectWithTag ("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
	
		//if (HindranceTimer > 0) {
		//
		//	HindranceTimer -= Time.deltaTime;
		//} else {
		//
		//	HindranceTimer = .5f;
		//	cameraShader.GetComponent<CameraShader> ().shader1.enabled = false;
		//	cameraShader.GetComponent<CameraShader> ().shader2.enabled = false;
		//}
	}

	public void OnTriggerEnter(Collider other){

		if (other.GetComponent<PlayerShipController>()!= null) {

			cameraShader.GetComponent<CameraShader> ().shader1.enabled = true;
			cameraShader.GetComponent<CameraShader> ().shader2.enabled = true;

			//Debug.Log("Hindrance!");
			
			//StartCoroutine(Hindrance());
		}
	}

	public void OnTriggerExit(Collider other){
		
		if(other.GetComponent<PlayerShipController>()!= null) {
			
			cameraShader.GetComponent<CameraShader> ().shader1.enabled = false;
			cameraShader.GetComponent<CameraShader> ().shader2.enabled = false;
		}
	}
}
