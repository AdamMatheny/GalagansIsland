﻿using UnityEngine;
using System.Collections;

public class BigBlastEmblem : MonoBehaviour 
{
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Translate(new Vector3(0f,-5f*Time.deltaTime,0f));
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<PlayerShipController>() != null)
		{
			other.GetComponent<PlayerShipController>().mHaveBigBlast = true;
			Destroy(this.gameObject);
		}
		if(other.GetComponent<PlayerShipCloneController>() != null)
		{
			FindObjectOfType<PlayerShipController>().mHaveBigBlast = true;
			Destroy(this.gameObject);
		}
	}
}