using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileFollow : Tile 
{
	
	public override void SetTileForce () 
	{	
		
		Tile tile;
		Vector2int p;
		
		for(int i=0, count=Level.AdjacentTile.Length; i < count; i++)
		{
			Vector2int dir = Level.AdjacentTile[i];
			
			//Attractor
			p = new Vector2int(pos.x, pos.y);
			p.x += dir.x * 2;
			p.y += dir.y * 2;

			tile = Level.Instance.GetTile(p.x, p.y);	
			
			if(tile != null && !tile.environment)
			{
				//Collision check
				p = new Vector2int(pos.x, pos.y);
				p.x += dir.x;
				p.y += dir.y;
				
				tile = Level.Instance.GetTile(p.x, p.y);
				
				if(tile == null) AddForce(dir.x, dir.y);
			}
		}
	}
}
