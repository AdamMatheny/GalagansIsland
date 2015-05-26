using UnityEngine;
using System.Collections;

public class AspectRatioPositionScaleAdjuster : MonoBehaviour 
{
	//Adjust the position and scale of a Canvas UI element based on what aspect ratio is employed -Adam
	[SerializeField] private Vector3 SixteenByNinePos;
	[SerializeField] private Vector3 SixteenByNineScale;

	[SerializeField] private Vector3 SixteenByTenPos;
	[SerializeField] private Vector3 SixteenByTenScale;

	[SerializeField] private Vector3 FourByThreePos;
	[SerializeField] private Vector3 FourByThreeScale;

	[SerializeField] private Vector3 FiveByFourPos;
	[SerializeField] private Vector3 FiveByFourScale;


	// Use this for initialization
	void Start () 
	{

		if(System.Math.Round(Camera.main.aspect,2) == System.Math.Round(16f/9f,2))
		{
			GetComponent<RectTransform>().localPosition = SixteenByNinePos;
			GetComponent<RectTransform>().localScale = SixteenByNineScale;
		}
		else if(System.Math.Round(Camera.main.aspect,2) == System.Math.Round(16f/10f,2))
		{
			GetComponent<RectTransform>().localPosition = SixteenByTenPos;
			GetComponent<RectTransform>().localScale = SixteenByTenScale;
		}
		else if(System.Math.Round(Camera.main.aspect,2) ==System.Math.Round(4f/3f,2))
		{
			GetComponent<RectTransform>().localPosition = FourByThreePos;
			GetComponent<RectTransform>().localScale = FourByThreeScale;
		}
		else if(System.Math.Round(Camera.main.aspect,2) == System.Math.Round(5f/4f,2))
		{
			GetComponent<RectTransform>().localPosition = FiveByFourPos;
			GetComponent<RectTransform>().localScale = FiveByFourScale;
		}

	}

	void Update()
	{

	}

}
