using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : WorldObject 
{
	public bool organic = true;
	public bool pushable = false;
	public GameObject visuals;
	public int age = 0;
	public bool debug;
	
	[HideInInspector]
	public Vector2int pos;
	
	protected Vector3 targetPos;
	protected Vector3 dir;
	
	protected Vector2int force;
	protected float moveSpeed = 13f;
	protected int ageTimer;
	
	public virtual void Initialize () 
	{
		pos = new Vector2int(transform.position.x, transform.position.z);
		
		force = Vector2int.zero;
		
		targetPos = transform.position;
		
		ageTimer = age;
	}
	
	public virtual void AddForce(int x, int y)
	{				
		//Add force
		force.x += x;
		force.y += y;
	}
	public virtual void AddForce(Vector2int f){	AddForce(f.x, f.y);	}
	
	public virtual void UpdateTileForce()
	{
		if(CheckAge())
		{
			SetTileForce();
		}
	}
	
	public virtual bool CheckAge()
	{
		if(age == 0) return true;
		
		if(ageTimer > 0)
		{
			ageTimer--;
			return false;
		}
		else
		{
			ageTimer = age;
			return true;
		}
		
	}
	
	public virtual void SetTileForce()
	{

	}
	
	public virtual void UpdateTile()
	{
		if(force != Vector2int.zero)
		{
			AttempMove();
			
			force.x = 0;
			force.y = 0;
		}
	}
	
	public virtual bool AttempMove()
	{
		//Clamp force
		force.Clamp();
		
		//Force adjustment mainly for diagonal forces
		
		//***Diagonal forces will currently not be influenced organics
		Tile tileX = Level.Instance.GetTile(pos.x + force.x, pos.y);
		if(tileX != null && !tileX.organic) force.x = 0; 
		
		Tile tileY = Level.Instance.GetTile(pos.x, pos.y + force.y);
		if(tileY != null && !tileY.organic) force.y = 0;
		
		//Without force there is no movement
		if(force == Vector2int.zero)
		{
			return false;	
		}
		
		//Add Force
		Vector2int p = pos + force;
		
		//Collision Check
		bool valid = Level.Instance.AttemptMove(p, this);
		
		if(valid) MoveTile(p);
		
		return valid;
	}

	public virtual void MoveTile(Vector2int p)
	{
		//Visual move target
		Vector3 newPos = p.ToVector3();
		targetPos = newPos;
		
		//Visual Rotation
		if(visuals!=null)
		{
			dir.x = p.x - pos.x;
			dir.z = p.y - pos.y;
			
			visuals.transform.LookAt(visuals.transform.position + dir);		
		}
		
		//Logic move
		pos = p;
	}
	
	
	public virtual void FixedUpdate()
	{
		if(transform.position != targetPos)
		{
			Vector3 pos = transform.position;
			
			pos += (targetPos - pos) * Time.deltaTime * moveSpeed * Level.Instance.moveFactor;
			
			transform.position = pos;
		}
	}
	
	public virtual void Remove()
	{
		Destroy(gameObject);
	}
}
