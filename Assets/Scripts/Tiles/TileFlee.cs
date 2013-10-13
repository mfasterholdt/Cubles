using UnityEngine;
using System.Collections;

public class TileFlee : Tile 
{
	public override void CalculateForce () 
	{
		for(int i=0, count = Vector2int.Adjacent.Length; i < count; i++)
		{
			Vector2int dir = Vector2int.Adjacent[i];
			
			//Scarer
			Tile tile = Level.Instance.GetTile(pos + dir);	
			
			if(tile != null && tile.organic)
			{
				//Collision Check				
				tile = Level.Instance.GetTile(pos - dir);
				
				if(tile != null && tile.pushable)
				{
					//Push Tile
					tile.AddForce(-dir);	
					
					//Still move self if organic
					if(tile.organic) 
					{
						AddForce(-dir); 
					}
				}
				else
				{
					AddForce(-dir);
				}
			}
		}
	}
	

}
