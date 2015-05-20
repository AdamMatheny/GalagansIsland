using UnityEngine;
using System.Collections;

public class ParallaxController : MonoBehaviour {

    public GameObject[] clouds;
    public GameObject[] nearHills;
    public GameObject[] farHills;

    public float cloudLayerSpeedModifier;
    public float nearHillLayerSpeedModifier;
    public float farHillSpeedModifier;

    public Camera myCamera;

    private Vector3 lasCamPos;
	// Use this for initialization
	void Start () {
        lasCamPos = myCamera.transform.position;
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 currCamPos = myCamera.transform.position;
        float xPosDiff = lasCamPos.x - currCamPos.x;

        adjustParallaxPositionsForArray(clouds, cloudLayerSpeedModifier, xPosDiff);
        adjustParallaxPositionsForArray(nearHills, nearHillLayerSpeedModifier, xPosDiff);
        adjustParallaxPositionsForArray(farHills, farHillSpeedModifier, xPosDiff);

        lasCamPos = myCamera.transform.position;
	
	}

    void adjustParallaxPositionsForArray(GameObject[] layerArray, float layerSpeedModifier, float xPosDiff)
    {
        for(int i =0; i< layerArray.Length; i++)
        {
            Vector3 objPos = layerArray[i].transform.position;
            objPos.x += xPosDiff * layerSpeedModifier;
            layerArray[i].transform.position = objPos;
        }
    }
}
