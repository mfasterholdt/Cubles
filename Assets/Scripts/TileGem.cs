using UnityEngine;
using System.Collections;

public class TileGem : Tile 
{
	public override void AddForce(int x, int y)
	{	
		
		Tile  tile = Level.Instance.GetTile(pos.x + x, pos.y + y);
		
		if(tile != null && tile.movable)
		{
			//Propegate force
			tile.AddForce(x, y);
		}
		else 
		{
			//Only add force on empty
			if(tile == null)
			{
				force.x += x;
				force.y += y;
			}
		}
	}
}
