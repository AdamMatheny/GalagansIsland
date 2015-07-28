using UnityEngine;
using System.Collections;

public class KillinTime : MonoBehaviour {

	public float maxTime;
	public float timeTiKill;
	
	// Update is called once per frame
	void Update () {
	
		timeTiKill -= Time.deltaTime;
		if (timeTiKill <= 0) {

			Destroy(gameObject);
		}
	}
}
