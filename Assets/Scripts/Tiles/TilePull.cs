using UnityEngine;
using System.Collections;

public class TilePull : Tile 
{
	public override void CalculateForce () 
	{
		for(int i = 0, count = Vector2int.Adjacent.Length; i < count; i++)
		{
			Vector2int dir = Vector2int.Adjacent[i];
			
			//Attractor
			Tile targetTile = Level.Instance.GetTile(pos + dir * 2);		
			
			if(targetTile != null && targetTile.pushable)
			{
				
				//Collision check
				Tile tile = Level.Instance.GetTile(pos + dir);
				
				if(tile == null)
				{					
					targetTile.AddForce(-dir);
				}
			}
		}
	}
}
