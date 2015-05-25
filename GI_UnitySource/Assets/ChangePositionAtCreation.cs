using UnityEngine;
using System.Collections;

public class ChangePositionAtCreation : MonoBehaviour {

	public int x, y, z;

	// Use this for initialization
	void Start () {

		transform.position = new Vector3 (transform.position.x + x, transform.position.y + y, transform.position.z + z);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
