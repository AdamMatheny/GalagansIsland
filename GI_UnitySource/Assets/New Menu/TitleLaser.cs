using UnityEngine;
using System.Collections;

public class TitleLaser : MonoBehaviour 
{
	public GameObject mMenuUI;

	public void DestroyShips()
	{
		mMenuUI.GetComponent<GetSome>().StartGame();
	}
}
