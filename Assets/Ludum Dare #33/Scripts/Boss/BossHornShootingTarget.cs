using UnityEngine;
using System.Collections;

public class BossHornShootingTarget : MonoBehaviour {

	public Vector2 offset;
	public Transform startingPosition;

	public GameObject bullet;

	public void Start(){
	}

	public void Update(){
		
		float horizontal = Input.GetAxis ("RightAnalogHorizontal");
		float vertical = Input.GetAxis ("RightAnalogVertical");
		
		transform.localPosition = new Vector2 (horizontal / 15 + offset.x, vertical / 15 + offset.y);

		Instantiate (bullet, startingPosition.position, Quaternion.identity);
	}
}
