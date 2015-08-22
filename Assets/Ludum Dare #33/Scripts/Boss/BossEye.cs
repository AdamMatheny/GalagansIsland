using UnityEngine;
using System.Collections;

public class BossEye : MonoBehaviour {

	public void Update(){

		float horizontal = Input.GetAxis ("RightAnalogHorizontal");
		float vertical = Input.GetAxis ("RightAnalogVertical");

		transform.localPosition = new Vector2 (horizontal / 15, (vertical / 15) + .04f);
	}
}
