using UnityEngine;
using System.Collections;

public class EscapeGame : MonoBehaviour {
	
	void Update () {
	
		if (Input.GetKey (KeyCode.Escape))
			Application.Quit ();
	}
}
