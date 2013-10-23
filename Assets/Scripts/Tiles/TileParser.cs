using UnityEngine;
using System.Collections;

public class TileParser : Tile 
{
	public Tile.Direction setMoveDir;
	private Vector2int moveDir;
	
	public bool sparking;
	
	public override void Initialize ()
	{
		base.Initialize ();
		
		moveDir = Tile.GetDirectionVector(setMoveDir);	
		
		if(sparking)
		{
			GainSpark(moveDir.x, moveDir.y);
		}
	}
	
	public override void CalculateForce () 
	{	
		if(!sparking) return;

		Tile tile = Level.Instance.GetTile(pos + moveDir);		
		
		if(tile == null)
		{
			AddForce(moveDir);
			//if(!tile.organic) SwapDirection();	
		}
	}
	
	public override void AddForce(int x, int y)
	{			
		if(!sparking) GainSpark(x, y);
		
		//Add force
		force.x += x;
		force.y += y;
	}
	
	public void LoseSpark()
	{
		force = Vector2int.zero;
		sparking = false;
	}
	
	public override void GainSpark (Vector2int dir)
	{
		sparking = true;
		
		moveDir.x = dir.x;
		moveDir.y = dir.y;
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
	
	public override void HandleEnd()
	{
			
		bool giveSpark = false;
		
		for(int i=0, count = Vector2int.Adjacent.Length; i < count; i++)
		{
			Vector2int dir = Vector2int.Adjacent[i];
			
			//Shock
			Tile tile = Level.Instance.GetTile(pos + dir);	
			
			if(tile != null && tile.organic)
			{
				tile.GainSpark(dir);
				giveSpark = true;
			}
		}
		
		if(giveSpark) 
		{
			LoseSpark();
		}
		
		
		if(!sparking)
		{
			force = Vector2int.zero;
		}
	}
}
