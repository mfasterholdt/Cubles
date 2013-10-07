using UnityEngine;
using System.Collections;

public class TileGem : Tile 
{
	public override void AddForce(int x, int y)
	{	
		
		Tile  tile = Level.Instance.GetTile(pos.x + x, pos.y + y);
		
		if(tile != null && tile.pushable)
		{
			//Propegate force
			tile.AddForce(x, y);
		}
		else if(tile == null)
		{
			//Only add force on empty
			force.x += x;
			force.y += y;
		}
	}
}
