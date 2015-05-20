using UnityEngine;
using System.Collections;

public class AsteroidCollision : MonoBehaviour 
{

	public GameObject pieceA;
	public GameObject pieceB;
	public GameObject pieceC;
	public GameObject pieceD;
	public GameObject pieceE;
	public GameObject pieceF;
	public GameObject pieceG;


	public GameObject mLaserFistEmblem;
	public GameObject mBigBlastEmblem;

	void OnTriggerEnter(Collider other)
	{
		
		if (other.gameObject.GetComponent<PlayerBulletController> () != null) 
		{
			AsteroidDeath();
			Destroy(other.gameObject);
		}
	}

	public void AsteroidDeath()
	{
		//Update the Asteroid death count
		PlayerPrefs.SetInt("AsteroidCount", PlayerPrefs.GetInt("AsteroidCount") + 1);

		GameObject newAsteroidBit = Instantiate(pieceA, transform.position, Quaternion.identity) as GameObject;
		newAsteroidBit.transform.localScale = transform.parent.localScale;
		newAsteroidBit.transform.localScale = transform.parent.localScale;
		newAsteroidBit = Instantiate(pieceB, transform.position, Quaternion.identity) as GameObject;
		newAsteroidBit.transform.localScale = transform.parent.localScale;
		newAsteroidBit = Instantiate(pieceC, transform.position, Quaternion.identity) as GameObject;
		newAsteroidBit.transform.localScale = transform.parent.localScale;
		newAsteroidBit = Instantiate(pieceD, transform.position, Quaternion.identity) as GameObject;
		newAsteroidBit.transform.localScale = transform.parent.localScale;
		newAsteroidBit = Instantiate(pieceE, transform.position, Quaternion.identity) as GameObject;
		newAsteroidBit.transform.localScale = transform.parent.localScale;
		newAsteroidBit = Instantiate(pieceF, transform.position, Quaternion.identity) as GameObject;
		newAsteroidBit.transform.localScale = transform.parent.localScale;
		newAsteroidBit = Instantiate(pieceD, transform.position, Quaternion.identity) as GameObject;
		newAsteroidBit.transform.localScale = transform.parent.localScale;

		newAsteroidBit = Instantiate(pieceA, transform.position, Quaternion.identity) as GameObject;
		newAsteroidBit.transform.localScale = transform.parent.localScale;
		newAsteroidBit = Instantiate(pieceB, transform.position, Quaternion.identity) as GameObject;
		newAsteroidBit.transform.localScale = transform.parent.localScale;
		newAsteroidBit = Instantiate(pieceC, transform.position, Quaternion.identity) as GameObject;
		newAsteroidBit.transform.localScale = transform.parent.localScale;
		newAsteroidBit = Instantiate(pieceD, transform.position, Quaternion.identity) as GameObject;
		newAsteroidBit.transform.localScale = transform.parent.localScale;
		newAsteroidBit = Instantiate(pieceE, transform.position, Quaternion.identity) as GameObject;
		newAsteroidBit.transform.localScale = transform.parent.localScale;
		newAsteroidBit = Instantiate(pieceF, transform.position, Quaternion.identity) as GameObject;
		newAsteroidBit.transform.localScale = transform.parent.localScale;
		newAsteroidBit = Instantiate(pieceD, transform.position, Quaternion.identity) as GameObject;
		newAsteroidBit.transform.localScale = transform.parent.localScale;

		//float spawnChance = Random.Range(1,1000);
		//if(spawnChance == 777)
		if(PlayerPrefs.GetInt("AsteroidCount") >= PlayerPrefs.GetInt("AsteroidRequiredCount"))
		{
			if(PlayerPrefs.GetInt("SpawnLaserFist") == 0)
			{
				Instantiate(mLaserFistEmblem, new Vector3(transform.position.x, transform.position.y, -2f), Quaternion.identity);
				PlayerPrefs.SetInt("SpawnLaserFist",1);
			}
			else if(PlayerPrefs.GetInt("SpawnLaserFist") == 1)
			{
				Instantiate(mBigBlastEmblem, new Vector3(transform.position.x, transform.position.y, -2f), Quaternion.identity);
				PlayerPrefs.SetInt("SpawnLaserFist",0);
			}

			PlayerPrefs.SetInt("AsteroidRequiredCount", PlayerPrefs.GetInt("AsteroidRequiredCount") + Random.Range(175,250));

		}

		Destroy(transform.parent.gameObject);
	}
}