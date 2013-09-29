using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : WorldObject 
{
	[HideInInspector]
	public Vector2int pos;
	
	protected Vector3 targetPos;
	
	public bool environment = false;
	public bool movable = false;

	protected Vector2int force;
	
	protected float moveSpeed = 13f;
	
	private bool initialized = false;
	
	public void Initialize () 
	{
		var x = Mathf.RoundToInt(transform.position.x);
		var y = Mathf.RoundToInt(transform.position.z); 
		
		pos = new Vector2int(x, y);
		
		force = new Vector2int(0,0);
		
		targetPos = transform.position;
		
		initialized = true;
	}
	
	public virtual void AddForce(int x, int y)
	{				
		//Add force
		force.x += x;
		force.y += y;
	}
	
	public virtual void SetTileForce()
	{

	}
	
	public virtual void UpdateTile()
	{
		if(force.x != 0 || force.y != 0)
		{
			AttempMove(force.x, force.y);
			
			force.x = 0;
			force.y = 0;
		}
	}
	
	public virtual bool AttempMove(int x, int y)
	{
		//Clamp force
		force.x = Mathf.Clamp(force.x, -1, 1);
		force.y = Mathf.Clamp(force.y, -1, 1);
		
		//Moves
		Tile tileX = Level.Instance.GetTile(pos.x + force.x, pos.y);
		if(tileX != null) force.x = 0;
		
		Tile tileY = Level.Instance.GetTile(pos.x, pos.y + force.y);
		if(tileY != null) force.y = 0;
		
		Vector2int p = new Vector2int(pos.x, pos.y);
		p.x += force.x;
		p.y += force.y;
		
		bool valid = Level.Instance.AttemptMove(p.x, p.y, this);
		
		if(valid) MoveTile(p);
		
		return valid;
	}
		
	public virtual void MoveTile(Vector2int p)
	{
		Vector3 newPos = new Vector3(p.x, 0, p.y);
		
		Vector3 dir = newPos - transform.position;
		
		transform.LookAt(newPos + dir);		
		
		targetPos = newPos;
		
		//transform.position = newPos; //***instant move
		
		pos = p;
	}
	
	
	public void Update()
	{
		if(transform.position != targetPos)
		{
			
			Vector3 pos = transform.position;
			
			pos += (targetPos-pos) * Time.fixedDeltaTime * moveSpeed * Level.Instance.moveFactor;
			
			transform.position = pos;
		}
	}
}
