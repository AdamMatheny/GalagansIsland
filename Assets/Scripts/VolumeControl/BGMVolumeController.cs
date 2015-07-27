using UnityEngine;
using System.Collections;

public class BGMVolumeController : MonoBehaviour 
{
	float mStartingVolume;

	// Use this for initialization
	void Start () 
	{
		mStartingVolume = GetComponent<AudioSource>().volume;
//		PlayerPrefs.SetFloat("SFXVolume", 0.8f);
//		PlayerPrefs.SetFloat("BGMVolume", 0.8f);
		GetComponent<AudioSource>().ignoreListenerVolume = true;
		GetComponent<AudioSource>().volume = mStartingVolume * PlayerPrefs.GetFloat("BGMVolume");
	}
	
	// Update is called once per frame
	void Update () 
	{
		GetComponent<AudioSource>().volume = mStartingVolume * PlayerPrefs.GetFloat("BGMVolume");
		AudioListener.volume = PlayerPrefs.GetFloat("SFXVolume");
	}
}
