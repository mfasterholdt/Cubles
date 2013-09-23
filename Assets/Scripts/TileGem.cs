using UnityEngine;
using System.Collections;

public class TileGem : Tile 
{
	public override void SetTileForce () 
	{
		 force = new Vector2int(0, 0);
	}
}
