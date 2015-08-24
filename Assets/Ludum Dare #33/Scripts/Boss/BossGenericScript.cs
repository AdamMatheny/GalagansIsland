using UnityEngine;
using System.Collections;

public class BossGenericScript : MonoBehaviour {

	//Put generic boss code here

	public bool leftHornAlive = true;
	public bool rightHornAlive = true;

	public SpriteRenderer spriter;

	public Sprite rightHorn;
	public Sprite leftHorn;
	public Sprite noHorns;

	//public int health;

	public Rigidbody2D rgb2d;

	public void Start(){
	
		spriter = GetComponent<SpriteRenderer> ();
		rgb2d = GetComponent<Rigidbody2D> ();
	}

	public virtual void Update(){

		if (!leftHornAlive && rightHornAlive) {

			spriter.sprite = rightHorn;
		}else{

			if(leftHornAlive && !rightHornAlive){

				spriter.sprite = leftHorn;
			}else{

				if(!leftHornAlive && !rightHornAlive){

					spriter.sprite = noHorns;
				}
			}
		}

		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		rgb2d.velocity = new Vector2 (horizontal * 10, vertical * 10);
	}

	//public void TakeDamage(){

		//health --;
	//}
}