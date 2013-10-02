using UnityEngine;
using System.Collections;

public class TileRunner : Tile 
{
	public Vector2int dir = new Vector2int(1, 0);
	
	public override void SetTileForce () 
	{
		Tile tile;
		Vector2int p;
		
		AddForce(dir.x, dir.y);
		
		//Attractor
		p = new Vector2int(pos.x, pos.y);
		p.x += dir.x;
		p.y += dir.y;

		tile = Level.Instance.GetTile(p.x, p.y);	
		
		if(tile !=null && tile.pushable)
		{
			
		}
		
		//Vector2int dir = Level.AdjacentTile[i];
		
		/*for(int i=0, count=Level.AdjacentTile.Length; i < count; i++)
		{
			Vector2int dir = Level.AdjacentTile[i];
			
			//Attractor
			p = new Vector2int(pos.x, pos.y);
			p.x += dir.x * 2;
			p.y += dir.y * 2;

			targetTile = Level.Instance.GetTile(p.x, p.y);	
				
			
			if(targetTile != null && (targetTile.movable || !targetTile.environment))
			{
				
				//Collision check
				p = new Vector2int(pos.x, pos.y);
				p.x += dir.x;
				p.y += dir.y;
				
				tile = Level.Instance.GetTile(p.x, p.y);
				
				if(tile == null)
				{					
					targetTile.AddForce(-dir.x, -dir.y);
				}
			}
		}*/
	}
}
