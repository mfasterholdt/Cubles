using UnityEngine;
using System.Collections;

public class Selection : WorldObject 
{
	public GameObject visuals;
	public delegate void SelectionEventDelegate (Selection sender, Vector2int pos);
	public event SelectionEventDelegate OnMouseClick;
	
	Vector2int mousePosition;
	
	void Update () 
	{
		MoveSelection();
		
		if (mousePosition != null && Input.GetMouseButtonUp(0))
		{		
			if(OnMouseClick != null)
			{
				OnMouseClick(this, mousePosition);
			}
		}   
	}
	
	void MoveSelection()
	{
		Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);		
		int x = Mathf.RoundToInt(p.x);
		int y = Mathf.RoundToInt(p.z + 12.5f);

		if(x < 0 || x >= Level.WorldSize || y < 0 || y >= Level.WorldSize)
		{
			visuals.SetActive(false);
			
			mousePosition = null;
		}
		else
		{
			visuals.SetActive(true);			
			
			mousePosition = new Vector2int(x, y);
			transform.position = new Vector3(mousePosition.x, 0, mousePosition.y);
		}
	}
}
