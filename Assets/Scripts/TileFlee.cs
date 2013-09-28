using UnityEngine;
using System.Collections;

public class TileFlee : Tile 
{
		
	public override void SetTileForce () 
	{
		Tile tile;
		Vector2int p;
		
		for(int i=0, count=Level.AdjacentTile.Length; i < count; i++)
		{
			Vector2int dir = Level.AdjacentTile[i];
			
			//Scarer
			p = new Vector2int(pos.x, pos.y);
			p.x += dir.x;
			p.y += dir.y;
			
			tile = Level.Instance.GetTile(p.x, p.y);	
			
			if(tile != null && !tile.environment)
			{
				p = new Vector2int(pos.x, pos.y);
				p.x -= dir.x;
				p.y -= dir.y;
				
				tile = Level.Instance.GetTile(p.x, p.y);
				
				if(tile != null && tile.movable)
				{
					tile.AddForce(-dir.x, -dir.y);
				}
				else
				{
					AddForce(-dir.x, -dir.y);
				}
			}
		}
	}
	

}
