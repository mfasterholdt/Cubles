using UnityEngine;
using System.Collections;

public class TileParserRunner : Tile 
{
	public Tile.Direction setMoveDir;
	public bool initSpark;
	
	private Vector2int moveDir;
	
	public override void Initialize ()
	{
		base.Initialize ();
		
		Vector2int d = Tile.GetDirectionVector(setMoveDir);	
		
		if(initSpark)
		{
			GainSpark(d);
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
				if(tile != null && tile.organic)
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
			force = moveDir;
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
		moveDir = dir;
		countSparks++;
	}

	public override void HandleEnd()
	{
		Tile tile = Level.Instance.GetTile(pos + moveDir);
		
		if(tile != null && !tile.organic)
		{
			Rotate();
		}
	}
	
	public void Rotate()
	{
		//Clock wise rotation
		if(moveDir == Vector2int.up)
		{
			moveDir = Vector2int.right;
		}
		else if(moveDir == Vector2int.right)
		{
			moveDir = Vector2int.down;
		}
		else if(moveDir == Vector2int.down)
		{
			moveDir = Vector2int.left;
		}
		else if(moveDir == Vector2int.left)
		{
			moveDir = Vector2int.up;
		}
	}
}
