using UnityEngine;
using System.Collections;

public class Spew : MonoBehaviour {

	public GameObject spewwy;

	public int rotationAmmount;
	public int tempX = 0;
	public int tempY = 0;

	public Vector3 rotate;

	void Update(){

		tempX += rotationAmmount;
		tempY += rotationAmmount;

		rotate = new Vector3 (tempX, tempY);

		Instantiate(spewwy, transform.position, Quaternion.Euler(rotate));
	}
}
