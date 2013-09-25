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
		var y = Mathf.RoundToInt(transform.position.z); 
		
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
		
		if(valid) MoveTile(p);
		
		return valid;
	}
	
	public virtual void MoveTile(Vector2int pos)
	{
		Vector3 newPos = new Vector3(pos.x, 0, pos.y);
		
		Vector3 dir = newPos - transform.position;
		
		transform.LookAt(newPos + dir);		
		transform.position = newPos;
		
		this.pos = pos;
	}
}
