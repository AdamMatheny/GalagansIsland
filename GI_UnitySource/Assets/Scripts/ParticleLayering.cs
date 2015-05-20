using UnityEngine;
using System.Collections;

public class ParticleLayering : MonoBehaviour {

    public string sortLayerString = "";

	// Use this for initialization
	void Start () {

        GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = sortLayerString;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
