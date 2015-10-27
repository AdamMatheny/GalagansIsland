using UnityEngine;
using System.Collections;
using Assets.Scripts.IndieGamesLauncher;

public class IndieGameCollider : MonoBehaviour 
{
	ScoreManager mScoreMan;
	public string mGameTitle;
	public float mDriftSpeed = 5.47f;

	// Use this for initialization
	void Start () 
	{
		transform.position = new Vector3(Random.Range (-10,10), Random.Range (-20,20), -2);

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(mScoreMan == null)
		{
			mScoreMan = FindObjectOfType<ScoreManager>();
		}
		else if(mScoreMan.mPlayer2Avatar != null || mScoreMan.mP2Score > 0)
		{
			Destroy(this.gameObject);
		}
		transform.Translate (Vector3.down* mDriftSpeed*Time.deltaTime);

	}

	void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<PlayerOneShipController>() != null)
		{
			other.GetComponent<PauseManager>().Pause ();
			IndieGamesManager.instance.StartGame(mGameTitle);
			Destroy (this.gameObject);
		}
	}
}
