using UnityEngine;
using System.Collections;

public class BossGenericScript : MonoBehaviour {

	//Put generic boss code here



	//public int health;

	public Rigidbody2D rgb2d;

	public virtual void Start(){
	

		rgb2d = GetComponent<Rigidbody2D> ();
	}

	public virtual void Update(){



		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		rgb2d.velocity = new Vector2 (horizontal * 10, vertical * 10);
	}

	//public void TakeDamage(){

		//health --;
	//}
}