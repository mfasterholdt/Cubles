using UnityEngine;
using System.Collections;

public class TileRunner : Tile 
{
	private Vector2int dir = new Vector2int(1, 0);
	
	public override void SetTileForce () 
	{
		AddForce(dir.x, dir.y);
		
		//Attractor
		Vector2int p = new Vector2int(pos.x, pos.y);
		p.x += dir.x;
		p.y += dir.y;
		
		Tile tile = Level.Instance.GetTile(p.x, p.y);	
		
		if(tile != null) 
		{
			if(tile.pushable)
			{
				tile.AddForce(dir.x, dir.y);	
			}
			else
			{
				//Clock wise rotation
				if(dir.x == 1)
				{
					dir.x = 0;
					dir.y = -1;
				}
				else if(dir.y == -1)
				{
					dir.x = -1;
					dir.y = 0;
				}
				else if(dir.x == -1)
				{
					dir.x = 0;
					dir.y = 1;
				}
				else if(dir.y == 1)
				{
					dir.x = 1;
					dir.y = 0;
				}
			}
		}
	}
}
