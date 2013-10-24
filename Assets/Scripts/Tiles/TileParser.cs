using UnityEngine;
using System.Collections;

public class TileParser : Tile 
{
	public Tile.Direction setMoveDir;
	public bool initSpark;
	
	public override void Initialize ()
	{
		base.Initialize ();
		
		Vector2int moveDir = Tile.GetDirectionVector(setMoveDir);	
		
		if(initSpark)
		{
			GainSpark(moveDir);
			sparking = true;
		}
	}
	
	public override void HandlePrepare()
	{
		if(sparking)
		{
			bool loseSpark = false;
			
			for(int i=0, count = Vector2int.Adjacent.Length; i < count; i++)
			{
				Vector2int dir = Vector2int.Adjacent[i];

				Tile tile = Level.Instance.GetTile(pos + dir);	
				
				//Shock
				if(tile != null)
				{
					tile.GainSpark(dir);
					
					loseSpark = true;
				}
			}
			
			if(loseSpark) 
			{
				LoseSpark();
			}
		}
	}
	
	public override void CalculateForce () 
	{	
		if(countSparks > 0)
		{
			countSparks = 1; //Absorb all but one spark
			sparking = true;
			/*if(sparkForce != Vector2int.zero)
			{
				AddForce(sparkForce);
				sparkForce = Vector2int.zero;
			}*/
		}
		else
		{
			force = Vector2int.zero;
			sparking = false;
		}
	}
	
	public override void LoseSpark()
	{	
		countSparks--;
	}

	public override void GainSpark (Vector2int dir)
	{	
		AddForce(dir);
		
		countSparks++;
	}
	
	/*public void SwapDirection()
	{
		//U-turn
		moveDir = moveDir * -1;
		
		if(visuals != null)
		{
			visuals.transform.LookAt(visuals.transform.position + moveDir.ToVector3());		
		}
	}*/
	
	public override void HandleEnd()
	{
		
	}
}
