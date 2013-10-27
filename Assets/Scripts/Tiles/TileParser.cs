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
			sparkEffect.SetActive(true);
		}
		else
		{
			sparkEffect.SetActive(false);
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
			sparkEffect.SetActive(true);
		}
		else
		{
			force = Vector2int.zero;
			sparking = false;
			sparkEffect.SetActive(false);
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

	public override void HandleEnd()
	{
		
	}
}
