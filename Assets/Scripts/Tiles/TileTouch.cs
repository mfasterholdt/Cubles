using UnityEngine;
using System.Collections;

public class TileTouch : Tile 
{
	public Tile.Direction setMoveDir;
	private Vector2int moveDir;
	
	public override void Initialize ()
	{
		base.Initialize ();
		
		moveDir = Tile.GetDirectionVector(setMoveDir);	
	}
	
	public override void CalculateForce () 
	{		
		//Push
		Tile tile = Level.Instance.GetTile(pos + moveDir);	
		
		if(tile != null)
		{
			if(tile.pushable) tile.AddForce(moveDir);	
				
			SwapDirection();			
		}
		else
		{
			AddForce(moveDir);
		}			
	}
	
	
	public void SwapDirection()
	{
		//U-turn
		moveDir = moveDir * -1;
		
		if(visuals != null)
		{
			visuals.transform.LookAt(visuals.transform.position + moveDir.ToVector3());		
		}
	}	
}
