using UnityEngine;
using System.Collections;

public class TilePush : Tile 
{
	public override void CalculateForce () 
	{
		for(int i = 0, count = Vector2int.Adjacent.Length; i < count; i++)
		{
			Vector2int dir = Vector2int.Adjacent[i];
			
			Tile targetTile = Level.Instance.GetTile(pos + dir);		
			
			if(targetTile != null && targetTile.pushable)
			{
				targetTile.AddForce(dir);
			}
		}
	}
}
