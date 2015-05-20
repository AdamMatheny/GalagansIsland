using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreWatcher : MonoBehaviour {

    public int currScore = 0;
   // private GUIText scoreMesh = null;
    public Text scoreMesh;
	// Use this for initialization
	void Start () {
        //scoreMesh = gameObject.GetComponent<Text>();//gameObject.GetComponent<TextMesh>();
        scoreMesh.text = "0";	
	}

    void OnEnable()
    {
        EnemyControllerScript.enemyDied += addScore;
    }

    void OnDisable()
    {
        EnemyControllerScript.enemyDied -= addScore;
    }

    public void addScore(int scoreToAdd)
    {
        currScore += scoreToAdd;
        scoreMesh.text = currScore.ToString();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
