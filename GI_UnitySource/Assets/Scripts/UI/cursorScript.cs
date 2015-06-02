using UnityEngine;
using System.Collections;

public class cursorScript : MonoBehaviour 
{

	public Texture2D sprite;
	Vector2 center = Vector2.zero;

	void Start()
	{
		Cursor.SetCursor (sprite, center, CursorMode.Auto);
	}

	void Update()
	{
		//center = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
	}
}
