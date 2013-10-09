using UnityEngine;
using System.Collections;

public class TileRunner : Tile 
{
	private Vector2int moveDir = new Vector2int(1, 0);
	
	public override void SetTileForce () 
	{
		AddForce(moveDir);
		
		//Target
		Tile tile = Level.Instance.GetTile(pos + moveDir);	
		
		if(tile != null) 
		{
			if(tile.pushable)
			{
				tile.AddForce(moveDir);	
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
