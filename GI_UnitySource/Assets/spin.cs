using UnityEngine;
using System.Collections;

public class spin : MonoBehaviour {

	public Rigidbody rb;

	// Use this for initialization
	void Start () {
	
		rb = GetComponent<Rigidbody>();
		rb.angularVelocity = new Vector3 (0f, 0f, 500f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
