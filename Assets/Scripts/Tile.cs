using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : WorldObject 
{
	[HideInInspector]
	public Vector2int pos;
	public bool environment = false;
	public bool movable = false;
	
	protected Vector2int force;
	
	public void Awake()
	{
		Initialize();
	}
	
	public void Initialize () 
	{
		var x = Mathf.RoundToInt(transform.position.x);
		var y = Mathf.RoundToInt(transform.position.y); 
		
		pos = new Vector2int(x, y);
	}
	
	public virtual void AddForce(int x, int y)
	{
		//Add force
		force.x += x;
		force.y += y;
	}
	
	public virtual void SetTileForce(){}
	
	public virtual void UpdateTile()
	{
		if(force != null)
		{
			AttempMove(force.x, force.y);
		}
	}
	
	public virtual bool AttempMove(int x, int y)
	{
		Tile tileX = Level.Instance.GetTile(pos.x + force.x, pos.y);
		if(tileX != null) force.x = 0;
		
		Tile tileY = Level.Instance.GetTile(pos.x, pos.y + force.y);
		if(tileY != null) force.y = 0;
		
		Vector2int p = new Vector2int(pos.x, pos.y);
		p.x += force.x;
		p.y += force.y;
		
		bool valid = Level.Instance.AttemptMove(p.x, p.y, this);
		
		if(valid) MoveTile(p.x, p.y);
		
		return valid;
	}
	
	public virtual void MoveTile(int x, int y)
	{
		transform.position = new Vector3(x, y, 0);
		
		pos.x = x;
		pos.y = y;
	}
}
