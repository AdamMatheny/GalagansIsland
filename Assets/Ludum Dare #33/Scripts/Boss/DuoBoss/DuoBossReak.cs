using UnityEngine;
using System.Collections;

public class DuoBossReak : BossGenericScript {
	
	public bool leftHornAlive = true;
	public bool rightHornAlive = true;
	
	//public SpriteRenderer spriter;
	
	public GameObject tail;
	public GameObject head;
	public GameObject stomach;
	
	public override void Start ()
	{
		//spriter = GetComponent<SpriteRenderer> ();
		base.Start ();
	}
	
	public override void Update ()
	{
		if (!leftHornAlive && rightHornAlive) {
			
			tail.SetActive(false);
		}else{
			
			if(leftHornAlive && !rightHornAlive){
				
				head.SetActive(false);
			}else{
				
				if(!leftHornAlive && !rightHornAlive){
					
					stomach.SetActive(false);
				}
			}
		}
		
		base.Update ();
	}
}
