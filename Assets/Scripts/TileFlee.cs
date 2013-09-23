using UnityEngine;
using System.Collections;

public class TileFlee : Tile 
{
		
	public override void SetTileForce () 
	{
		Tile tile;
		Vector2int p;
		force = new Vector2int(0,0);
		
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
				
				if(tile != null && tile.movable) tile.AddForce(-dir.x, -dir.y);
				
				AddForce(-dir.x, -dir.y);
			}
		}
	}
	

}
