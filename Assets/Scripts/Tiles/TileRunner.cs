using UnityEngine;
using System.Collections;

public class TileRunner : Tile 
{
	private Vector2int moveDir = new Vector2int(1, 0);
	
	public override void SetTileForce () 
	{
		AddForce(moveDir.x, moveDir.y);
		
		//Attractor
		Vector2int p = new Vector2int(pos.x, pos.y);
		p.x += moveDir.x;
		p.y += moveDir.y;
		
		Tile tile = Level.Instance.GetTile(p.x, p.y);	
		
		if(tile != null) 
		{
			if(tile.pushable)
			{
				tile.AddForce(moveDir.x, moveDir.y);	
			}
			else
			{
				//Clock wise rotation
				if(moveDir.x == 1)
				{
					moveDir.x = 0;
					moveDir.y = -1;
				}
				else if(moveDir.y == -1)
				{
					moveDir.x = -1;
					moveDir.y = 0;
				}
				else if(moveDir.x == -1)
				{
					moveDir.x = 0;
					moveDir.y = 1;
				}
				else if(moveDir.y == 1)
				{
					moveDir.x = 1;
					moveDir.y = 0;
				}
			}
		}
	}
}
