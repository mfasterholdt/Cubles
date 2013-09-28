using UnityEngine;
using System.Collections;

public class TileGem : Tile 
{
	public override void AddForce(int x, int y)
	{	
		//Propegate force
		Tile  tile = Level.Instance.GetTile(pos.x + x, pos.y + y);
		
		if(tile != null && tile.movable)
		{
			tile.AddForce(x, y);
		}
		else
		{
			//Add force
			force.x += x;
			force.y += y;
		}
	}
}
