using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : WorldObject 
{
	public bool organic = true;
	public bool pushable = false;
	public GameObject visuals;
	
	[HideInInspector]
	public Vector2int pos;
	
	protected Vector3 targetPos;
	protected Vector2int force;
	protected float moveSpeed = 13f;
	
	public virtual void Initialize () 
	{
		var x = Mathf.RoundToInt(transform.position.x);
		var y = Mathf.RoundToInt(transform.position.z); 
		
		pos = new Vector2int(x, y);
		
		force = new Vector2int(0,0);
		
		targetPos = transform.position;
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

		//Force adjustment mainly for diagonal forces
		//***Diagonal forces will currently not be influenced organics
		Tile tileX = Level.Instance.GetTile(pos.x + force.x, pos.y);
		if(tileX != null && !tileX.organic) force.x = 0; 
		
		Tile tileY = Level.Instance.GetTile(pos.x, pos.y + force.y);
		if(tileY != null && !tileY.organic) force.y = 0;
		
		//Without force there is no movement
		if(force.x == 0 && force.y == 0)
		{
			return false;	
		}
		
		//Add Force
		Vector2int p = new Vector2int(pos.x, pos.y);
		p.x += force.x;
		p.y += force.y;
		
		//Collision Check
		bool valid = Level.Instance.AttemptMove(p.x, p.y, this);
		
		if(valid) MoveTile(p);
		
		return valid;
	}
		
	public virtual void MoveTile(Vector2int p)
	{
		Vector3 newPos = new Vector3(p.x, 0, p.y);
		
		Vector3 dir = newPos - transform.position;
		
		if(visuals!=null)
		{
			visuals.transform.LookAt(newPos + dir);		
		}
		
		targetPos = newPos;
		
		pos = p;
	}
	
	
	public virtual void FixedUpdate()
	{
		if(transform.position != targetPos)
		{
			Vector3 pos = transform.position;
			
			pos += (targetPos-pos) * Time.deltaTime * moveSpeed * Level.Instance.moveFactor;
			
			transform.position = pos;
		}
	}
	
	public virtual void Remove()
	{
		Destroy(gameObject);
	}
}
