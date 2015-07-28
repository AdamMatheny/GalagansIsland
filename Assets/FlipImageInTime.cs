using UnityEngine;
using System.Collections;

public class FlipImageInTime : MonoBehaviour {

	public float maxTime;
	public float timeToFlip;

	void Start(){

		timeToFlip = maxTime;
	}

	// Update is called once per frame
	void Update () {
	
		timeToFlip -= Time.deltaTime;

		if (timeToFlip <= 0) {

			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			timeToFlip = maxTime;
		}
	}
}
