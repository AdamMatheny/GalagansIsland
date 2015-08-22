using UnityEngine;
using System.Collections;

public class BossBulletRotater : MonoBehaviour {

	public Rigidbody2D rgb2d;

	void Start(){

		rgb2d = GetComponent<Rigidbody2D> ();
	}

	void Update(){

		transform.rotation = new Quaternion(Input.GetAxis ("RightAnalogHorizontal"), Input.GetAxis ("RightAnalogVertical"), 0, 0);
	}
	
}
