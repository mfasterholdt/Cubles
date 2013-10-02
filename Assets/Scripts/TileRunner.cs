using UnityEngine;
using System.Collections;

public class TileRunner : Tile 
{
	private Vector2int dir = new Vector2int(1, 0);
	
	public override void SetTileForce () 
	{
		AddForce(dir.x, dir.y);
	}
	
	public override void UpdateTile()
	{
		bool valid = AttempMove(force.x, force.y);
	
		if(!valid)
		{
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
