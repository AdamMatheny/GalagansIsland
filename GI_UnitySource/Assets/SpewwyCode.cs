using UnityEngine;
using System.Collections;

public class SpewwyCode : MonoBehaviour {

	public Rigidbody rb;

	public float timer;

	void Update(){

		timer -= Time.deltaTime;
		if (timer <= 0) {

			Destroy(gameObject);
		}

		rb.AddForce(transform.forward * 5);
	}
}
