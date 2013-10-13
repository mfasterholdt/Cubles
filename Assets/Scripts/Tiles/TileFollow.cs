using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileFollow : Tile 
{
	public override void CalculateForce () 
	{	
		for(int i=0, count = Vector2int.Adjacent.Length; i < count; i++)
		{
			Vector2int dir = Vector2int.Adjacent[i];
			
			//Attractor
			Tile tile = Level.Instance.GetTile(pos + dir * 2);	
			
			if(tile != null && tile.organic)
			{
				//Collision check
				tile = Level.Instance.GetTile(pos + dir);
				
				if(tile == null ) 
				{
					AddForce(dir);	
				}
			}
		}
	}
}