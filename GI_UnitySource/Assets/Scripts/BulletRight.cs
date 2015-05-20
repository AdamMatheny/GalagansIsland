using UnityEngine;
using System.Collections;

public class BulletRight : MonoBehaviour 
{
	
	public float bulletSpeed = 20.0f;
	private float selfDestructTimer = 0.0f;
	
	public void Start()
	{
		bulletSpeed+= (20f/25f*Application.loadedLevel);
		Vector2 bulletForce;
		bulletForce = new Vector2(bulletSpeed / 5,bulletSpeed);
		
		GetComponent<Rigidbody>().velocity = bulletForce;
		selfDestructTimer = Time.time + 5.0f;
	}
	
	void Update()
	{
		if(selfDestructTimer < Time.time || transform.position.y >= 24f)
		{
			Destroy(gameObject);
		}
	}
}
