using UnityEngine;
using System.Collections;

public class TileRunner : Tile 
{
	public Tile.Direction setMoveDir;
	
	private Vector2int moveDir;
	
	//private Vector2int pushedPreviousPos;
	
	private Tile shouldMove;
	private Vector2int previousPos;
	
	//private bool rotationCheck;
	
	public override void Initialize ()
	{
		base.Initialize ();
		
		moveDir = Tile.GetDirectionVector(setMoveDir);	
		//rotationCheck = 1;
	}
	
	public override void CalculateForce () 
	{		
		//Rotate if blocked	
		if(shouldMove && shouldMove.pos == previousPos)
		{
			Rotate();	
		}
		
		//Push
		Tile tile = Level.Instance.GetTile(pos + moveDir);	
		
		if(tile != null && tile.pushable)
		{
			tile.AddForce(moveDir);	
			shouldMove = tile;
		}
		else
		{
			AddForce(moveDir);
			shouldMove = this;
		}			
		
		
		previousPos = shouldMove.pos;
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
